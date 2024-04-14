namespace YouAreShutUpDiscordBot.Discord.Configuration.Options;

public class DiscordFriendsSettings
{
    public const string Key = nameof(DiscordFriendsSettings);
    public ulong DenisId { get; init; }
    public ulong FirsId { get; init; }
    public ulong JonseeId { get; init; }
    public ulong MainChannelId { get; init; }
    public ulong AdditionalChannelId { get; init; }
}