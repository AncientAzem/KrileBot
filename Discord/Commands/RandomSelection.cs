using System.Net;
using Discord;
using Discord.Interactions;

namespace KrileDotNet.Commands;

public class RandomSelection : InteractionModuleBase<SocketInteractionContext>
{
    private static readonly Random Randomizer = new Random();
    
    [EnabledInDmAttribute(true)]
    [MessageCommand("Random Selection")]
    public async Task StartTask(IMessage message)
    {
        try
        {
            var items = message.Content.ToLower().StartsWith("http")
                ? await GetFromUrl(message.Content)
                : await GetFromMessage(message.Content);
            await RespondAsync($"Item Selected: `{items[Randomizer.Next((items.Count))]}`", ephemeral: true);
        }
        catch (Exception e)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Error, "Commands", e.Message));
            await RespondAsync("Unable to select a random item. Please you used this command on a message containing only a URL or a message with a comma delineated list.", ephemeral: true);
        }
    }
    
    private async Task<List<string>> GetFromUrl(string url)
    {
        try
        {
            var client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(url);
            using HttpContent content = response.Content;
            var raw = await content.ReadAsStringAsync();
            return raw.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
        }
        catch (Exception e)
        {
            throw new Exception("Unable to obtain or parse list from URL");
        }
    }

    private async Task<List<string>> GetFromMessage(string msg)
    {
        return null;
    }
}