using Discord;

namespace YouAreShutUp.DiscordBot.Discord.Cache;

public interface IDiscordMessageCache<TMessage, TKey> where TMessage : IMessage
{
    TMessage? GetMessage(TKey key);
    bool SetMessage(TMessage message);
}