using Statify.Modules.LoginViewModule.Views;

namespace Statify.Modules.LoginViewModule;

public class LoginModule : IModule
{
	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.RegisterForNavigation<LoginView>();
	}

	public void OnInitialized(IContainerProvider containerProvider)
	{
		var regionManager = containerProvider.Resolve<IRegionManager>();

		regionManager.RequestNavigate("MainRegion", nameof(LoginView));
	}
}