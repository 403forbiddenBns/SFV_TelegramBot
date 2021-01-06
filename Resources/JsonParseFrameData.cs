using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Resources
{
	public static class JsonParseFrameData
	{
		private static string path { get; set; } = "sfv.json";

		private static string json { get; set; } = File.ReadAllText(path);

		#region Methods

		/// <summary>
		/// Returns move info.
		/// </summary>
		/// <param name="character">Character name.</param>
		/// <param name="moveName">Move name.</param>
		/// <returns></returns>
		public static string GetMoveInfo(string character, string moveName)
		{
			StringBuilder moveInfo = new StringBuilder();

			if (JObject.Parse(json).ContainsKey(character) && JObject.Parse(json)[character]["moves"]["normal"]
				.ToObject<JObject>().ContainsKey(moveName))
			{
				var data = JObject.Parse(json)[character]["moves"]["normal"][moveName].ToObject<JObject>();
				foreach (var attribute in data)
				{
					if (attribute.Key == "cancelsTo")
					{
						moveInfo.AppendLine($"{attribute.Key} :");
						for (int i = 0; i < attribute.Value.Count(); i++)
						{
							moveInfo.AppendLine($"\t{attribute.Value[i]}");
						}
					}
					else if (attribute.Key == "extraInfo")
					{
						moveInfo.AppendLine($"{attribute.Key} :");
						for (int i = 0; i < attribute.Value.Count(); i++)
						{
							moveInfo.AppendLine($"\t{attribute.Value[i]}");
						}
					}
					else
					{
						moveInfo.AppendLine($"{attribute.Key} - {attribute.Value}");
					}
				}
			}
			else
			{
				return "No data.";
			}

			return moveInfo.ToString();
		}

		/// <summary>
		/// Returns move info under influence of specified V-Trigger.
		/// </summary>
		/// <param name="character">Character name.</param>
		/// <param name="triggerNumber">Number of V-Trigger.</param>
		/// <param name="moveName">Move name.</param>
		/// <returns></returns>
		public static string GetTriggerMoveInfo(string character, int triggerNumber, string moveName)
		{
			StringBuilder mvInfo = new StringBuilder(200);
			JObject data;


			if (triggerNumber == 1)
			{
				if (JObject.Parse(json).ContainsKey(character) && JObject.Parse(json)[character]["moves"]["vtOne"]
					.ToObject<JObject>().ContainsKey(moveName))
				{
					data = JObject.Parse(json)[character]["moves"]["vtOne"][moveName].ToObject<JObject>();
					foreach (var attribute in data)
					{
						if (attribute.Key == "cancelsTo")
						{
							mvInfo.AppendLine($"{attribute.Key} :");
							for (int i = 0; i < attribute.Value.Count(); i++)
							{
								mvInfo.AppendLine($"\t{attribute.Value[i]}");
							}
						}
						else if (attribute.Key == "extraInfo")
						{
							mvInfo.AppendLine($"{attribute.Key} :");
							for (int i = 0; i < attribute.Value.Count(); i++)
							{
								mvInfo.AppendLine($"\t{attribute.Value[i]}");
							}
						}
						else
						{
							mvInfo.AppendLine($"{attribute.Key} - {attribute.Value}");
						}
					}

					return mvInfo.ToString();
				}
				else
				{
					return "No data.";
				}
			}

			else if (triggerNumber == 2)
			{
				if (JObject.Parse(json).ContainsKey(character) && JObject.Parse(json)[character]["moves"]["vtTwo"]
					.ToObject<JObject>().ContainsKey(moveName))
				{
					data = JObject.Parse(json)[character]["moves"]["vtTwo"][moveName].ToObject<JObject>();

					foreach (var attribute in data)
					{
						if (attribute.Key == "cancelsTo")
						{
							mvInfo.AppendLine($"{attribute.Key} :");
							for (int i = 0; i < attribute.Value.Count(); i++)
							{
								mvInfo.AppendLine($"\t{attribute.Value[i]}");
							}
						}
						else if (attribute.Key == "extraInfo")
						{
							mvInfo.AppendLine($"{attribute.Key} :");
							for (int i = 0; i < attribute.Value.Count(); i++)
							{
								mvInfo.AppendLine($"\t{attribute.Value[i]}");
							}
						}
						else
						{
							mvInfo.AppendLine($"{attribute.Key} - {attribute.Value}");
						}
					}

					return mvInfo.ToString();
				}
				else
				{
					return "No data.";
				}
			}

			else
			{
				mvInfo.AppendLine("No data.");
				return mvInfo.ToString();
			}
		}

		/// <summary>
		/// Returns all character stats.
		/// </summary>
		/// <param name="character">Character name.</param>
		/// <returns></returns>
		public static string GetStats(string character)
		{
			StringBuilder statsList = new StringBuilder(150);

			if (JObject.Parse(json).ContainsKey(character))
			{
				var data = JObject.Parse(json)[character]["stats"].ToObject<JObject>();

				foreach (var stat in data)
				{
					if (stat.Key != "threeLetterCode" && stat.Key != "color")
						statsList.AppendLine($"{stat.Key} - {stat.Value}");
				}

				return statsList.ToString();
			}

			statsList.AppendLine("No data.");
			return statsList.ToString();
		}

		/// <summary>
		/// Returns all available moves of specified V-Trigger.
		/// </summary>
		/// <param name="character">Character name.</param>
		/// <param name="triggerNumber">Trigger number.</param>
		/// <returns></returns>
		public static string GetTriggerMoveList(string character, int triggerNumber)
		{
			StringBuilder trigerInfo = new StringBuilder(200);

			if (triggerNumber == 1)
			{
				if (JObject.Parse(json).ContainsKey(character))
				{
					var data = JObject.Parse(json)[character]["moves"]["vtOne"].ToObject<JObject>();

					foreach (var move in data)
					{
						trigerInfo.AppendLine(move.Key);
					}

					return trigerInfo.ToString();
				}
			}
			else if (triggerNumber == 2)
			{
				if (JObject.Parse(json).ContainsKey(character))
				{
					var data = JObject.Parse(json)[character]["moves"]["vtTwo"].ToObject<JObject>();

					foreach (var move in data)
					{
						trigerInfo.AppendLine(move.Key);
					}

					return trigerInfo.ToString();
				}
			}

			trigerInfo.AppendLine("No data.");
			return trigerInfo.ToString();
		}

		/// <summary>
		/// Returns list of all available characters.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static string GetCharacterList()
		{
			StringBuilder characterList = new StringBuilder(200);

			var data = JObject.Parse(json);

			foreach (var character in data)
			{
				characterList.AppendLine(character.Key);
			}

			return characterList.ToString();
		}

		/// <summary>
		/// Returns character move list.
		/// </summary>
		/// <param name="character">Character name.</param>
		/// <returns></returns>
		public static string GetMoveList(string character)
		{
			StringBuilder sb = new StringBuilder(150);

			if (JObject.Parse(json).ContainsKey(character))
			{
				var data = JObject.Parse(json)[character]["moves"]["normal"].ToObject<JObject>();
				foreach (var move in data)
				{
					sb.AppendLine(move.Key);
				}

				return sb.ToString();
			}
			else
			{
				return "No data.";
			}
		}
		/// <summary>
		/// Check user request for correctness and form text answer.
		/// </summary>
		/// <returns></returns>
		public static string BotAnswer(MessageEventArgs e)
		{
			string result = "No data.";

			switch (e.Message.Type)
			{
				case MessageType.Text: //check for type
					{
						#region Text message answer

						string msgText = e.Message.Text; //user message

						if (msgText.Length < 3) //if message legth < than 3 return error
						{
							return "Введите корректную команду.";
						}

						//Turns character name to lower case for better UX
						string lowerCaseName = msgText.Substring(1);
						lowerCaseName = char.ToLower(lowerCaseName[0]).ToString() + lowerCaseName.Substring(1);

						//Available commands
						switch (msgText)
						{
							case "/move":
								return "Введите данные в формате /move [character name] [move name]";
							case "/start":
								return "Введите /commands для отображения всех команд.";
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

						string[] tempData; //Array for parse data.
						char[] separator = new[] { ' ' };

						#region Proper divide


						//Commands /vt1, /vt2, /move required split for 3 parts
						if (msgText.Contains("/vt1") || msgText.Contains("/vt2") || msgText.Contains("/move"))
						{
							tempData = msgText.Split(separator, 3);
						}
						//Commands /movelist, /stats, /vt1movelist, /vt2movelist required split for 2 parts
						else if (msgText.Contains("/movelist") || msgText.Contains("/stats") || msgText.Contains("/vt1movelist") || msgText.Contains("/vt2movelist"))
						{
							tempData = msgText.Split(separator, 2);
						}
						else
						{
							return result;
						}

						#endregion

						string tempName = char.ToUpper(tempData[1][0]) + tempData[1].Substring(1); //variable that contains character name in lower case.

						switch (tempData.Length)
						{
							case 2:
								if (tempData[0] == "/movelist")
								{
									return result = JsonParseFrameData.GetCharacterList().Contains(tempName) ? JsonParseFrameData.GetMoveList(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1)) : "No data.";
								}
								else if (tempData[0] == "/stats")
								{
									return result = JsonParseFrameData.GetCharacterList().Contains(tempName) ? JsonParseFrameData.GetStats(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1)) : "No data.";
								}
								else if (tempData[0] == "/vt1movelist")
								{
									return result = JsonParseFrameData.GetCharacterList().Contains(tempName) ? JsonParseFrameData.GetTriggerMoveList(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1), 1) : "No data.";

								}
								else if (tempData[0] == "/vt2movelist")
								{
									return result = JsonParseFrameData.GetCharacterList().Contains(tempName) ? JsonParseFrameData.GetTriggerMoveList(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1), 2) : "No data.";
								}
								break;
							case 3:
								if (tempData[0] == "/vt1")
								{
									return result = JsonParseFrameData.GetCharacterList().Contains(tempName) ? JsonParseFrameData.GetTriggerMoveInfo(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1), 1, tempData[2]) : "No data.";
								}
								else if (tempData[0] == "/vt2")
								{
									return result = JsonParseFrameData.GetCharacterList().Contains(tempName) ? JsonParseFrameData.GetTriggerMoveInfo(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1), 2, tempData[2]) : "No data.";
								}
								else if (tempData[0] == "/move")
								{
									return result = JsonParseFrameData.GetCharacterList().Contains(tempName) ? JsonParseFrameData.GetMoveInfo(char.ToUpper(tempData[1][0]).ToString() + tempData[1].Substring(1), tempData[2]) : "No data.";
								}
								break;
						}

						#endregion
					}
					break;
				case MessageType.Document:
					{
						result = "I don't know what to do with documents.";
					}
					break;
				case MessageType.Photo:
					{
						result = "I don't know what to do with images.";
					}
					break;
				case MessageType.Audio:
					{
						result = "I don't work with audio, sorry.";
					}
					break;
			}

			return result;
		}

		#endregion
	}
}