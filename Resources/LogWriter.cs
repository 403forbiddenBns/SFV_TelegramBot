using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Resources
{
	public class LogWriter
	{
		private List<LogItem> logCol = new List<LogItem>();
		private string filename;

		#region ctor

		public LogWriter(string filename, List<LogItem> col)
		{
			this.filename = filename;
			logCol = col;
		}

		public void Process()
		{
			List<string> outputLines = new List<string>();
			foreach (var logItem in logCol)
			{
				outputLines.Add($"{logItem.Date} | {logItem.Name} : {logItem.Message}");
			}

			StreamWriter stream = new StreamWriter(filename, true);

			try
			{
				foreach (var line in outputLines)
				{
					stream.WriteLine(line);
				}
				stream.Close();
			}
			catch { }
			
		}

		#endregion
	}
}
