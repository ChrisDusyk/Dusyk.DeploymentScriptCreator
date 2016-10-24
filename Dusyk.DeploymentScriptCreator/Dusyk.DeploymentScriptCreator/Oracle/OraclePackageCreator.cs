using Dusyk.DeploymentScriptCreator.Interfaces;
using Dusyk.DeploymentScriptCreator.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dusyk.DeploymentScriptCreator.Oracle
{
	/// <summary>
	/// Contains methods to create the deployment script for an Oracle database.
	/// </summary>
	public class OraclePackageCreator : IPackageCreator
	{
		/// <summary>
		/// List of files to add to the deployment script.
		/// </summary>
		public List<InputFile> Files { get; set; }

		/// <summary>
		/// Path to create the deployment script at.
		/// </summary>
		public string OutputPath { get; set; }

		/// <summary>
		/// Filename of the resulting deployment script.
		/// </summary>
		public string OutputFileName { get; set; }

		/// <summary>
		/// NLog logger instance for the OraclePackageCreator.
		/// </summary>
		private static Logger _logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Iterate through the list of files and create a combined deployment script.
		/// </summary>
		/// <returns></returns>
		public bool CreateScript()
		{
			try
			{
				using (FileStream outputWriter = new FileStream($"{OutputPath}\\{OutputFileName}", FileMode.Create))
				{
					foreach (var file in Files)
					{
						// Read all text from the file. File.ReadAllText closes the file when completed.
						string fileContents = File.ReadAllText(file.FileNameWithPath);

						StringBuilder outputBuilder = new StringBuilder();
						outputBuilder.AppendLine($"-- Start File: {file.FileName}");
						outputBuilder.Append(fileContents);
						outputBuilder.AppendLine();
						outputBuilder.AppendLine("COMMIT;");
						outputBuilder.AppendLine($"-- End File: {file.FileName}");
						outputBuilder.AppendLine();

						byte[] outputBytes = Encoding.UTF8.GetBytes(outputBuilder.ToString());

						outputWriter.Write(outputBytes, 0, outputBytes.Length);

						_logger.Info("File: {0} added to script.", file.FileNameWithPath);
					}
				}
			}
			catch (Exception ex)
			{
				// Log the exception and return false for a failed state
				_logger.Error(ex, ex.Message);
				return false;
			}

			return true;
		}
	}
}