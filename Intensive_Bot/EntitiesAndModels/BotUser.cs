using Telegram.Bot.Types;
using Telegram.Bot;

namespace Intensive_Bot.Entities;

public class BotUser
{
    public long ChatId { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }

    public ITelegramBotClient BotClient { get; set; }
    public Message Message { get; set; }

    public int AlertFrequencyMinutes { get; set; } = 0;
    public bool AlertsOn { get; set; } = false;
    
    public BotUser(ITelegramBotClient botClient, Message message)
    {
        ChatId = message.Chat.Id;
        Username = message.Chat.Username;
        FirstName = message.Chat.FirstName;
        BotClient = botClient;
        Message = message;
    }
}