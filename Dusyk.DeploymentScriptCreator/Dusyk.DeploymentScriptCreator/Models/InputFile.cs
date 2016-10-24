namespace Dusyk.DeploymentScriptCreator.Models
{
	/// <summary>
	/// Describes a file to be added to the deployment script.
	/// </summary>
	public class InputFile
	{
		/// <summary>
		/// Name of the file.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Fully qualified path, including the file name.
		/// </summary>
		public string FileNameWithPath { get; set; }
	}
}