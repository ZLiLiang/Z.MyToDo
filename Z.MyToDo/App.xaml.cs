using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;
using Z.MyToDo.Views;

namespace Z.MyToDo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        public static void LoginOut(IContainerProvider containerProvider)
        {

        }

        protected override void OnInitialized()
        {

            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
