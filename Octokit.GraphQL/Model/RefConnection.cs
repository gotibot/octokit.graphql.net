namespace Octokit.GraphQL.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Octokit.GraphQL.Core;
    using Octokit.GraphQL.Core.Builders;

    /// <summary>
    /// The connection type for Ref.
    /// </summary>
    public class RefConnection : QueryableValue<RefConnection>
    {
        public RefConnection(IQueryProvider provider, Expression expression) : base(provider, expression)
        {
        }

        /// <summary>
        /// A list of edges.
        /// </summary>
        public IQueryableList<RefEdge> Edges => this.CreateProperty(x => x.Edges);

        /// <summary>
        /// A list of nodes.
        /// </summary>
        public IQueryableList<Ref> Nodes => this.CreateProperty(x => x.Nodes);

        /// <summary>
        /// Information to aid in pagination.
        /// </summary>
        public PageInfo PageInfo => this.CreateProperty(x => x.PageInfo, Octokit.GraphQL.Model.PageInfo.Create);

        /// <summary>
        /// Identifies the total count of items in the connection.
        /// </summary>
        public int TotalCount { get; }

        internal static RefConnection Create(IQueryProvider provider, Expression expression)
        {
            return new RefConnection(provider, expression);
        }
    }
}