using Dusyk.DeploymentScriptCreator.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Dusyk.DeploymentScriptCreator.Util
{
	public static class Extensions
	{
		public static ObservableCollection<InputFile> Swap<InputFile>(this ObservableCollection<InputFile> list, int indexA, int indexB)
		{
			InputFile temp = list[indexA];

			list.RemoveAt(indexA);
			list.Insert(indexB, temp);

			return list;
		}
	}
}