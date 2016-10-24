using System.Collections.ObjectModel;

namespace Dusyk.DeploymentScriptCreator.Util
{
	/// <summary>
	/// General extension methods.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Swap two entries in the collection.
		/// </summary>
		/// <typeparam name="InputFile"></typeparam>
		/// <param name="list">Extension syntax for the list being operated on.</param>
		/// <param name="indexA">Index of the first item.</param>
		/// <param name="indexB">Index of the second item.</param>
		/// <returns>List with swap applied.</returns>
		public static ObservableCollection<InputFile> Swap<InputFile>(this ObservableCollection<InputFile> list, int indexA, int indexB)
		{
			InputFile temp = list[indexA];

			list.RemoveAt(indexA);
			list.Insert(indexB, temp);

			return list;
		}
	}
}