using System.Collections.Generic;
using Aelfcraeft.Infrastructure.Services;

namespace Aelfcraeft.Infrastructure.Management
{
    public class ServiceCollection
    {
        #region Public API

        public void AddService(BaseService service)
        {
            lock (_lock)
            {
                // Prevent duplicates by ensuring the service's type doesn't already exist
                if (!_services.Exists(s => s.GetType() == service.GetType()))
                {
                    _services.Add(service);
                }
            }
        }

        public T GetService<T>() where T : class
        {
            lock (_lock)
            {
                foreach (var service in _services)
                {
                    if (service is T typedService)
                    {
                        return typedService;
                    }
                }
                return null;
            }
        }

        #endregion

        #region Private Implementation

        private readonly List<BaseService> _services = new List<BaseService>();
        private readonly object _lock = new object();

        // Add any private methods or fields here if needed in the future.

        #endregion
    }
}