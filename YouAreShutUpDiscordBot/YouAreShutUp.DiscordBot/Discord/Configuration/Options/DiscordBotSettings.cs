namespace YouAreShutUp.DiscordBot.Discord.Configuration.Options;

public class DiscordBotSettings
{
    public const string Key = nameof(DiscordBotSettings);
    public required string BotToken { get; init; }
}