using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Reflection;
using System.IO;
using System;
using System.Linq;
using MyApplication.Services;

namespace MyApplication.API
{
    internal static class DependencyInjectionConfiguration
    {
        internal static void Configure(IServiceCollection services)
        {
            services.AddTransient<IBlogPostsDataService, BlogPostsDataService>().AsSelf();
        }
    }

    /// <summary>
    /// Extension methods for ServiceCollection.
    /// Credits to Maarten Balliauw
    /// https://blog.maartenballiauw.be/post/2018/10/19/registering-a-type-as-an-interface-and-as-self-with-asp.net-core-dependency-injection.html
    /// </summary>
    public static class ServiceCollectionHostedServiceExtensions
    {
        /// <summary>
        /// Register the last registration as its own type.
        /// </summary>
        /// <returns>The original <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</returns>
        public static IServiceCollection AsSelf(this IServiceCollection services)
        {
            var lastRegistration = services.LastOrDefault();
            if (lastRegistration != null)
            {
                var implementationType = GetImplementationType(lastRegistration);
                
                // When the last registration service type was already registered
                // as its implementation type, bail out.
                if (lastRegistration.ServiceType == implementationType)
                {
                    return services;
                }
                
                if (lastRegistration.ImplementationInstance != null)
                {
                    // Register "self" registration as the same instance
                    services.Add(new ServiceDescriptor(
                        implementationType, 
                        lastRegistration.ImplementationInstance));
                }
                else 
                {
                    // Remove last registration
                    services.Remove(lastRegistration);
                
                    // Register "self" registration first
                    if (lastRegistration.ImplementationFactory != null)
                    {
                        // Factory-based
                        services.Add(new ServiceDescriptor(
                            lastRegistration.ImplementationType,
                            lastRegistration.ImplementationFactory,
                            lastRegistration.Lifetime));
                    }
                    else
                    {
                        // Type-based
                        services.Add(new ServiceDescriptor(
                            lastRegistration.ImplementationType,
                            lastRegistration.ImplementationType, 
                            lastRegistration.Lifetime));
                    }

                    // Re-register last registration, proxying our specific registration
                    services.Add(new ServiceDescriptor(
                        lastRegistration.ServiceType,
                        provider => provider.GetService(implementationType), 
                        lastRegistration.Lifetime));
                }
            }

            return services;
        }
        
        private static Type GetImplementationType(ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationType != null)
            {
                return descriptor.ImplementationType;
            }

            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance.GetType();
            }
            
            if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory.GetType().GenericTypeArguments[1];
            }

            return null;
        }
    }
}