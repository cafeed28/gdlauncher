using Microsoft.Win32;
using System;
using System.Windows.Controls;

namespace Launcher
{
	public partial class SettingsPage
	{
		private string version = "1.0.4 Beta";
		public SettingsPage()
		{
			InitializeComponent();
			CreditsText.Text = "Adrenaline Launcher " + version + " by caFeeD";
			string ActionOnLaunch = Settings.ReadKey("ActionOnLaunch");
			if (ActionOnLaunch == null)
			{
				ComboBox.SelectedIndex = 2;
				Settings.WriteKey("ActionOnLaunch", "2");
			}
			else
			{
				ComboBox.SelectedIndex = Convert.ToInt32(ActionOnLaunch);
			}
			string GamePath = Settings.ReadKey("GamePath");
			if (GamePath == null)
			{
				GamePathText.Text = "Путь не указан";
			}
			else
			{
				GamePathText.Text = GamePath;
			}
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Settings.WriteKey("ActionOnLaunch", ComboBox.SelectedIndex.ToString());
		}

		private void OpenButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Запускные файлы exe (*.exe)|*.exe";
			if (openFileDialog.ShowDialog() == true)
			{
				Settings.WriteKey("GamePath", openFileDialog.FileName);
				GamePathText.Text = Settings.ReadKey("GamePath");
			}
		}
	}
}
