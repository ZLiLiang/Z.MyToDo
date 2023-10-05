using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using Z.MyToDo.Common;
using Z.MyToDo.Service;
using Z.MyToDo.View;
using Z.MyToDo.ViewModels;
using Z.MyToDo.ViewModels.Dialogs;
using Z.MyToDo.Views;
using Z.MyToDo.Views.Dialogs;

namespace Z.MyToDo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return base.Container.Resolve<MainView>();
        }

        protected override void OnInitialized()
        {
            var dialog = base.Container.Resolve<IDialogService>();

            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }

                var service = App.Current.MainWindow.DataContext as IConfigureService;
                if (service != null)
                {
                    service.Configure();
                }
                base.OnInitialized();
            });
        }

        public static void LoginOut(IContainerProvider containerProvider)
        {
            Current.MainWindow.Hide();

            var dialog = containerProvider.Resolve<IDialogService>();

            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }

                Current.MainWindow.Show();
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            containerRegistry.GetContainer().RegisterInstance(@"http://localhost:41295/", serviceKey: "webUrl");

            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<IToDoService, ToDoService>();
            containerRegistry.Register<IMemoService, MemoService>();
            containerRegistry.Register<IDialogHostService, DialogHostService>();

            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();

            containerRegistry.RegisterForNavigation<AddToDoView, AddToDoViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoView, AddMemoViewModel>();
            containerRegistry.RegisterForNavigation<AboutView>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<ToDoView, ToDoViewModel>();
            containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
        }
    }
}
