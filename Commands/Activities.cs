using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace KrileDotNet.Commands;

public class Activities : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("activities", "[DEPRECATED] Starts an activity in a voice channel")]
    public async Task StartActivity()
    {
        await RespondAsync("This command is not longer usable. Please support the official release and rollout of activities by discord.", ephemeral: true);
    }

    public enum ActivityList : ulong
    {
        [ChoiceDisplay("Watch Together (YouTube)")]
        WatchTogether = DefaultApplications.Youtube,
        [ChoiceDisplay("Letter League (Scrabble)")]
        LetterLeague = DefaultApplications.LetterTile,
        [ChoiceDisplay("Sketch Heads (formally Doodle Crew)")]
        DoodleCrew = DefaultApplications.DoodleCrew,
        [ChoiceDisplay("Poker Night")]
        PokerNight = DefaultApplications.Poker,
        [ChoiceDisplay("Chess in the Park")]
        ChessInThePark = DefaultApplications.Chess,
        [ChoiceDisplay("Checkers in the Park")]
        CheckersInThePark = DefaultApplications.Checkers,
        [ChoiceDisplay("Blaze 8s (formally Ocho)")]
        Blaze8 = 832025144389533716,
    }
}