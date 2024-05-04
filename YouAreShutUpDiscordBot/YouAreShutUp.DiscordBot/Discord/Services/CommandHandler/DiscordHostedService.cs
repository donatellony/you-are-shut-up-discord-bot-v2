using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using YouAreShutUp.DiscordBot.Discord.Configuration.Options;

namespace YouAreShutUp.DiscordBot.Discord.Services.CommandHandler;

// Class is a little bit overloaded, but it's ok for the tests.
internal class DiscordHostedService : IHostedService, ICommandHandler
{
    private readonly DiscordShardedClient _client;
    private readonly CommandService _commandService;
    private readonly IOptions<DiscordBotSettings> _settings;
    private readonly ILogger<DiscordHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DiscordHostedService(
        DiscordShardedClient client,
        IOptions<DiscordBotSettings> settings,
        ILogger<DiscordHostedService> logger,
        CommandService commandService,
        IServiceProvider serviceProvider
    )
    {
        _client = client;
        _settings = settings;
        _logger = logger;
        _commandService = commandService;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await InitializeAsync();

        CreateHandlers();
        
        await _client.LoginAsync(TokenType.Bot, _settings.Value.BotToken);
        await _client.StartAsync();
        await _client.SetGameAsync("Firs Simpit! V3");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.LogoutAsync();
        await _client.StopAsync();
    }

    public async Task InitializeAsync()
    {
        await _commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
        _commandService.CommandExecuted += async (optional, context, result) =>
        {
            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
            {
                // the command failed, let's notify the user that something happened.
                await context.Channel.SendMessageAsync($"Error: {result}");
            }
        };

        foreach (var module in _commandService.Modules)
        {
            _logger.LogInformation("Module '{ModuleName}' initialized.", module.Name);
        }
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
        // Bail out if it's a System Message.
        if (arg is not SocketUserMessage msg)
            return;
        
        // We don't want the bot to respond to itself or other bots.
        if (msg.Author.IsBot || msg.Author.Id == _client.CurrentUser.Id)
            return;

        // Create a Command Context.
        var context = new ShardedCommandContext(_client, msg);
        
        var markPos = 0;
        if (msg.HasCharPrefix('!', ref markPos) || msg.HasCharPrefix('?', ref markPos))
        {
            await _commandService.ExecuteAsync(context, markPos, _serviceProvider);
        }
    }

    private void CreateHandlers()
    {
        _client.Log += async log =>
        {
            _logger.LogInformation("Discord log message : {Message}", log.Message);
            await Task.CompletedTask;
        };
        _client.ShardConnected += async shard =>
        {
            _logger.LogInformation("Shard Number {ShardId} is connected!", shard.ShardId);
            await Task.CompletedTask;
        };
        _client.ShardDisconnected += async (ex, shard) =>
        {
            _logger.LogError("Shard Number {ShardId} is disconnected!", shard.ShardId);
            await Task.CompletedTask;
        };
        _client.ShardReady += async shard =>
        {
            _logger.LogInformation("Shard Number {ShardId} is connected and ready!", shard.ShardId);
            await Task.CompletedTask;
        };
        _client.MessageReceived += HandleCommandAsync;
    }
}