using Azure.Core;
using CG.Options;
using System;

namespace CG.Secrets.Azure.Repositories.Options
{
    /// <summary>
    /// This class represents configuration options for an Azure login.
    /// </summary>
    public abstract class LoginOptions : OptionsBase
    {
        /// <summary>
        /// This method is called to create Azure login credentials.
        /// </summary>
        /// <returns>Azure login credentials.</returns>
        public abstract TokenCredential CreateCredentials();
    }
}
