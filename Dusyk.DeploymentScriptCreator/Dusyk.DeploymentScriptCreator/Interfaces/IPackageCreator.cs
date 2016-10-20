using System.Collections.Generic;

namespace Dusyk.DeploymentScriptCreator.Interfaces
{
	internal interface IPackageCreator
	{
		string OutputPath { get; set; }

		string OutputFileName { get; set; }

		List<string> Files { get; set; }

		bool CreateScript();
	}
}