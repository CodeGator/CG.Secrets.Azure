using Azure.Core;
using Azure.Identity;
using System.ComponentModel.DataAnnotations;

namespace CG.Secrets.Azure.Repositories.Options
{
    /// <summary>
    /// This class represents options for the default Azure approach to 
    /// connecting to Azure.
    /// </summary>
    public class DefaultAzureLoginOptions : LoginOptions
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a manage identity client identifier.
        /// </summary>
        public string ClientId { get; set; }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public override TokenCredential CreateCredentials()
        {
            if (string.IsNullOrEmpty(ClientId))
            {
                return new DefaultAzureCredential();
            }
            else
            {
                return new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions()
                    {
                        ManagedIdentityClientId = ClientId
                    });
            }
        }

        #endregion
    }
}
