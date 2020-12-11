using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TelegramBot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = Telegram.Bot.Types.File;


namespace TelegramBot.Resources
{
	public static class BotMethods
	{
		private static string TextToSend { get; set; }  = "No data.";

		//List of all character.
		private static string[] characterList { get; } = new[]
		{
			"ryu",
			"akuma",
			"abigail",
			"alex",
			"kage",
			"ken",
			"karin",
			"kolin",
			"balrog",
			"birdie",
			"blanka",
			"laura",
			"lucia",
			"bison",
			"menat",
			"cammy",
			"nash",
			"chun-li",
			"cody",
			"necalli",
			"dhalsim",
			"poison",
			"mika",
			"rashid",
			"ed",
			"honda",
			"fang",
			"falke",
			"sagat",
			"sakura",
			"seth",
			"g",
			"gill",
			"guile",
			"urien",
			"vega",
			"ibuki",
			"juri",
			"zangief",
			"zekuyoung",
			"zekuold",
		};

		//private static Task<File> FileTask { get; set; }
		private static string DownloadUrl { get; set; } = $@"https://api.telegram.org/file/bot{BotConfiguration.GetToken()}/";

		/// <summary>
		/// Bot message answer delegate.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static async void Bot_OnMessage(object sender, MessageEventArgs e)
		{
			//todo: exception processing

			var recivedMsg = e.Message;

			#region Notification

			switch (recivedMsg.Type)
			{
				case MessageType.Photo:
					Console.WriteLine($"Received a photo from @{e.Message.From.Username}.");
					break;
				case MessageType.Text:
					Console.WriteLine($"Received a message from @{e.Message.From.Username}: {recivedMsg.Text}.");
					break;
				case MessageType.Audio:
					Console.WriteLine($"Received an audio message from @{e.Message.From.Username}.");
					break;
				case MessageType.Document:
					Console.WriteLine($"Received a document from @{e.Message.From.Username}.");
					break;
				case MessageType.Sticker:
					Console.WriteLine($"Received a sticker from @{e.Message.From.Username}.");
					break;
			}

			#endregion

			//Exit if recieved nothing.
			if (recivedMsg == null)
				return;

			//Main logic.
			switch (recivedMsg.Type)
			{
				case MessageType.Text:
				{
					string respond = TextResponse(recivedMsg.Text);
					if (respond == String.Empty)
					{
						respond = "No data.";
					}
					//string respond = "Try again";

					try
					{
						await Execute.tbot.SendTextMessageAsync(
							chatId: recivedMsg.Chat,
							text: respond
						);
					}
					catch (Exception exception)
					{
						Console.WriteLine("Error:" + exception.Message);
					}
				}
					break;
				case MessageType.Document:
				{
					await Execute.tbot.SendDocumentAsync(
						chatId: recivedMsg.Chat,
						caption: "I don't know what to do with documents, so i just bring it back to you.",
						document: recivedMsg.Document.FileId
					);
					return;
				}
					break;
				case MessageType.Photo:
				{
					string photoName = PhotoName(); //Generates original photo name.
					string path = @"Photos\";

					//Download photo.
					FileDownAsyncCustom(recivedMsg.Photo[recivedMsg.Photo.Length - 1].FileId, path + photoName);

					try
					{
						await Execute.tbot.SendPhotoAsync(
							chatId: recivedMsg.Chat,
							photo: recivedMsg.Photo[recivedMsg.Photo.Length - 1].FileId,
							"Im don't know what to do with images, so i just bring it back to you."
						);
					}
					catch (Exception exception)
					{
						Console.WriteLine($"Error: {exception.GetType()}");
					}

				}
					break;
				case MessageType.Audio:
				{
					await Execute.tbot.SendTextMessageAsync(
						chatId: recivedMsg.Chat,
						text: "I don't work with audio, sorry."
					);
				}
					break;
				case MessageType.Sticker:
					//
					break;
				case MessageType.Voice:
				{
					await Execute.tbot.SendTextMessageAsync(
						chatId: recivedMsg.Chat,
						text: "I can not recognize voice, sorry."
					);
				}
					break;
			}
		}


		/// <summary>
		/// Returns original name for photo.
		/// </summary>
		/// <returns>Photo name.</returns>
		private static string PhotoName()
		{
			if (!Directory.Exists("Photos"))
			{
				Directory.CreateDirectory("Photos");
			}
			string photoName = "photo_1.jpg";
			while (System.IO.File.Exists($@"Photos\{photoName}"))
			{
				string tempName = photoName;
				int number = 0;
				Int32.TryParse(tempName.Substring(6).Replace(".jpg", ""), out number);
				photoName = $"photo_{++number}.jpg";
			}

			return photoName;
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
		
		/// <summary>
		/// Returns full response depends on what user asks.
		/// </summary>
		/// <param name="recivedData">Recieved message.</param>
		/// <returns></returns>
		private static string TextResponse(string recivedData)
		{
			// /start
			// /commands
			// /vt1
			// /vt2
			// /movelist
			// /stats
			// /vt1movelist
			// /vt2movelist
			// /characterlist
			// /move

			if (recivedData.Length == 1)
			{
				return "Введите корректную команду.";
			}

			string result = "No data.";

			//Turns character name to lower case for better UX
			string lowerCaseName = recivedData.Substring(1);
			lowerCaseName = char.ToLower(lowerCaseName[1]).ToString() + lowerCaseName.Substring(2);

			//base commands.
			switch (recivedData)
			{
				case "/move":
					return "Введите данные в формате /move [character name] [move name]";
				case "/start":
					return "Введите /commands для отображения всех команд.";
					break;
				case "/commands":
					return "/move [character name] [move name] \n" +
							"/vt1 [character name] [move name] \n" +
					       "/vt2 [character name] [move name] \n" +
					       "/movelist [character name] \n" +
					       "/stats [character name] \n" +
					       "/vt1movelist [character name] \n" +
					       "/vt2movelist [character name] \n" +
					       "/characterlist";
				case "/vt1":
					return "Введите данные в формате /vt1 [character name] [move name]";
				case "/vt2":
					return "Введите данные в формате /vt2 [character name] [move name]";
				case "/movelist":
					return "Введите данные в формате /movelist [character name]";
				case "/stats":
					return "Введите данные в формате /stats [character name]";
				case "/characterlist":
					return JsonParseFrameData.GetCharacterList();
			}

			string[] tempData;

			#region Proper divide

			if (recivedData.Contains("/vt1"))
			{
				char[] separator = new[] { ' ' };
				tempData = recivedData.Split(separator, 3);
			}
			else if (recivedData.Contains("/vt2"))
			{
				char[] separator = new[] { ' ' };
				tempData = recivedData.Split(separator, 3);
			}
			else if (recivedData.Contains("/movelist"))
			{
				char[] separator = new[] { ' ' };
				tempData = recivedData.Split(separator, 2);
			}
			else if (recivedData.Contains("/stats"))
			{
				char[] separator = new[] { ' ' };
				tempData = recivedData.Split(separator, 2);
			}
			else if (recivedData.Contains("/vt1movelist"))
			{
				char[] separator = new[] { ' ' };
				tempData = recivedData.Split(separator, 2);
			}
			else if (recivedData.Contains("/vt2movelist"))
			{
				char[] separator = new[] { ' ' };
				tempData = recivedData.Split(separator, 2);
			}
			else if (recivedData.Contains("/move"))
			{
				char[] separator = new[] { ' ' };
				tempData = recivedData.Split(separator, 3);
			}
			else
			{
				return result;
			}

			#endregion

			string tempName = char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1);

			switch (tempData.Length)
			{
				case 2:
					if (tempData[0] == "/movelist")
					{
						return result = characterList.Contains(tempName.ToLower()) ? JsonParseFrameData.GetStats(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1)) : "No data.";
					}
					else if (tempData[0] == "/stats")
					{
						return result = characterList.Contains(tempName.ToLower()) ? JsonParseFrameData.GetStats(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1)) : "No data.";
					}
					else if(tempData[0] == "/vt1movelist")
					{
						return result = characterList.Contains(tempName.ToLower()) ? JsonParseFrameData.GetTriggerMoveList(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1), 1) : "No data.";
						
					}
					else if (tempData[0] == "/vt2movelist")
					{
						return result = characterList.Contains(tempName.ToLower()) ? JsonParseFrameData.GetTriggerMoveList(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1), 2) : "No data.";
					}
					break;
				case 3:
					if (tempData[0] == "/vt1")
					{
						return result = characterList.Contains(tempName.ToLower()) ? JsonParseFrameData.GetTriggerMoveInfo(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1), 1, tempData[2]) : "No data.";
					}
					else if (tempData[0] == "/vt2")
					{
						return result = characterList.Contains(tempName.ToLower()) ? JsonParseFrameData.GetTriggerMoveInfo(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1), 2, tempData[2]) : "No data.";
					}
					else if (tempData[0] == "/move")
					{
						return result = characterList.Contains(tempName.ToLower()) ? JsonParseFrameData.GetMoveInfo(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1), tempData[2]) : "No data.";
					}
					break;
			}

			return result;
		}
	}
}
