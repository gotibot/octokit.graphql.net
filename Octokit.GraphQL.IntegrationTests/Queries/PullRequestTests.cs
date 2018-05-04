using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit.GraphQL.IntegrationTests.Utilities;
using Xunit;
using static Octokit.GraphQL.Variable;

namespace Octokit.GraphQL.IntegrationTests.Queries
{
    public class PullRequestTests : IntegrationTestBase
    {
        [IntegrationTest]
        public void Should_Query_Commits()
        {
            var query = new GraphQL.Query().Repository("octokit", "octokit.net").PullRequest(1).Commits(3).Nodes
                .Select(pullRequestCommit => new
                {
                    pullRequestCommit.Commit.Id,
                    pullRequestCommit.Commit.Message,
                    pullRequestCommit.Commit.Author.Name
                });

            var results = Enumerable.ToArray(Connection.Run(query).Result);

            Assert.Equal(1, results.Length);
            Assert.Equal("MDY6Q29tbWl0NzUyODY3OTpkYWZhYjhhZjA0ODM5NDU1ODM4Y2QzZmRlMTFkMTM5MTc0MTYyZmFh", results[0].Id.Value);
            var expectedMessage = "Adding README, CONTRIBUTING, LICENSE\n\nWe plan to release this code under the MIT license so might as well get\nthe right things in place early.";
            Assert.Equal(expectedMessage, results[0].Message);
            Assert.Equal("Haacked", results[0].Name);
        }

        [IntegrationTest]
        public async Task Should_Query_Title_With_Vars()
        {
            var query = new Query()
                .Repository(Var("owner"), Var("name"))
                .PullRequest(Var("number"))
                .Select(x => x.Title)
                .Compile();

            var vars = new Dictionary<string, object>
            {
                { "owner", "octokit" },
                { "name", "octokit.net" },
                { "number", 1 },
            };

            var result = await Connection.Run(query, vars);

            Assert.Equal("Adding README, CONTRIBUTING, LICENSE", result);
        }

        [IntegrationTest]
        public async Task Can_Use_Conditional_In_BaseRef_Query()
        {
            var query = new Query()
                .Repository("octokit", "octokit.net")
                .PullRequest(1)
                .Select(pr => pr.BaseRef != null ? pr.BaseRef.Name : null);

            var result = await Connection.Run(query);

            Assert.Equal("master", result);
        }
    }
}