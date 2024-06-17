namespace Intensive_Bot.Entities;

public class BotEnvironment
{
    public string BotToken { get; set; }
    public string AdminUsername { get; set; }
    public string AclFilePath { get; set; }
    public string LogFilePath { get; set; }
    public string ExceptionLogFilePath { get; set; }
    public string GitLabAuthToken { get; set; }
}