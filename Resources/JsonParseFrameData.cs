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

		#endregion
	}
}