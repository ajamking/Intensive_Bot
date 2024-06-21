using Telegram.Bot.Types;
using Telegram.Bot;

namespace Intensive_Bot.EntitiesAndModels;

public class BotUser
{
    public long ChatId { get; set; }
    public string? FirstName { get; set; }
    public string? Username { get; set; }
    public bool IsAdmin { get; set; }

    public ITelegramBotClient BotClient { get; set; }
    public Message Message { get; set; }

    public int NotificationFrequencyMinutes { get; set; } = 0;
    public bool NotificationEnabled { get; set; } = false;
    
    public BotUser(ITelegramBotClient botClient, Message message)
    {
        ChatId = message.Chat.Id;
        FirstName = message.Chat.FirstName;
        Username = message.Chat.Username;
        IsAdmin = Username == Program.BotEnvironment.AdminUsername;
        
        BotClient = botClient;
        Message = message;
    }
}