using Dusyk.DeploymentScriptCreator.Oracle;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Windows;

namespace Dusyk.DeploymentScriptCreator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string _outputFolder;

		public MainWindow()
		{
			InitializeComponent();
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

					InputFilesListBox.Items.Clear();

					foreach (var file in files)
					{
						InputFilesListBox.Items.Add(file);
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

			oraclePackage.Files = new List<string>();

			foreach (var file in InputFilesListBox.Items)
			{
				oraclePackage.Files.Add(file.ToString());
			}

			oraclePackage.CreateScript();
		}

		private void InputFileDelete_Click(object sender, RoutedEventArgs e)
		{
			InputFilesListBox.Items.Remove(((System.Windows.Controls.Button)sender).DataContext);
		}
	}
}