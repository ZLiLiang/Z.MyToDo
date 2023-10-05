using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MyToDo.Common.Models;
using Z.MyToDo.Extensions;

namespace Z.MyToDo.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }

        private ObservableCollection<MenuBar> menuBars;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; }
        }


        public SettingsViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            MenuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            CreateMenuBar();
        }

        private void Navigate(MenuBar bar)
        {
            if (bar == null || string.IsNullOrWhiteSpace(bar.NameSpace))
            {
                return;
            }

            regionManager.Regions[PrismManager.SettingsViewRegionName].RequestNavigate(bar.NameSpace);
        }

        private void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Palette", Title = "个性化", NameSpace = "SkinView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "系统设置", NameSpace = "" });
            MenuBars.Add(new MenuBar() { Icon = "Information", Title = "关于更多", NameSpace = "AboutView" });
        }
    }
}
