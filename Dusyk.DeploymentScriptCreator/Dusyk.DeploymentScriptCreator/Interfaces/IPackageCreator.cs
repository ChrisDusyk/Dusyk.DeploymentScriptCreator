using Dusyk.DeploymentScriptCreator.Models;
using System.Collections.Generic;

namespace Dusyk.DeploymentScriptCreator.Interfaces
{
	/// <summary>
	/// Interface to extend for all deployment script creators.
	/// </summary>
	internal interface IPackageCreator
	{
		/// <summary>
		/// Path to create the deployment script at.
		/// </summary>
		string OutputPath { get; set; }

		/// <summary>
		/// Filename of the resulting deployment script.
		/// </summary>
		string OutputFileName { get; set; }

		/// <summary>
		/// List of files to add to the deployment script.
		/// </summary>
		List<InputFile> Files { get; set; }

		/// <summary>
		/// Iterate through the list of files and create a combined deployment script.
		/// </summary>
		/// <returns></returns>
		bool CreateScript();
	}
}