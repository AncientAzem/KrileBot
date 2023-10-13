using System.Diagnostics.CodeAnalysis;
using Discord;
using Discord.Interactions;

namespace KrileDotNet.Commands;

public class CustomEmbedBuilder : InteractionModuleBase<SocketInteractionContext>
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
            await Helpers.LogMessage(LogSeverity.Error, "Commands", e.Message);
            await RespondAsync("Unable to create modal for embed creation", ephemeral: true);
        }
    }

    [ModalInteraction("krile_embed_builder")]
    public async Task HandleEmbedCreation(EmbedModal modal)
    {
        var buttons = new ComponentBuilder();
        if (!string.IsNullOrWhiteSpace(modal.MsgLinkOne) && modal.MsgLinkOne.Contains('|'))
        {
            buttons = buttons.WithButton(modal.MsgLinkOne.Split('|')[0], style: ButtonStyle.Link, url: modal.MsgLinkOne.Substring(modal.MsgLinkOne.LastIndexOf('|') + 1));
        }
        if (!string.IsNullOrWhiteSpace(modal.MsgLinkTwo) && modal.MsgLinkTwo.Contains('|'))
        {
            buttons = buttons.WithButton(modal.MsgLinkTwo.Split('|')[0], style: ButtonStyle.Link, url: modal.MsgLinkTwo.Substring(modal.MsgLinkTwo.LastIndexOf('|') + 1));
        }
        if (!string.IsNullOrWhiteSpace(modal.MsgLinkThree) && modal.MsgLinkThree.Contains('|'))
        {
            buttons = buttons.WithButton(modal.MsgLinkThree.Split('|')[0], style: ButtonStyle.Link, url: modal.MsgLinkThree.Substring(modal.MsgLinkThree.LastIndexOf('|') + 1));
        }

        var msg = new EmbedBuilder()
            .WithTitle(modal.MsgTitle)
            .WithDescription(modal.MsgContent)
            .WithAuthor(Context.User);
        await ReplyAsync(embed: msg.Build(), components: buttons.Build());
        await RespondAsync("Your embed has been created", ephemeral: true);
    }

    public class EmbedModal : IModal
    {
        public string Title => "Custom Embed Template";
        
        [InputLabel("Message Title")]
        [ModalTextInput("msg_title", placeholder: "Hi there!")]
        public required string MsgTitle { get; init; }
        
        [InputLabel("Message Content")]
        [ModalTextInput("msg_content", TextInputStyle.Paragraph, placeholder: "Hello world! Below is......")]
        public required string MsgContent { get; init; }
        
        [InputLabel("Link 1")]
        [RequiredInput(false)]
        [ModalTextInput("msg_link_one", placeholder: "Google|https://google.com/")]
        public string? MsgLinkOne { get; }
        
        [InputLabel("Link 2")]
        [RequiredInput(false)]
        [ModalTextInput("msg_link_two", placeholder: "Google|https://google.com/")]
        public string? MsgLinkTwo { get; }
        
        [InputLabel("Link 3")]
        [RequiredInput(false)]
        [ModalTextInput("msg_link_three", placeholder: "Google|https://google.com/")]
        public string? MsgLinkThree { get; }
    }
}