using PasswordManager.ViewModels;
using PasswordManager.ViewModels.PasswordViewModels;
using PasswordManager.Stores;
using PasswordManager.Views;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Services;
using PasswordManager.ViewModels.PasswordViewModels.Components;
using PasswordManager.Domain.Models;
using System.Collections.ObjectModel;
using PasswordManager.Domain.DataAccess.Password;
using PasswordManager.Domain.DataAccess.Setting;
using PasswordManager.Domain.Commands;
using PasswordManager.Data.Commands.AddCommands;
using PasswordManager.Data.Commands.DeleteCommands;
using PasswordManager.Data.Commands.GetAllCommands;
using PasswordManager.Data.Commands.UpdateCommands;
using PasswordManager.Data.Commands.OtherCommands;
using PasswordManager.ViewModels.PopUpViewModels;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //.net genereic host is doing more than dependency injection
        //It also handles app configuration with app settings like Json and other infrastructures like login, But This program is Handle own setting parts
        private IHost host;

        public App()
        {
            //The followings are registers as a singeltone since they store our application state.
            //Register our services
            host = Host.CreateDefaultBuilder().ConfigureServices((context, service) =>
            {
                //ObservableCollection<string> fileTypeList = new ObservableCollection<string>() { "TXT", "JSON", "XML", "MYSQL", "MSSQL", "MONGODB", "ORACLEDB" };
                //ObservableCollection<string> encryptionTypeList = new ObservableCollection<string>() { "Hashing", "Symetric AES", "Symetric Rijndael" };

                ObservableCollection<string> fileTypeList = new ObservableCollection<string>() { "TXT", "JSON" };
                ObservableCollection<string> encryptionTypeList = new ObservableCollection<string>() { "Symetric AES", "Symetric Rijndael" };

                //The followings are registers as a singeltone since they store our application state.
                //Register our services

                service.AddSingleton<NavigationStore>();
                service.AddSingleton<ModalNavigationStore>();
                service.AddSingleton<SettingModel>((x) => new SettingModel(new SettingDataModel()));
                service.AddSingleton<SettingDataStore>(x => new SettingDataStore(encryptionTypeList, fileTypeList, x.GetRequiredService<SettingModel>(), CreateSettingDao(x)));
                service.AddSingleton<PasswordDao>((x) => CreatePasswordDao(x));
                service.AddSingleton<PasswordListStore>((x) => new PasswordListStore( x.GetRequiredService<PasswordDao>(), x.GetRequiredService<SettingDataStore>()));

                service.AddSingleton<IGetAllItemWithSettingCommand<PasswordModel>, GetAllPasswordItemCommand>();
                service.AddSingleton<IDeleteItemWithSettingCommand<PasswordModel>, DeletePasswordItemCommand>();
                service.AddSingleton<IUpdateItemWithSettingCommand<PasswordModel>, UpdatePasswordItemCommand>();
                service.AddSingleton<IAddItemWithSettingCommand<PasswordModel>, AddPasswordItemCommand>();
                service.AddSingleton<IUpdateItemsWithSettingCommand<PasswordModel>, UpdatePasswordItemsCommand>();

                service.AddSingleton<IGetItemWithSettingCommand<SettingModel>, GetSettingItemCommand >();
                service.AddSingleton<IAddItemReturnWithItCommand<SettingModel>, AddSettingItemCommand>();

                service.AddSingleton<IDeleteFileCommand, DeleteFileCommand>();

                service.AddSingleton<IReversePasswordCommand, ReversePasswordCommand>();

                service.AddSingleton<SettingViewModel>(x => new SettingViewModel(x.GetRequiredService<SettingDataStore>(), x.GetRequiredService<PasswordListStore>()));
               
                //Not need a factory function because all the dependencies for the service are already registered in dependency injection
                service.AddSingleton<CloseModalNavigationService>();

                //Navigation pages are initial register here and using the defined page
                service.AddSingleton<INavigationService>(x => CreateWelcomeNavigationService(x));

                service.AddSingleton<WelcomeViewModel>(x => CreateWelcomeViewModel(x));

                //Register ViewModel as Trnsient in Dependency Injection 
                //Using Transient because the ViewModels are disposed
                //Case of disposing ViewModels, then they will not be resuable again what results a new istance of them.
                //generate the same instance everytime even it was disposed of thats instance

                service.AddTransient<PasswordViewModel>(x => CreatePasswordViewModel(x));

                service.AddTransient<DashBoardViewModel>(x => new DashBoardViewModel(CreateNavigationSideModel(x)));
                service.AddTransient<PasswordListViewModel>(x => CreatePasswordListViewModelLoad(x));
                service.AddTransient<AddPasswordViewModel>(x => new AddPasswordViewModel(CreateAddPasswordDetailFormViewModel(x), x.GetRequiredService<PasswordListStore>(), x.GetRequiredService<CloseModalNavigationService>()));
                service.AddTransient<EditPasswordViewModel>(x => new EditPasswordViewModel(x.GetRequiredService<PasswordListStore>(), x.GetRequiredService<PasswordModel>(), x.GetRequiredService<CloseModalNavigationService>()));

                service.AddSingleton<AboutViewModel>(x => new AboutViewModel());
                service.AddTransient<ConformationViewModel>(x => new ConformationViewModel("Header", "Descrioption"));
                service.AddTransient<SearchPasswordViewModel>(x => new SearchPasswordViewModel());
                service.AddTransient<NavigationSideViewModel>(CreateNavigationSideModel);

                //Register MainViewModel since only one of it is used in the whole application
                //The MainViewModel depends on the NavigationStore and  the ModalNavigationStore what are already registered in the services earlier
                service.AddSingleton<MainWindowViewModel>();

                //Register MainWindow as the following since only one of it used.
                //Using a factory function since the DateContext should be set here
                service.AddSingleton<MainWindow>(x => new MainWindow()
                {
                    DataContext = x.GetRequiredService<MainWindowViewModel>()
                });
            }).Build();
        }

        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            host.Start();
            //One Solution --> register all navigation service into the provider to one by one
            //make sence to register multiple INavigationServices
            INavigationService navigationSerice = host.Services.GetRequiredService<INavigationService>();
            navigationSerice.Navigate();

            var MainWindow = host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }

        private PasswordDao CreatePasswordDao(IServiceProvider serviceProvider) =>
            new PasswordDao(serviceProvider.GetServices<IGetAllItemWithSettingCommand<PasswordModel>>().FirstOrDefault(x => x.GetType() == typeof(GetAllPasswordItemCommand)),
                serviceProvider.GetServices<IAddItemWithSettingCommand<PasswordModel>>().FirstOrDefault(x => x.GetType() == typeof(AddPasswordItemCommand)),
                serviceProvider.GetServices<IUpdateItemsWithSettingCommand<PasswordModel>>().FirstOrDefault(x => x.GetType() == typeof(UpdatePasswordItemsCommand)),
                serviceProvider.GetServices<IDeleteItemWithSettingCommand<PasswordModel>>().FirstOrDefault(x => x.GetType() == typeof(DeletePasswordItemCommand)),
                serviceProvider.GetServices<IUpdateItemWithSettingCommand<PasswordModel>>().FirstOrDefault(x => x.GetType() == typeof(UpdatePasswordItemCommand)),
                serviceProvider.GetService<IDeleteFileCommand>(), serviceProvider.GetService<IReversePasswordCommand>());

        private SettingDao CreateSettingDao(IServiceProvider serviceProvider) =>
            new SettingDao(serviceProvider.GetServices<IGetItemWithSettingCommand<SettingModel>>().FirstOrDefault(x => x.GetType() == typeof(GetSettingItemCommand)),
                serviceProvider.GetServices<IAddItemReturnWithItCommand<SettingModel>>().FirstOrDefault(x => x.GetType() == typeof(AddSettingItemCommand)),
                serviceProvider.GetServices<IUpdateItemReturnWithItCommand<SettingModel>>().FirstOrDefault(x => x.GetType() == typeof(UpdateSettingItemCommand)));

        private PasswordListViewModel CreatePasswordListViewModelLoad(IServiceProvider serviceProvider) =>
            PasswordListViewModel.LoadPasswordListViewModel(serviceProvider.GetRequiredService<PasswordListStore>(),
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                serviceProvider.GetRequiredService<CloseModalNavigationService>());

        private WelcomeViewModel CreateWelcomeViewModel(IServiceProvider serviceProvider)
        {
            CompositeNavigationService navigationService =
                new CompositeNavigationService(serviceProvider.GetRequiredService<CloseModalNavigationService>(),
                CreateDashBoardNavigationService(serviceProvider));

            return new WelcomeViewModel(serviceProvider.GetRequiredService<SettingDataStore>(), navigationService, CreateNavigationSideModel(serviceProvider));
        }

        private PasswordDetailFormViewModel CreateAddPasswordDetailFormViewModel(IServiceProvider serviceProvider) =>
            new PasswordDetailFormViewModel("Add");

        private INavigationService CreateAddPasswordNavigationViewModel(IServiceProvider serviceProvider) =>
            new NavigationService<AddPasswordViewModel>(serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<AddPasswordViewModel>());

        private INavigationService CreateWelcomeNavigationService(IServiceProvider serviceProvider) =>
            new NavigationService<WelcomeViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), 
                () => serviceProvider.GetRequiredService<WelcomeViewModel>());
        private INavigationService CreateDashBoardNavigationService(IServiceProvider serviceProvider) =>
                new NavigationService<DashBoardViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<DashBoardViewModel>());

        private INavigationService CreateSettingdNavigationService(IServiceProvider serviceProvider) =>
                new NavigationLayoutService<SettingViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<SettingViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationSideViewModel>(), "Setting - Content");

        private INavigationService CreateAboutNavigationService(IServiceProvider serviceProvider) =>
                new NavigationLayoutService<AboutViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<AboutViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationSideViewModel>(), "Setting - Content");


        private INavigationService CreatePasswordNavigationService(IServiceProvider serviceProvider) =>
                new NavigationLayoutService<PasswordViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<PasswordViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationSideViewModel>(), "Setting - Content");


        private NavigationSideViewModel CreateNavigationSideModel(IServiceProvider serviceProvider)
        => new NavigationSideViewModel(
            CreateSettingdNavigationService(serviceProvider),
            CreateDashBoardNavigationService(serviceProvider),
            CreateAboutNavigationService(serviceProvider),
            CreatePasswordNavigationService(serviceProvider),
            serviceProvider.GetRequiredService<NavigationStore>());

        private PasswordViewModel CreatePasswordViewModel(IServiceProvider serviceProvider)
            => new PasswordViewModel(serviceProvider.GetRequiredService<PasswordListStore>(),
                serviceProvider.GetRequiredService<PasswordListViewModel>(),
                CreatePasswordListViewModelLoad(serviceProvider),
                CreateAddPasswordNavigationViewModel(serviceProvider),
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                serviceProvider.GetRequiredService<CloseModalNavigationService>());

        protected void Application_Exit(object sender, ExitEventArgs e)
        {
            host.StopAsync();
            host.Dispose();
        }
    }
}
