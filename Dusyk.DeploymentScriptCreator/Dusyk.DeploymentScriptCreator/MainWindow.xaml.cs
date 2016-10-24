using Dusyk.DeploymentScriptCreator.Models;
using Dusyk.DeploymentScriptCreator.Oracle;
using Dusyk.DeploymentScriptCreator.Util;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using NLog;

namespace Dusyk.DeploymentScriptCreator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string _outputFolder;
		private ObservableCollection<InputFile> _inputFileList;
		private static Logger _logger = LogManager.GetLogger("MainWpfWindow");

		public MainWindow()
		{
			InitializeComponent();

			_inputFileList = new ObservableCollection<InputFile>();
			InputFilesListBox.DataContext = _inputFileList;
		}

		private void OutputFolderDialogSelector_Click(object sender, RoutedEventArgs e)
		{
			var folderDialog = new CommonOpenFileDialog();

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

					_outputFolder = folderDialog.FileName;

					break;

				case CommonFileDialogResult.Cancel:
				default:
					OutputFolderTextbox.Text = null;
					OutputFolderTextbox.ToolTip = null;
					break;
			}
		}

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

		private void GenerateScriptButton_Click(object sender, RoutedEventArgs e)
		{
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
		}

		private void InputFilesDeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (InputFilesListBox.SelectedItem != null)
			{
				
				InputFile[] selectedFiles = new InputFile[InputFilesListBox.SelectedItems.Count];
				InputFilesListBox.SelectedItems.CopyTo(selectedFiles, 0);

				foreach (var item in selectedFiles)
				{
					_inputFileList.Remove(item);
				}
			}
		}

		private void InputFileUp_Click(object sender, RoutedEventArgs e)
		{
			InputFile[] selectedFiles = new InputFile[InputFilesListBox.SelectedItems.Count];
			InputFilesListBox.SelectedItems.CopyTo(selectedFiles, 0);

			foreach (var file in selectedFiles)
			{
				int indexA = _inputFileList.IndexOf(file);
				int indexB = indexA - 1;

				_inputFileList = _inputFileList.Swap(indexA, indexB);
			}
		}

		private void InputFileDown_Click(object sender, RoutedEventArgs e)
		{
			InputFile[] selectedFiles = new InputFile[InputFilesListBox.SelectedItems.Count];
			InputFilesListBox.SelectedItems.CopyTo(selectedFiles, 0);

			foreach (var file in selectedFiles)
			{
				int indexA = _inputFileList.IndexOf(file);
				int indexB = indexA + 1;

				_inputFileList = _inputFileList.Swap(indexA, indexB);
			}
		}

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
	}
}