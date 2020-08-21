using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Vensha.Handlers;

namespace Vensha.Modules.Utility
{
    [Remarks("Utility")]
    [Summary("Get info on my commands!")]
    public class Help : ModuleBase<CommandContext>
    {
        private CommandService _service;
        public Help(CommandService service) => _service = service;

        [Command("help")]
        [Alias("h")]
        [Remarks("[CommandName]")]
        public Task Callback(string commandName)
        {
            commandName = commandName.ToLower();
            var command = _service.Commands.FirstOrDefault(x => x.Name.ToLower() == commandName || x.Aliases.Contains(commandName));

            if (command == null || command.Module.Remarks == "Development") return ReplyAsync($"`{commandName}` is not a valid command.");

            var embed = new Discord.EmbedBuilder()
                .WithTitle($"{Constants.Config.Prefix}{command.Name}")
                .WithDescription(command.Module.Summary ?? "No description provided.")
                .AddField("Aliases", command.Aliases.Count > 0 ? string.Join(", ", command.Aliases) : $"{command.Name} has no aliases.")
                .AddField("Usage", $"{Constants.Config.Prefix}{command.Name} {command.Remarks}");

            return ReplyAsync(null, false, embed.Build());
        }

        [Command("help")]
        [Alias("h")]
        [Remarks("[CommandName]")]
        public Task Callback()
        {
            var embed = new Discord.EmbedBuilder()
                .WithTitle("Help menu")
                .WithFooter($"For info on a specific command use {Constants.Config.Prefix}help [CommandName]");

            foreach (string category in (from mod in _service.Modules
                                         where mod.Remarks != "Development"
                                         select mod.Remarks)
                                         .Distinct())
            {
                var commands = from cmd in _service.Commands
                               where cmd.Module.Remarks == category
                               select $"`{Constants.Config.Prefix}{cmd.Name}` - {cmd.Module.Summary ?? "No description provided."}";

                embed.AddField(category, string.Join('\n', commands.Distinct()));
            }

            return ReplyAsync(null, false, embed.Build());
        }
    }
}