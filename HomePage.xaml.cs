using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace Launcher
{
    public partial class HomePage
    {
        private NotifyIcon notifyIcon;

        public HomePage()
        {
            InitializeComponent();
            NewsFrame.Navigate(typeof(NewsPage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = Settings.ReadKey("GamePath");
            if (path == null)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Путь не указан. Откройте \"Параметры\" и укажите путь к игре",
                    CloseButtonText = "ОК"
                };
                var result = dialog.ShowAsync();
                return;
            }
            try
            {
                Process.Start(path);
            }
            catch (Win32Exception)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Не удаётся найти указанный файл",
                    CloseButtonText = "ОК"
                };
                var result = dialog.ShowAsync();
                return;
            }

            switch (Settings.ReadKey("ActionOnLaunch"))
            {
                case "0":
                    notifyIcon = new NotifyIcon();
                    notifyIcon.Icon = new Icon(System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Assets/Icon.ico")).Stream);
                    notifyIcon.Text = "Adrenaline Launcher";
                    notifyIcon.Visible = true;
                    notifyIcon.Click += new EventHandler(notifyIcon_Click);

                    System.Windows.Application.Current.MainWindow.Visibility = Visibility.Hidden;
                    break;
                case "1":
                    System.Windows.Application.Current.Shutdown();
                    break;
                case "2":
                    break;
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            System.Windows.Application.Current.MainWindow.Visibility = Visibility.Visible;
        }
    }
}
