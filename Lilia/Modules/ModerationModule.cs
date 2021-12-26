﻿using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Lilia.Database;
using Lilia.Services;

namespace Lilia.Modules;

public class ModerationModule : BaseCommandModule
{
    private LiliaClient _client;
    private LiliaDbContext _dbCtx;

    public ModerationModule(LiliaClient client)
    {
        this._client = client;
        this._dbCtx = client.Database.GetContext();
    }

    [Command("ban")]
    [RequirePermissions(Permissions.BanMembers)]
    public async Task BanMembersCommand(CommandContext ctx,
        [Description("List of members to ban.")]
        params DiscordMember[] members)
    {
        await ctx.RespondAsync("Banning mischievous people...");

        foreach (DiscordMember member in members)
        {
            if (member == ctx.Message.Author)
            {
                await ctx.RespondAsync(
                    $"Beaned {Formatter.Bold($"{ctx.Client.CurrentUser.Username}#{ctx.Client.CurrentUser.Discriminator}")} from this server.");
            }
            else
            {
                await ctx.Guild.BanMemberAsync(member, 0, "Batch banning.");
                await ctx.RespondAsync($"Banned {member.DisplayName}#{member.Discriminator} from this server.");
            }

            await Task.Delay(1000);
        }
    }

    [Command("kick")]
    [RequirePermissions(Permissions.KickMembers)]
    public async Task KickMembersCommand(CommandContext ctx,
        [Description("List of members to kick.")]
        params DiscordMember[] members)
    {
        await ctx.RespondAsync("Kicking mischievous people...");

        foreach (DiscordMember member in members)
        {
            if (member == ctx.Message.Author)
            {
                await ctx.RespondAsync("Imagine kicking yourself, smh.");
            }
            else
            {
                await member.RemoveAsync("Batch kicking");
                await ctx.RespondAsync($"Kicked {member.DisplayName}#{member.Discriminator} from this server.");
            }

            await Task.Delay(1000);
        }
    }
}