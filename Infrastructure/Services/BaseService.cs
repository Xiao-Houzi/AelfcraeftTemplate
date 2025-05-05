using System;
using Aelfcraeft.Infrastructure.Management;

namespace Aelfcraeft.Infrastructure.Services
{
    public abstract class BaseService
    {
        public static ServiceCollection Services { get; set; }
        public static ConfigService ConfigService { get; private set; }

        public static void Initialize(ServiceCollection serviceCollection)
        {
            Services = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            ConfigService = serviceCollection.GetService<ConfigService>();
        }

        // Add individual service variables for specific services.
        public static LogService LogService => Services.GetService<LogService>();
        public static StateStorageService StateStorage => Services.GetService<StateStorageService>();
        protected static AssetLoaderService AssetLoader => Services.GetService<AssetLoaderService>();
        protected static MessengerService Messenger => Services.GetService<MessengerService>();
    }
}