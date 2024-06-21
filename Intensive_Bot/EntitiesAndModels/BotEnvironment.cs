namespace Intensive_Bot.EntitiesAndModels;

public class BotEnvironment
{
    public string BotToken { get; set; }
    public string AdminUsername { get; set; }
    public string LogFilePath { get; set; }
    public string ExceptionLogFilePath { get; set; }
    public string GitLabAuthToken { get; set; }
    public string GitLabUrl { get; set; }
}