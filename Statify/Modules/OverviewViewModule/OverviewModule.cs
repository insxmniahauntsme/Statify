using Statify.Modules.OverviewViewModule.ViewModels;
using Statify.Modules.OverviewViewModule.Views;

namespace Statify.Modules.OverviewViewModule;

public class OverviewModule : IModule
{
	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.Register<OverviewViewModel>();
		containerRegistry.RegisterForNavigation<OverviewView>();
	}

	public void OnInitialized(IContainerProvider containerProvider)
	{
		var regionManager = containerProvider.Resolve<IRegionManager>();
		
		regionManager.RegisterViewWithRegion("MainRegion", typeof(OverviewView));
	}
}