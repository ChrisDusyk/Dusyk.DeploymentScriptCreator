using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dusyk.DeploymentScriptCreator.Interfaces
{
	interface IPackageCreator
	{
		string OutputPath { get; set; }

		string OutputFileName { get; set; }

		List<string> Files { get; set; }

		bool CreateScript();
	}
}
