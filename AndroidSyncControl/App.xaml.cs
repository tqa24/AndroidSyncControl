using System.Windows;
using AndroidSyncControl.Themes;

namespace AndroidSyncControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Apply the persisted theme before the main window is created.
            ThemeManager.Apply(ThemeManager.Parse(Singleton.Setting.Setting.Theme));
            base.OnStartup(e);
        }
    }
}
