namespace Octokit.GraphQL.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Octokit.GraphQL.Model;
    using Octokit.GraphQL.Core;
    using Octokit.GraphQL.Core.Builders;

    /// <summary>
    /// Entities that can be deleted.
    /// </summary>
    public interface IDeletable : IQueryableValue<IDeletable>
    {
        /// <summary>
        /// Check if the current viewer can delete this object.
        /// </summary>
        bool ViewerCanDelete { get; }
    }
}

namespace Octokit.GraphQL.Model.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Octokit.GraphQL.Core;
    using Octokit.GraphQL.Core.Builders;

    internal class StubIDeletable : QueryableValue<StubIDeletable>, IDeletable
    {
        public StubIDeletable(IQueryProvider provider, Expression expression) : base(provider, expression)
        {
        }

        public bool ViewerCanDelete { get; }

        internal static StubIDeletable Create(IQueryProvider provider, Expression expression)
        {
            return new StubIDeletable(provider, expression);
        }
    }
}