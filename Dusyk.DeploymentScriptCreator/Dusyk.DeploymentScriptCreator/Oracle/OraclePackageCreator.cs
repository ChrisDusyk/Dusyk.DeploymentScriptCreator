using Dusyk.DeploymentScriptCreator.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dusyk.DeploymentScriptCreator.Oracle
{
	public class OraclePackageCreator : IPackageCreator
	{
		public List<string> Files { get; set; }

		public string OutputPath { get; set; }

		public string OutputFileName { get; set; }

		private static Logger _logger = LogManager.GetCurrentClassLogger();

		public bool CreateScript()
		{
			try
			{
				using (FileStream outputWriter = new FileStream($"{OutputPath}\\{OutputFileName}", FileMode.Create))
				{
					foreach (string fileName in Files)
					{
						// Read all text from the file. File.ReadAllText closes the file when completed.
						string fileContents = File.ReadAllText(fileName);

						StringBuilder outputBuilder = new StringBuilder();
						outputBuilder.Append(fileContents);
						outputBuilder.AppendLine();
						outputBuilder.AppendLine("COMMIT;");
						outputBuilder.AppendLine();

						byte[] outputBytes = Encoding.UTF8.GetBytes(outputBuilder.ToString());

						outputWriter.Write(outputBytes, 0, outputBytes.Length);

						_logger.Info("File: {0} added to script.", fileName);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, ex.Message);
				return false;
			}

			return true;
		}
	}
}
