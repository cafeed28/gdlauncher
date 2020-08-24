using System;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using ModernWpf.Controls;

namespace Launcher
{
    public partial class MainWindow : Window
    {
        private bool isFontInstalled;

        public MainWindow()
        {
            InitializeComponent();
            string proc = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(proc);
            if (processes.Length > 1)
            {
                MessageBox.Show("Another process is running", "Adrenaline Launcher", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            var fontsCollection = new InstalledFontCollection();
            foreach (var fontFamily in fontsCollection.Families)
            {
                if (fontFamily.Name == "Segoe MDL2 Assets")
                {
                    Console.WriteLine("Segoe MDL2 Assets Finded");
                    isFontInstalled = true;
                    break;
                }
            }
            if (!isFontInstalled)
            {
                File.WriteAllBytes("SegMDL2.ttf", Properties.Resources.SegMDL2);
                MessageBox.Show("Пожалуйста, нажмите кнопку \"Установить\" в открывшемся окне", "Установка шрифта", MessageBoxButton.OK, MessageBoxImage.Warning);
                Process.Start("SegMDL2.ttf");
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // set the initial SelectedItem 
            //Debug.WriteLine("NavView_Loaded");
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "Home")
                {
                    NavView.SelectedItem = item;
                    NavView_Navigate((NavigationViewItem)item);
                    break;
                }
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item as NavigationViewItem);
            }
        }

        private void NavView_Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "Home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;
                case "News":
                    ContentFrame.Navigate(typeof(NewsPage));
                    break;
                case "PlayersTop":
                    ContentFrame.Navigate(typeof(PlayersPage));
                    PlayersPage.Type = "PlayersTop";
                    break;
                case "Moderators":
                    ContentFrame.Navigate(typeof(PlayersPage));
                    PlayersPage.Type = "Moderators";
                    break;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.System)
            {
                e.Handled = true;
            }
        }
    }
}