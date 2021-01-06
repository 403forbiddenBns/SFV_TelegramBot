using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace TelegramBot.Resources
{
	public class LogItem
	{ 
		private string message;
		private string name;
		private DateTime date;

		public string Message { get => message; set => message = value; }
		public string Name { get => name;  set => name = value; }
		public DateTime Date { get => date; set => date = value; }


		#region ctor

		public LogItem(string message)
		{
			Message = message;
			Name = BotConfiguration.GetName();
			date = DateTime.Now;
		}

		public LogItem(MessageEventArgs e)
		{
			Message = e.Message.Text;
			Name = e.Message.Chat.Username;
			date = DateTime.Now;
		}

		#endregion
	}
}
