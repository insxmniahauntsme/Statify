using System.IO;
using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Statify.Modules.LoginViewModule.Views;
using Statify.Modules.LoginViewModule;
using Statify.Servers;
using Statify.Views;

namespace Statify;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	public static IConfiguration? Configuration;
	
	protected override void OnInitialized()
	{
		base.OnInitialized();

		var builder = new ConfigurationBuilder()
			.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
		
		Configuration = builder.Build();
		
		var regionManager = Container.Resolve<IRegionManager>();
		
		regionManager.RequestNavigate("MainRegion", "LoginView");
	}
	protected override void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.Register<HttpClient>( () =>
		{
			var baseUrl = Configuration!["API:base-url"];
			if(string.IsNullOrWhiteSpace(baseUrl))
				throw new Exception("Couldn't get API base url");
			
			var client = new HttpClient
			{
				BaseAddress = new Uri(baseUrl),
				Timeout = TimeSpan.FromSeconds(30)
			};
			
			client.DefaultRequestHeaders.Add("Accept", "application/json");
			
			return client;
		});
		
		containerRegistry.Register<CallbackServer>();
	}
	
	protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
	{
		moduleCatalog.AddModule<LoginModule>();
	}

	
	protected override Window CreateShell()
	{
		return Container.Resolve<MainWindow>();
	}


}