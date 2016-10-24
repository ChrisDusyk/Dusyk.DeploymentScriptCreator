using Dusyk.DeploymentScriptCreator.Models;
using Dusyk.DeploymentScriptCreator.Oracle;
using Dusyk.DeploymentScriptCreator.Util;
using Microsoft.WindowsAPICodePack.Dialogs;
using NLog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Dusyk.DeploymentScriptCreator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// List of files to be added to the deployment script. This is an ObservableCollection as it triggers updates to the ListBox view upon any change to the collection.
		/// </summary>
		private ObservableCollection<InputFile> _inputFileList;

		/// <summary>
		/// NLog logger instance for the MainWindow.
		/// </summary>
		private static Logger _logger = LogManager.GetLogger("MainWpfWindow");

		public MainWindow()
		{
			InitializeComponent();

			// Set up the _inputFileList collection as the data source for the InputFilesListBox control
			_inputFileList = new ObservableCollection<InputFile>();
			InputFilesListBox.DataContext = _inputFileList;
		}

		/// <summary>
		/// Click event for selecting the output folder of the deployment script. This opens a folder selection dialog and populates the TextBox with the folder path.
		/// </summary>
		/// <param name="sender">Sender object</param>
		/// <param name="e">Event args, not used by the handler</param>
		private void OutputFolderDialogSelector_Click(object sender, RoutedEventArgs e)
		{
			var folderDialog = new CommonOpenFileDialog();

			// Set up dialog for selecting output folder
			folderDialog.AllowNonFileSystemItems = true;
			folderDialog.IsFolderPicker = true;
			folderDialog.Title = "Select script output folder";

			var result = folderDialog.ShowDialog();

			switch (result)
			{
				case CommonFileDialogResult.Ok:
					var folder = folderDialog.FileName;
					OutputFolderTextbox.Text = folder;
					OutputFolderTextbox.ToolTip = folder;

					break;

				case CommonFileDialogResult.Cancel:
				default:
					OutputFolderTextbox.Text = null;
					OutputFolderTextbox.ToolTip = null;
					break;
			}
		}

		/// <summary>
		/// Click even handler for adding files to the input files list. This opens the mutli-select file picker dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InputFilesDialogSelector_Click(object sender, RoutedEventArgs e)
		{
			var fileDialog = new CommonOpenFileDialog();

			fileDialog.AllowNonFileSystemItems = true;
			fileDialog.IsFolderPicker = false;
			fileDialog.Multiselect = true;
			fileDialog.Title = "Select scripts to include";

			var result = fileDialog.ShowDialog();

			switch (result)
			{
				case CommonFileDialogResult.Ok:
					// Add all files selected to the _inputFileList
					var files = fileDialog.FileNames;

					foreach (var file in files)
					{
						InputFile addedFile = new InputFile()
						{
							FileName = file,
							FileNameWithPath = file
						};

						_inputFileList.Add(addedFile);
					}
					break;

				case CommonFileDialogResult.Cancel:
				default:
					break;
			}
		}

		/// <summary>
		/// Click event handler to create the deployment script based on the database variant.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GenerateScriptButton_Click(object sender, RoutedEventArgs e)
		{
			// Set up for creating an Oracle deployment script
			OraclePackageCreator oraclePackage = new OraclePackageCreator()
			{
				OutputFileName = OutputFileNameText.Text,
				OutputPath = OutputFolderTextbox.Text
			};

			oraclePackage.Files = new List<InputFile>();

			foreach (var file in _inputFileList)
			{
				oraclePackage.Files.Add(file);
			}

			oraclePackage.CreateScript();

			// Once completed, notify the user and clear out the data
			MessageBox.Show($"Script has been created at {oraclePackage.OutputPath}\\{oraclePackage.OutputFileName}", "Script created!", MessageBoxButton.OK);
			ClearAllFields();
		}

		/// <summary>
		/// Click event handler to delete files from the list of input files.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InputFilesDeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (InputFilesListBox.SelectedItem != null)
			{
				// Copy to an array as deleting while iterating through the InputFilesListBox.Items collection causes weird behaviour
				InputFile[] selectedFiles = new InputFile[InputFilesListBox.SelectedItems.Count];
				InputFilesListBox.SelectedItems.CopyTo(selectedFiles, 0);

				foreach (var item in selectedFiles)
				{
					_inputFileList.Remove(item);
				}
			}
		}

		/// <summary>
		/// Click event handler to move the selected file up one spot in the input files list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InputFileUp_Click(object sender, RoutedEventArgs e)
		{
			// Copy to an array as deleting while iterating through the InputFilesListBox.Items collection causes weird behaviour
			InputFile[] selectedFiles = new InputFile[InputFilesListBox.SelectedItems.Count];
			InputFilesListBox.SelectedItems.CopyTo(selectedFiles, 0);

			foreach (var file in selectedFiles)
			{
				int indexA = _inputFileList.IndexOf(file);
				int indexB = indexA - 1;

				_inputFileList = _inputFileList.Swap(indexA, indexB);
			}
		}

		/// <summary>
		/// Click event handler to move the selected file down one spot in the input files list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InputFileDown_Click(object sender, RoutedEventArgs e)
		{
			// Copy to an array as deleting while iterating through the InputFilesListBox.Items collection causes weird behaviour
			InputFile[] selectedFiles = new InputFile[InputFilesListBox.SelectedItems.Count];
			InputFilesListBox.SelectedItems.CopyTo(selectedFiles, 0);

			foreach (var file in selectedFiles)
			{
				int indexA = _inputFileList.IndexOf(file);
				int indexB = indexA + 1;

				_inputFileList = _inputFileList.Swap(indexA, indexB);
			}
		}

		/// <summary>
		/// SelectionChanged handler to check the bounds of the selected items and ensure they can move up or down in the list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InputFilesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			InputFileUp.IsEnabled = true;
			InputFileDown.IsEnabled = true;

			foreach (var file in InputFilesListBox.SelectedItems)
			{
				var typedFile = _inputFileList.FirstOrDefault(f => f.FileNameWithPath == ((InputFile)file).FileNameWithPath);

				if (typedFile == null)
				{
					_logger.Warn($"No file returned from file list for {((InputFile)file)?.FileNameWithPath}");
				}
				else
				{
					if (_inputFileList.IndexOf(typedFile) == 0)
					{
						InputFileUp.IsEnabled = false;
					}
					else if (_inputFileList.IndexOf(typedFile) == _inputFileList.Count - 1)
					{
						InputFileDown.IsEnabled = false;
					}
				}
			}
		}

		/// <summary>
		/// Click handler for ClearInputFiles which clears all items from the input files list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClearInputFiles_Click(object sender, RoutedEventArgs e)
		{
			_inputFileList.Clear();
		}

		/// <summary>
		/// Clears all UI fields that the user would have filled out.
		/// </summary>
		private void ClearAllFields()
		{
			_inputFileList.Clear();

			OutputFileNameText.Text = null;
			OutputFileNameText.ToolTip = null;

			OutputFolderTextbox.Text = null;
			OutputFolderTextbox.ToolTip = null;
		}
	}
}