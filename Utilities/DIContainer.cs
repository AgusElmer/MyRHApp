using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRHApp.Utilities
{
    public static class DIContainer
    {
        private static readonly Dictionary<Type, Func<object>> _services = new Dictionary<Type, Func<object>>();
        private static readonly HashSet<Type> _currentlyResolving = new HashSet<Type>();

        public static void Register<TService, TImplementation>() where TImplementation : TService
        {
            _services[typeof(TService)] = () => ResolveImplementation(typeof(TImplementation));
        }

        public static void RegisterSingleton<TService>(TService instance)
        {
            _services[typeof(TService)] = () => instance;
        }

        public static TService Resolve<TService>()
        {
            return (TService)ResolveImplementation(typeof(TService));
        }

        private static object ResolveImplementation(Type serviceType)
        {
            if (_currentlyResolving.Contains(serviceType))
            {
                throw new InvalidOperationException($"Circular dependency detected for type {serviceType.FullName}");
            }

            try
            {
                _currentlyResolving.Add(serviceType);

                if (_services.TryGetValue(serviceType, out var implementationFactory))
                {
                    return implementationFactory();
                }

                var implementationType = serviceType;
                if (serviceType.IsInterface || serviceType.IsAbstract)
                {
                    implementationType = _services.Keys.FirstOrDefault(k => serviceType.IsAssignableFrom(k));
                    if (implementationType == null)
                    {
                        throw new InvalidOperationException($"No implementation registered for type {serviceType.FullName}");
                    }
                }

                var constructors = implementationType.GetConstructors();
                if (constructors.Length == 0)
                {
                    throw new InvalidOperationException($"No public constructors found for type {implementationType.FullName}");
                }

                var constructor = constructors.First();
                var parameters = constructor.GetParameters();
                if (parameters.Length == 0)
                {
                    return Activator.CreateInstance(implementationType);
                }

                var parameterImplementations = parameters.Select(p => ResolveImplementation(p.ParameterType)).ToArray();
                return Activator.CreateInstance(implementationType, parameterImplementations);
            }
            finally
            {
                _currentlyResolving.Remove(serviceType);
            }
        }
    }
}
