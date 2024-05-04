﻿using Discord;
using MassTransit;
using YouAreShutUp.Contracts.GetPlayerGamingPreferences;
using YouAreShutUp.DiscordBot.Discord.Cache;

namespace YouAreShutUp.DiscordBot.Steam.Consumers;

public class GetPlayerGamingPreferencesConsumer : IConsumer<GetPlayerGamingPreferencesResponse>
{
    private readonly IDiscordMessageCache<IUserMessage, ulong> _messageCache;

    public GetPlayerGamingPreferencesConsumer(IDiscordMessageCache<IUserMessage, ulong> messageCache)
    {
        _messageCache = messageCache;
    }

    public async Task Consume(ConsumeContext<GetPlayerGamingPreferencesResponse> context)
    {
        var message = _messageCache.GetMessage(context.Message.ExternalMessageId);
        if (message is null)
            return;
        var stringMessage = MapToString(context.Message.GamingPreferences);
        await message.ReplyAsync(stringMessage);
    }

    private static string MapToString(IEnumerable<GetPlayerGamingPreferencesResponsePart> parts)
    {
        if (!parts.Any())
        {
            return
                "The player haven't played anything or it's not possible to fetch the data from Steam at the moment.\nTry again later.";
        }

        parts = parts.OrderByDescending(x => x.TotalHours)
            .ThenByDescending(x => x.Genre);
        return string.Join(Environment.NewLine, parts.Select(part => $"Genre: {part.Genre}, Hours: {part.TotalHours}"));
    }
}