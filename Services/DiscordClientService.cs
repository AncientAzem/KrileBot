using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace KrileDotNet.Services;

public class DiscordClientService : IHostedService
{
    private static DiscordSocketClient? _client;
    private static InteractionService? _interactionService;
    private static CommandService? _commandService;
    private static IServiceProvider? _serviceProvider;
    private static HuntRelayService _huntRelayService;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_client is null)
        {
            var config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All
            };
            
            _client = new DiscordSocketClient(config);
            _client.Log += LogDiscordMessage;
            _client.Ready += ClientReady;
        }

        if (_interactionService is null)
        {
            _interactionService = new InteractionService(_client.Rest);
        }

        if (_commandService is null)
        {
            _commandService = new CommandService();
        }
        
        _serviceProvider = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commandService)
            .AddSingleton(_interactionService)
            .BuildServiceProvider();
        
        // Hunt Provider
        // HuntRelayFormatter.Setup(827202823774666792, 1071183795585290290, 1016065075875938334); // Actual Server
        

        var botToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
        await _client.LoginAsync(TokenType.Bot, botToken);
        await _client.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client!.StopAsync();
        await _client.LogoutAsync();
    }

    private static async Task ClientReady()
    {
        try
        {
            await _client!.SetActivityAsync(new Game("Ejika's theories", ActivityType.Listening));
            await _interactionService!.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
            var guildId = Environment.GetEnvironmentVariable("GUILD_ID");
            if (guildId is not null)
            {
                await LogDiscordMessage(new LogMessage(LogSeverity.Info, "Startup", "Initializing Guild Commands"));
                await _interactionService.RegisterCommandsToGuildAsync(Convert.ToUInt64(guildId));
            }
            else
            {
                await LogDiscordMessage(new LogMessage(LogSeverity.Info, "Startup", "Initializing Global Commands"));
                await _interactionService.RegisterCommandsGloballyAsync();
            }
            
            // Setup Hunts
            _huntRelayService = new HuntRelayService(ref _client,
                ulong.Parse(Environment.GetEnvironmentVariable("RELAY_SERVER_ID")!), 
                ulong.Parse(Environment.GetEnvironmentVariable("RELAY_CHANNEL_ID")!), 
                ulong.Parse(Environment.GetEnvironmentVariable("SONAR_CHANNEL_ID")!));
        }
        catch (HttpException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }

        _client!.SlashCommandExecuted += async interaction =>
        {
            var scope = _serviceProvider!.CreateScope();
            var ctx = new SocketInteractionContext(_client, interaction);
            await _interactionService!.ExecuteCommandAsync(ctx, scope.ServiceProvider);
        };

        _client!.MessageCommandExecuted += async interaction =>
        {
            var temp = await _client.Rest.GetGlobalApplicationCommands();
            var scope = _serviceProvider!.CreateScope();
            var ctx = new SocketInteractionContext(_client, interaction);
            await _interactionService!.ExecuteCommandAsync(ctx, scope.ServiceProvider);
        };

        _client!.MessageReceived += async interaction =>
        {
            if (interaction.Channel.Id == ulong.Parse(Environment.GetEnvironmentVariable("SONAR_CHANNEL_ID")!))
            {
                await _huntRelayService.ProcessSonarReport(interaction.Content);
            }
        };
    }

    private static Task LogDiscordMessage(LogMessage message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}