namespace Octokit.GraphQL.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Autogenerated input type of AddProjectCard
    /// </summary>
    public class AddProjectCardInput
    {
        public string ClientMutationId { get; set; }

        public string ProjectColumnId { get; set; }

        public string ContentId { get; set; }

        public string Note { get; set; }
    }
}