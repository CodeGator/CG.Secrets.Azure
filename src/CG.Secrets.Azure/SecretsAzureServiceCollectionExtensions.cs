using Azure.Security.KeyVault.Secrets;
using CG;
using CG.Secrets.Azure.Repositories;
using CG.Secrets.Azure.Repositories.Options;
using CG.Secrets.Repositories;
using CG.Validations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IServiceCollection"/>
    /// type, for registering types from the <see cref="CG.Secrets.Azure"/> library.
    /// </summary>
    public static partial class SecretsAzureServiceCollectionExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method adds Azure KeyVault repositories for the CG.Secrets library.
        /// </summary>
        /// <param name="serviceCollection">The service collection to use for
        /// the operation.</param>
        /// <param name="configuration">The configuration to use for the operation.</param>
        /// <param name="serviceLifetime">The service lifetime to use for the operation.</param>
        /// <returns>The value of the <paramref name="serviceCollection"/> parameter,
        /// for chaining calls together.</returns>
        public static IServiceCollection AddAzureRepositories(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(configuration, nameof(configuration));

            // Register the repository options.
            serviceCollection.ConfigureOptions<SecretRepositoryOptions>(
                configuration,
                out var repositoryOptions
                );

            // Register the login options.
            LoginOptions loginOptions = null;
            if ("Default" == repositoryOptions.LoginType)
            {
                // Register the login options.
                serviceCollection.ConfigureOptions<LoginOptions, DefaultAzureLoginOptions>(
                    configuration.GetSection("Default"),
                    out loginOptions
                    );
            }
            else if ("Environment" == repositoryOptions.LoginType)
            {
                // Register the login options.
                serviceCollection.ConfigureOptions<LoginOptions, EnvironmentLoginOptions>(
                    configuration.GetSection("Environment"),
                    out loginOptions
                    );
            }
            else if ("ChainedToken" == repositoryOptions.LoginType)
            {
                // Register the login options.
                serviceCollection.ConfigureOptions<LoginOptions, ChainedTokenLoginOptions>(
                    configuration.GetSection("ChainedToken"),
                    out loginOptions
                    );
            }
            else if ("VisualStudio" == repositoryOptions.LoginType)
            {
                // Register the login options.
                serviceCollection.ConfigureOptions<LoginOptions, VisualStudioLoginOptions>(
                    configuration.GetSection("VisualStudio"),
                    out loginOptions
                    );
            }
            else if ("VisualStudioCode" == repositoryOptions.LoginType)
            {
                // Register the login options.
                serviceCollection.ConfigureOptions<LoginOptions, VisualStudioCodeLoginOptions>(
                    configuration.GetSection("VisualStudioCode"),
                    out loginOptions
                    );
            }
            else if ("ClientSecret" == repositoryOptions.LoginType)
            {
                // Register the login options.
                serviceCollection.ConfigureOptions<LoginOptions, ClientSecretLoginOptions>(
                    configuration.GetSection("ClientSecret"),
                    out loginOptions
                    );
            }
            else
            {
                // Panic!!
                throw new ArgumentException(
                    message: $"Unknown login type detected: '{repositoryOptions.LoginType}'"
                    ).SetDateTime()
                     .SetOriginator(nameof(SecretsAzureServiceCollectionExtensions));
            }
            
            // Register the azure client.
            serviceCollection.Add<SecretClient>(serviceProvider =>
            {
                // Get the repository options.
                var repositoryOptions = serviceProvider.GetRequiredService<IOptions<SecretRepositoryOptions>>();

                // Create the Azure credentials.
                var credential = loginOptions?.CreateCredentials();

                // Create a client instance.
                var client = new SecretClient(
                    new Uri($"https://{repositoryOptions.Value.KeyVaultName}.vault.azure.net/"),
                    credential
                );

                // Return the results.
                return client;
            },
            serviceLifetime
            );

            // Register the repository.
            serviceCollection.Add<ISecretRepository, SecretRepository>(serviceLifetime);

            // Return the service collection.
            return serviceCollection;
        }

        #endregion
    }
}
