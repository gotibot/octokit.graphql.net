﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Octokit.GraphQL.Core.Deserializers;

namespace Octokit.GraphQL.Core
{
    public class PagedQuery<TResult> : ICompiledQuery<TResult>
    {
        public PagedQuery(
            CompiledQuery<TResult> masterQuery,
            IReadOnlyList<Subquery> subqueries)
        {
            MasterQuery = masterQuery;
            Subqueries = subqueries;
        }

        public CompiledQuery<TResult> MasterQuery { get; }
        public IReadOnlyList<Subquery> Subqueries { get; }

        public IQueryRunner<TResult> Start(IConnection connection, Dictionary<string, object> variables)
        {
            return new Runner(this, connection, variables);
        }

        public string ToString(int indentation)
        {
            return MasterQuery.ToString(indentation);
        }

        IQueryRunner ICompiledQuery.Start(IConnection connection, Dictionary<string, object> variables)
        {
            return Start(connection, variables);
        }

        class Runner : IQueryRunner<TResult>
        {
            readonly PagedQuery<TResult> parent;
            readonly IConnection connection;
            readonly Dictionary<string, object> variables;
            readonly ResponseDeserializer deserializer = new ResponseDeserializer();
            Dictionary<Subquery, IQueryRunner> subqueries;

            public Runner(
                PagedQuery<TResult> parent,
                IConnection connection,
                Dictionary<string, object> variables)
            {
                this.parent = parent;
                this.connection = connection;
                this.variables = variables;
            }

            public TResult Result { get; private set; }

            public async Task<bool> RunPage()
            {
                if (subqueries == null)
                {
                    var master = parent.MasterQuery;
                    var data = await connection.Run(master.GetPayload(variables));
                    var json = JObject.Parse(data);

                    Result = deserializer.Deserialize(master.CompiledExpression, json);
                    subqueries = new Dictionary<Subquery, IQueryRunner>();

                    foreach (var subquery in parent.Subqueries)
                    {
                        var pageInfo = subquery.ParentPageInfo.Compile()(json);

                        if ((bool)pageInfo["hasNextPage"] == true)
                        {
                            subqueries.Add(subquery, null);
                        }
                    }
                }
                else
                {
                    var item = subqueries.First();
                    var subquery = item.Key;
                    var runner = item.Value;

                    if (runner == null)
                    {
                        runner = subquery.Query.Start(connection, variables);
                        subqueries[subquery] = runner;
                        return await runner.RunPage();
                    }
                }

                return subqueries.Count > 0;
            }
        }
    }
}
