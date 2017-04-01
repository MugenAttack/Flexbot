using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using IniParser;
using IniParser.Model;

namespace FlexBot
{
	class Program
	{
		static void Main(string[] args) => new Program().Start();

		private DiscordClient _client;

		public void Start()
		{
			_client = new DiscordClient();
			var parser = new FileIniDataParser();
			IniData Settings = parser.ReadFile("Config.ini");

			_client.UsingCommands(x =>
			{
				x.PrefixChar = '!';
				x.HelpMode = HelpMode.Public;
			});

			ServerManagement SM = new ServerManagement(ref Settings);
			SM.BasicCommands(ref _client);


			CustomCommands CC = new CustomCommands();
			if (Settings["Bot"]["useCustomCommands"].Trim() == "true")
				CC.Load(ref _client, Settings["Folders"]["CustomCommands"] + "/");

			FactCommands FC = new FactCommands();
			if (Settings["Bot"]["useFactCommands"].Trim() == "true")
				FC.Load(ref _client, Settings["Folders"]["FactCommands"] + "/");

			_client.ExecuteAndWait(async () =>
			{
				await _client.Connect(Settings["Bot"]["DiscordBotToken"], TokenType.Bot);
			});
		}
	}
}
