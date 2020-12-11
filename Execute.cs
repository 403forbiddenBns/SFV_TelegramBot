using System;
using Newtonsoft.Json;
using TelegramBot.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot
{

	class Execute
	{
		public static TelegramBotClient tbot;
		static void Main(string[] args)
		{
			tbot = new TelegramBotClient(BotConfiguration.GetToken());

			tbot.OnMessage += BotMethods.Bot_OnMessage;
			tbot.StartReceiving();

			Console.WriteLine("Press any key to stop..");
			Console.ReadKey();

			tbot.StopReceiving();

			Console.ReadKey();
		}
	}
}
