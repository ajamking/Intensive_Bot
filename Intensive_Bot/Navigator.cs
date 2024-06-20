using Exceptions;
using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Intensive_Bot;

public static class Navigator
{
    private static readonly Func<BotUser, Task<bool>>[] _handlers = new[]
    {
        HandleStartMessage,
        HandleKeyboardWordMessage,
        HandleAdminsAlertsCustomization,
        HandleAnyUnknownMessage,
    };



    public static readonly Dictionary<long, BotUser> BotUsers = new();

    public static void Execute(ITelegramBotClient botClient, Message message)
    {
        var currentBotUser = CheckInUser(botClient, message);

        ResetUserMessage(currentBotUser, message);

        BotLogger.LogUserMessage(currentBotUser);

        foreach (var handler in _handlers)
        {
            if (handler.Invoke(currentBotUser))
            {
                if (currentBotUser.Username == Program.BotEnvironment.AdminUsername)
                {
                    AnswerSender.ShowKeyboard(currentBotUser);
                }

                break;
            }
        }
    }

    private static async Task<bool> HandleStartMessage(BotUser botUser)
    {
        if (botUser.Message.Text != "/start")
        {
            return false;
        }

        var answer = BeautyHelper.MakeItStyled($"Приветствую!\n\nЯ - ваш персональный бот. Моя основная задача - упрощение " +
                         $"мониторинга MergeRequest-ов на GitLab.\n" +
                         $"\nВы можете проверять обновления своих проектов вручную при помощи клавиатурных кнопок " +
                         $"<{AnswerSender.KeyboardWordsDic[KeyboardWords.ShowAllMR]}> или {AnswerSender.KeyboardWordsDic[KeyboardWords.ShowMyMR]}>, " +
                         $"а также настроить периодические оповещения при помощи кнопок " +
                         $"<{AnswerSender.KeyboardWordsDic[KeyboardWords.CustomizeNotification]}> и <{AnswerSender.KeyboardWordsDic[KeyboardWords.SwitchNotification]}>.\n" +
                         $"\nВ случае возникновения неполадок в работе бота - обратитесь в службу поддержки.\n" +
                         $"\np.s. Если вы не администратор бота, то меню вам недоступно и пользоваться функциями бота вы не сможете.", UiTextStyle.Default);

        return await AnswerSender.SendMessage(botUser, answer);
    }

    private static Task<bool> HandleKeyboardWordMessage(BotUser botUser)
    {
        if (!AnswerSender.KeyboardWordsDic.ContainsValue(botUser.Message.Text))
        {
            return false;
        }

        try
        {
            switch (botUser.Message.Text)
            {
                case var text when text == AnswerSender.KeyboardWordsDic[KeyboardWords.ShowAllMR]:
                    {
                        var answer = BeautyHelper.MakeItStyled($"Все ваши активные Merge Request-ы\n", UiTextStyle.Header);

                        answer += BotFunctions.GetAllActiveMergeRequests().MakeMrResponseBeautier();

                        AnswerSender.SendMessage(botUser, answer);

                        break;
                    }
                case var text when text == AnswerSender.KeyboardWordsDic[KeyboardWords.ShowMyMR]:
                    {
                        var answer = BeautyHelper.MakeItStyled($"Все закрепленные за вами активные Merge Request-ы\n", UiTextStyle.Header);

                        answer += BotFunctions.GetAllAttachedToMeMergeRequests().MakeMrResponseBeautier();

                        AnswerSender.SendMessage(botUser, answer);

                        break;
                    }
                case var text when text == AnswerSender.KeyboardWordsDic[KeyboardWords.CustomizeNotification]:
                    {
                        var answer = BeautyHelper.MakeItStyled($"Для установки нового интервала оповещений пришлите мне сообщение формата:\n" +
                            $"H:3, где H - hours (часы), 3 - количество часов, или формата:\n" +
                            $"m:600, где m - minutes (минуты), 600 - количество минут.", UiTextStyle.Default);

                        AnswerSender.SendMessage(botUser, answer);

                        break;
                    }
                case var text when text == AnswerSender.KeyboardWordsDic[KeyboardWords.SwitchNotification]:
                    {
                        var answer = BotFunctions.SwitchNotifications(botUser) ?
                            BeautyHelper.MakeItStyled("Регулярные оповещения включены! ✅", UiTextStyle.Default) :
                            BeautyHelper.MakeItStyled("Регулярные оповещения отключены! 🔴", UiTextStyle.Default);

                        AnswerSender.SendMessage(botUser, answer);

                        break;
                    }
                case var text when text == AnswerSender.KeyboardWordsDic[KeyboardWords.AboutInfo]:
                    {
                        var answer = BeautyHelper.MakeItStyled($"Приветствую!\n\nЯ - ваш персональный бот. Моя основная задача - упрощение " +
                            $"мониторинга MergeRequest-ов на GitLab.\n" +
                            $"\nВы можете проверять обновления своих проектов вручную при помощи клавиатурных кнопок " +
                            $"<{AnswerSender.KeyboardWordsDic[KeyboardWords.ShowAllMR]}> или <{AnswerSender.KeyboardWordsDic[KeyboardWords.ShowMyMR]}>, " +
                            $"а также настроить периодические оповещения при помощи кнопок " +
                            $"<{AnswerSender.KeyboardWordsDic[KeyboardWords.CustomizeNotification]}> и <{AnswerSender.KeyboardWordsDic[KeyboardWords.SwitchNotification]}>.\n" +
                            $"По умолчанию оповещения отключены, а все настройки действуют лишь в рамках одной рабочей сессии.\n" +
                            $"\nВ случае возникновения неполадок в работе бота - обратитесь в службу поддержки.\n", UiTextStyle.Default);

                        AnswerSender.SendMessage(botUser, answer);

                        break;
                    }
            }
        }
        catch (BadApiResponseException ex)
        {
            BotLogger.LogException(ex, "", botUser);
        }
        catch (BadMarkdownSyntaxException ex)
        {
            BotLogger.LogException(ex, "Скорее всего исключение вызвано причудами кодировки телеграмма", botUser);
        }
        catch (Exception ex)
        {
            BotLogger.LogException(ex, "", botUser);
        }

        return true;
    }

    private static Task<bool> HandleAdminsAlertsCustomization(BotUser botUser)
    {
        if (_hourRegex.IsMatch(botUser.Message.Text) || _minuteRegex.IsMatch(botUser.Message.Text))
        {
            if (_hourRegex.IsMatch(botUser.Message.Text))
            {
                BotFunctions.CustomizeNotifications(botUser, int.Parse(Regex.Match(botUser.Message.Text, @"\d+").Value) * 60);
            }
            else
            {
                BotFunctions.CustomizeNotifications(botUser, int.Parse(Regex.Match(botUser.Message.Text, @"\d+").Value));
            }

            BotFunctions.SwitchNotifications(botUser);

            var answer = BeautyHelper.MakeItStyled($"Изменения успешно применены!\nТеперь вы будете получать информацию " +
                $"о новых MergeRequest-ах каждые {botUser.NotificationFrequencyMinutes} минут", UiTextStyle.Default);

            AnswerSender.SendMessage(botUser, answer);

            return true;
        }

        return false;
    }

    private static Task<bool> HandleAnyUnknownMessage(BotUser botUser)
    {
        AnswerSender.SendMessage(botUser, "Я на этом свете недавно и еще не знаю таких сложных команд 😢");

        return true;
    }

    private static BotUser CheckInUser(ITelegramBotClient telegramBotClient, Message message)
    {
        if (BotUsers.TryGetValue(message.Chat.Id, out var user))
        {
            return user;
        }

        var newUser = new BotUser(telegramBotClient, message);

        BotUsers.Add(message.Chat.Id, newUser);

        return newUser;
    }

    private static void ResetUserMessage(BotUser currentBotUser, Message message)
    {
        currentBotUser.Message = message;
    }
}