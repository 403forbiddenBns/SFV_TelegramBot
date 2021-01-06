using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;


namespace TelegramBot.Resources
{
	//todo: other path to log
	//todo: json log?


	public static class BotMethods
	{
		private static List<LogItem> messagesToLog = new List<LogItem>();

		/// <summary>
		/// Async bot answer method.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static async void Bot_OnMessage(object sender, MessageEventArgs e)
		{
			//Exit if recieved nothing.
			if (e.Message == null)
				return;

			Console.WriteLine($"Received {e.Message.Type} message from {e.Message.Chat.Username}.");
			if (e.Message.Type == MessageType.Text)
			{
				Console.Write($"\t msgText: {e.Message.Text} \n\r");
			}


			//Log message data.
			messagesToLog.Add(new LogItem(e));

			string respond = JsonParseFrameData.BotAnswer(e);

			await Execute.tbot.SendTextMessageAsync(
				chatId: e.Message.Chat,
				text: respond
			);

			//Log message to Log.json
			messagesToLog.Add(new LogItem(respond));

			LogWriter logWriter = new LogWriter("Log.txt", messagesToLog);

			logWriter.Process(); //Write log data to file.
		}

		/// <summary>
		/// Async file download.
		/// </summary>
		/// <param name="fieldID">Field ID.</param>
		/// <param name="path">Path to new file.</param>
		private static async void FileDownAsyncCustom(string fieldID, string path)
		{
			var file = await Execute.tbot.GetFileAsync(fieldID);
			FileStream fs = new FileStream(path, FileMode.Create);
			await Execute.tbot.DownloadFileAsync(file.FilePath, fs);
			fs.Close();

			fs.Dispose();
		}
	}
}
