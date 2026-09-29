using System.Windows;
using Warehouse.Core.Services;

namespace Warehouse
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                SaveService.LoadFromJson(SaveService.DefaultPath);
            }
            catch
            {
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                SaveService.SaveToJson(SaveService.DefaultPath);
            }
            catch
            {
            }

            base.OnExit(e);
        }
    }
}