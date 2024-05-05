using System.Collections.Concurrent;
using Discord;

namespace YouAreShutUp.DiscordBot.Discord.Cache;

public class DiscordUserMessageCache : IDiscordMessageCache<IUserMessage, ulong>
{
    private readonly ConcurrentDictionary<ulong, IUserMessage> _cache = new();

    public IUserMessage? GetMessage(ulong key)
    {
        _cache.TryRemove(key, out var result);
        return result;
    }

    public bool SetMessage(IUserMessage message)
    {
        _cache[message.Id] = message;
        return true;
    }
}