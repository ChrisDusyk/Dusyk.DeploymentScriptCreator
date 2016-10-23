using Dusyk.DeploymentScriptCreator.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Dusyk.DeploymentScriptCreator.Util
{
	public static class Extensions
	{
		public static ObservableCollection<InputFile> RecalculateSortOrder(this ObservableCollection<InputFile> inputFiles)
		{
			var orderedList = new ObservableCollection<InputFile>(inputFiles.OrderBy(files => files.SortOrder));
			int sortIndex = 0;

			foreach (var file in orderedList)
			{
				file.SortOrder = sortIndex;
				sortIndex++;
			}

			return orderedList;
		}

		public static ObservableCollection<InputFile> Swap<InputFile>(this ObservableCollection<InputFile> list, int indexA, int indexB)
		{
			InputFile temp = list[indexA];
			list[indexA] = list[indexB];
			list[indexB] = temp;

			return list;
		}
	}
}