namespace Octokit.GraphQL.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Autogenerated input type of RemoveBusinessBillingManager
    /// </summary>
    public class RemoveBusinessBillingManagerInput
    {
        /// <summary>
        /// The Business ID to update.
        /// </summary>
        public ID BusinessId { get; set; }

        /// <summary>
        /// The login of the user to add as a billing manager.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// A unique identifier for the client performing the mutation.
        /// </summary>
        public string ClientMutationId { get; set; }
    }
}