using Azure.Core;
using Azure.Identity;
using CG.Options;
using System;
using System.ComponentModel.DataAnnotations;

namespace CG.Secrets.Azure.Repositories.Options
{
    /// <summary>
    /// This class represents options for the chained token approach to 
    /// connecting to Azure.
    /// </summary>
    public class ChainedTokenLoginOptions : LoginOptions
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains an Azure tenant identifier.
        /// </summary>
        [Required]
        public string TenantId { get; set; }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public override TokenCredential CreateCredentials()
        {
            return new ChainedTokenCredential(
                new ManagedIdentityCredential(),
                new AzureCliCredential()
                );
        }

        #endregion
    }
}
