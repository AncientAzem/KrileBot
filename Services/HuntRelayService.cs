using Discord;
using Discord.WebSocket;

namespace KrileDotNet.Services;
/// <summary>
/// NOTE: This is hardcoded to only work in my personal server.
/// </summary>
public class HuntRelayService
{
     private DiscordSocketClient _client;
     private IMessageChannel? _relayChannel;
     private IMessageChannel? _sonarChannel;
     
     private ulong _relayServerId;

     public HuntRelayService(ref DiscordSocketClient client, ulong relayServer, ulong relayChannel, ulong sonarChannel)
     {
          _client = client;
          _relayServerId = relayServer;
          _relayChannel = _client.GetChannel(relayChannel) as IMessageChannel;
          _sonarChannel = _client.GetChannel(sonarChannel) as IMessageChannel;
     }
     
     public async Task ProcessSonarReport(string msg)
     {
          if (!msg.Contains("was just killed"))
          {
               string server = msg.Substring(msg.IndexOf('<') + 1, msg.IndexOf('>') - msg.IndexOf('<') - 1);
               string rank = msg.Substring(17, 1);
               string mobName = msg.Substring(20, msg.IndexOf('➲') - 21);
               string location = msg.Substring(msg.IndexOf('➲') + 1, msg.IndexOf(')') - msg.IndexOf('➲') + 1);
               string zone = location.Substring(0, location.IndexOf('(') -1);
               string? expansion = Helpers.FFXIV_Zones
                    .FirstOrDefault(x => x.Value.Contains(zone)).Key;
          
               this._relayChannel?.SendMessageAsync($"Expansion: {expansion ?? "Unknown"}\nServer: {server}\nMob Name: {mobName}\nLocation: {location}");
          }
     }
}