using Dusyk.DeploymentScriptCreator.Models;
using System.Collections.Generic;

namespace Dusyk.DeploymentScriptCreator.Interfaces
{
	internal interface IPackageCreator
	{
		string OutputPath { get; set; }

		string OutputFileName { get; set; }

		List<InputFile> Files { get; set; }

		bool CreateScript();
	}
}