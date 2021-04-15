using Azure.Security.KeyVault.Secrets;
using CG.Business.Repositories;
using CG.Secrets.Models;
using CG.Secrets.Repositories;
using CG.Validations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CG.Secrets.Azure.Repositories
{
    /// <summary>
    /// This class is an Azure implementation of the <see cref="ISecretRepository"/>
    /// interface.
    /// </summary>
    public class SecretRepository : RepositoryBase, ISecretRepository
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a reference to an Azure secret client.
        /// </summary>
        protected SecretClient SecretClient { get; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="SecretRepository"/>
        /// class.
        /// </summary>
        /// <param name="secretClient">Thd Azure secret client to use with the 
        /// repository.</param>
        public SecretRepository(
            SecretClient secretClient
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(secretClient, nameof(secretClient));

            // Save the references.
            SecretClient = secretClient;
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public virtual async Task<Secret> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                // Validate the parameters before attempting to use them.
                Guard.Instance().ThrowIfNullOrEmpty(name, nameof(name));

                // Defer to the client.
                var azureSecret = await SecretClient.GetSecretAsync(
                    name,
                    null,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Parse out the key.
                var key = azureSecret.Value.Id.PathAndQuery.Substring(
                    $"/secrets/{name}/".Length
                    );

                // Convert the results to our model.
                var secret = new Secret()
                {
                    Key = key,
                    Value = azureSecret.Value.Value,
                    Name = azureSecret.Value.Name
                };

                // Return the results.
                return secret;
            }
            catch (Exception ex)
            {
                // Provide better context for the error.
                throw new RepositoryException(
                    message: $"Failed to query the value of a secret, by name!",
                    innerException: ex
                    ).SetCallerInfo()
                     .SetOriginator(nameof(SecretRepository))
                     .SetDateTime();
            }
        }

        // *******************************************************************
        
        /// <inheritdoc/>
        public virtual async Task<Secret> SetByNameAsync(
            string name,
            string value,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                // Validate the parameters before attempting to use them.
                Guard.Instance().ThrowIfNullOrEmpty(name, nameof(name));

                // Defer to the client.
                var azureSecret = await SecretClient.SetSecretAsync(
                    name,
                    value,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Parse out the key.
                var key = azureSecret.Value.Id.PathAndQuery.Substring(
                    $"/secrets/{name}/".Length
                    );

                // Convert the results to our model.
                var secret = new Secret()
                {
                    Key = key,
                    Value = azureSecret.Value.Value,
                    Name = azureSecret.Value.Name
                };

                // Return the results.
                return secret;
            }
            catch (Exception ex)
            {
                // Provide better context for the error.
                throw new RepositoryException(
                    message: $"Failed to set the value for a secret, by name!",
                    innerException: ex
                    ).SetCallerInfo()
                     .SetOriginator(nameof(SecretRepository))
                     .SetDateTime();
            }
        }

        #endregion
    }
}
