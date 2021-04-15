using CG.Business.Repositories.Options;
using System;
using System.ComponentModel.DataAnnotations;

namespace CG.Secrets.Azure.Repositories.Options
{
    /// <summary>
    /// This class represents configuration options for the <see cref="SecretRepository"/>
    /// class.
    /// </summary>
    public class SecretRepositoryOptions : RepositoryOptions
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the name of the associated Azure key vault.
        /// </summary>
        [Required]
        public string KeyVaultName { get; set; }

        /// <summary>
        /// This property contains the name of a strategy for logging into Azure.
        /// </summary>
        [Required]
        public string LoginType { get; set; }

        #endregion
    }
}
