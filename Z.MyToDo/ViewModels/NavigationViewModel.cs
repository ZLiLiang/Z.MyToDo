using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Z.MyToDo.Extensions;

namespace Z.MyToDo.ViewModels
{
    public class NavigationViewModel : BindableBase, INavigationAware
    {
        private readonly IContainerProvider containerProvider;
        public readonly IEventAggregator aggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
            this.aggregator = containerProvider.Resolve<IEventAggregator>();
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public void UpdateLoading(bool IsOpen)
        {
            aggregator.UpdateLoading(new Common.Events.UpdateModel
            {
                IsOpen = IsOpen
            });
        }
    }
}
