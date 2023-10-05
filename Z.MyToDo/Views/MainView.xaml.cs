using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Z.MyToDo.Common;
using Z.MyToDo.Extensions;
using Z.MyToDo.Views;

namespace Z.MyToDo.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IDialogHostService _dialogHostService;

        public MainView(IEventAggregator aggregator,
            IDialogHostService dialogHostService)
        {
            InitializeComponent();
            this._dialogHostService = dialogHostService;

            //注册提示消息
            aggregator.ResgiterMessage(arg =>
            {
                Snackbar.MessageQueue.Enqueue(arg);
            });

            //注册等待消息窗口
            aggregator.Resgiter(arg =>
            {
                DialogHost.IsOpen = arg.IsOpen;

                if (DialogHost.IsOpen)
                {
                    DialogHost.DialogContent = new ProgressView();
                }
            });

            menuBar.SelectionChanged += MenuBar_SelectionChanged;
            ColorZone.MouseDoubleClick += ColorZone_MouseDoubleClick;
            ColorZone.MouseMove += ColorZone_MouseMove;
            btnClose.Click += BtnClose_Click;
            btnMax.Click += BtnMax_Click;
            btnMin.Click += BtnMin_Click;
        }

        private void MenuBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            drawerHost.IsLeftDrawerOpen = false;
        }

        private void ColorZone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void ColorZone_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private async void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = await _dialogHostService.Question("温馨提示", "确认退出系统?");
            if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK)
            {
                return;
            }
            this.Close();
        }

        private void BtnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
