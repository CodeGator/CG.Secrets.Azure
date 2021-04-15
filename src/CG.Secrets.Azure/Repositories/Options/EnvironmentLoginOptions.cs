using Azure.Core;
using Azure.Identity;
using CG.Options;
using System;
using System.ComponentModel.DataAnnotations;

namespace CG.Secrets.Azure.Repositories.Options
{
    /// <summary>
    /// This class represents options for the environment approach to 
    /// connecting to Azure.
    /// </summary>
    public class EnvironmentLoginOptions : LoginOptions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public override TokenCredential CreateCredentials()
        {
            return new EnvironmentCredential();
        }

        #endregion
    }
}
