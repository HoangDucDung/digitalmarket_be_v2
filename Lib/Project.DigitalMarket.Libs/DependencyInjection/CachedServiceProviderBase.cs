using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Project.DigitalMarket.Libs.DependencyInjection
{
    public class CachedServiceProviderBase : ICachedServiceProviderBase
    {
        protected IServiceProvider _serviceProvider { get; }
        protected ConcurrentDictionary<ServiceIdentifier, Lazy<object?>> CachedServices { get; }

        public CachedServiceProviderBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            CachedServices = new ConcurrentDictionary<ServiceIdentifier, Lazy<object?>>();
            CachedServices.TryAdd(new ServiceIdentifier(typeof(IServiceProvider)), new Lazy<object?>(() => _serviceProvider));
        }

        #region get service
        public virtual object? GetService(Type serviceType)
        {
            return CachedServices.GetOrAdd(
                new ServiceIdentifier(serviceType),
                _ => new Lazy<object?>(() => _serviceProvider.GetService(serviceType))
            ).Value;
        }

        public T GetService<T>(T defaultValue)
        {
            return (T)GetService(typeof(T), defaultValue!);
        }

        public object GetService(Type serviceType, object defaultValue)
        {
            return GetService(serviceType) ?? defaultValue;
        }

        #endregion

        #region get require service

        public T GetRequiredService<T>()
        {
            return (T)GetRequiredService(typeof(T))!;
        }

        public object GetRequiredService(Type serviceType, object defaultValue)
        {
            return GetRequiredService(serviceType) ?? defaultValue;
        }

        public virtual object GetRequiredService(Type serviceType)
        {
            return CachedServices.GetOrAdd(
                new ServiceIdentifier(serviceType),
                _ => new Lazy<object?>(() => _serviceProvider.GetRequiredService(serviceType))
            ).Value!;
        }
        #endregion

        public T GetService<T>(Func<IServiceProvider, object> factory)
        {
            return (T)GetService(typeof(T), factory);
        }

        public object GetService(Type serviceType, Func<IServiceProvider, object> factory)
        {
            return CachedServices.GetOrAdd(
                new ServiceIdentifier(serviceType),
                _ => new Lazy<object?>(() => factory(_serviceProvider))
            ).Value!;
        }

        public object? GetKeyedService(Type serviceType, object? serviceKey)
        {
            return CachedServices.GetOrAdd(
                new ServiceIdentifier(serviceKey, serviceType),
                _ => new Lazy<object?>(() => _serviceProvider.GetKeyedServices(serviceType, serviceKey))
            ).Value;
        }

        public object GetRequiredKeyedService(Type serviceType, object? serviceKey = null)
        {
            return CachedServices.GetOrAdd(
                new ServiceIdentifier(serviceKey, serviceType),
                _ => new Lazy<object?>(() => _serviceProvider.GetRequiredKeyedService(serviceType, serviceKey))
            ).Value!;
        }


    }
}
