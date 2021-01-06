using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Resources
{
	public static class BotConfiguration
	{
		private static string BotName { get; set; } = "SFVFramedataBot";
		private static string Token { get; set; } = File.ReadAllText(@"C:\Users\me\Desktop\gihub\token.txt");


		#region Methods

		/// <summary>
		/// Returns bot name.
		/// </summary>
		/// <returns>BotName</returns>
		public static string GetName()
		{
			return BotName;
		}

		/// <summary>
		/// Returns bot token.
		/// </summary>
		/// <returns>Token.</returns>
		public static string GetToken()
		{
			return Token;
		}

		#endregion
	}
}
