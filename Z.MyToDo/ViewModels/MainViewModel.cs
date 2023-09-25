﻿using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MyToDo.Common;
using Z.MyToDo.Common.Models;
using Z.MyToDo.Extensions;

namespace Z.MyToDo.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand LoginOutCommand { get; private set; }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand GoForwardCommand { get; private set; }

        private ObservableCollection<MenuBar> menuBars;
        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set
            {
                menuBars = value;
                RaisePropertyChanged();
            }
        }

        private readonly IContainerProvider containerProvider;
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;

        public MainViewModel(IContainerProvider containerProvider, IRegionManager regionManager)
        {
            this.containerProvider = containerProvider;
            this.regionManager = regionManager;

            MenuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
            GoForwardCommand = new DelegateCommand(GoForward);
            LoginOutCommand = new DelegateCommand(LoginOut);
            this.containerProvider = containerProvider;
            this.regionManager = regionManager;
        }

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public void Configure()
        {
            CreateMenuBar();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }

        private void LoginOut()
        {

        }

        private void GoForward()
        {
            if (journal != null && journal.CanGoForward)
            {
                journal.GoForward();
            }
        }

        private void GoBack()
        {
            if (journal != null && journal.CanGoBack)
            {
                journal.GoBack();
            }
        }

        private void Navigate(MenuBar bar)
        {
            if (bar == null || string.IsNullOrWhiteSpace(bar.NameSpace))
            {
                return;
            }

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(bar.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }

        private void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar { Icon = "NoteBookOutline", Title = "待办事项", NameSpace = "ToDoView" });
            MenuBars.Add(new MenuBar { Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
            MenuBars.Add(new MenuBar { Icon = "Cog", Title = "设置", NameSpace = "SettingsView" });
        }
    }
}
