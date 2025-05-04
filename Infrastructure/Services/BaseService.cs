using System;

namespace Survival.Infrastructure.Services
{
    public abstract class BaseService
    {
        public static ServiceCollection Services { get; set; }

        public static void Initialize(ServiceCollection serviceCollection)
        {
            Services = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
        }

        // Add individual service variables for specific services.
        protected static LogService LogService => Services.GetService<LogService>();
        public static StateStorageService StateStorage => Services.GetService<StateStorageService>();
        protected static AssetLoaderService AssetLoader => Services.GetService<AssetLoaderService>();
        protected static MessengerService Messenger => Services.GetService<MessengerService>();
    }
}