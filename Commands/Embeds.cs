using Discord;
using Discord.Interactions;

namespace KrileDotNet.Commands;

public class Embeds : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("embed", "Create a custom embed in the current channel")]
    public async Task PromptForEmbedData()
    {
        try
        {
            await Context.Interaction.RespondWithModalAsync<EmbedModal>( "krile_embed_builder");
        }
        catch (Exception e)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Error, "Commands", e.Message));
            await RespondAsync("Unable to create modal for embed creation", ephemeral: true);
        }
    }

    [ModalInteraction("krile_embed_builder")]
    public async Task HandleEmbedCreation(EmbedModal modal)
    {
        var buttons = new ComponentBuilder();
        if (!string.IsNullOrWhiteSpace(modal.msg_link_one) && modal.msg_link_one.Contains('|'))
        {
            buttons = buttons.WithButton(modal.msg_link_one.Split('|')[0], style: ButtonStyle.Link, url: modal.msg_link_one.Substring(modal.msg_link_one.LastIndexOf('|') + 1));
        }
        if (!string.IsNullOrWhiteSpace(modal.msg_link_two) && modal.msg_link_two.Contains('|'))
        {
            buttons = buttons.WithButton(modal.msg_link_two.Split('|')[0], style: ButtonStyle.Link, url: modal.msg_link_two.Substring(modal.msg_link_two.LastIndexOf('|') + 1));
        }
        if (!string.IsNullOrWhiteSpace(modal.msg_link_three) && modal.msg_link_three.Contains('|'))
        {
            buttons = buttons.WithButton(modal.msg_link_three.Split('|')[0], style: ButtonStyle.Link, url: modal.msg_link_three.Substring(modal.msg_link_three.LastIndexOf('|') + 1));
        }

        var msg = new EmbedBuilder()
            .WithTitle(modal.msg_title)
            .WithDescription(modal.msg_content)
            .WithAuthor(Context.User);
        await ReplyAsync(embed: msg.Build(), components: buttons.Build());
        await RespondAsync("Your embed has been created", ephemeral: true);
    }

    public class EmbedModal : IModal
    {
        public string Title => "Custom Embed Creator";
        [InputLabel("Message Title")]
        [ModalTextInput("msg_title", TextInputStyle.Short, placeholder: "Hi there!")]
        public string msg_title { get; set; }
        [InputLabel("Message Content")]
        [ModalTextInput("msg_content", TextInputStyle.Paragraph, placeholder: "Hello world! Below is......")]
        public string msg_content { get; set; }
        [InputLabel("Link 1")]
        [RequiredInput(false)]
        [ModalTextInput("msg_link_one", TextInputStyle.Short, placeholder: "Google|https://google.com/")]
        public string msg_link_one { get; set; }
        [InputLabel("Link 2")]
        [RequiredInput(false)]
        [ModalTextInput("msg_link_two", TextInputStyle.Short, placeholder: "Google|https://google.com/")]
        public string msg_link_two { get; set; }
        [InputLabel("Link 3")]
        [RequiredInput(false)]
        [ModalTextInput("msg_link_three", TextInputStyle.Short, placeholder: "Google|https://google.com/")]
        public string msg_link_three { get; set; }
    }
}