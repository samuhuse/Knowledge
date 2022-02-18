using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BehavioralFeatures.AsyncStreams
{
    // IAsyncEnumerable<T> is pull-based

    public class IASyncEnumerables
    {
        #region Model
        public class User
        {
            public string login { get; set; }
            public int id { get; set; }
            public string node_id { get; set; }
            public string avatar_url { get; set; }
            public string gravatar_id { get; set; }
            public string url { get; set; }
            public string html_url { get; set; }
            public string followers_url { get; set; }
            public string following_url { get; set; }
            public string gists_url { get; set; }
            public string starred_url { get; set; }
            public string subscriptions_url { get; set; }
            public string organizations_url { get; set; }
            public string repos_url { get; set; }
            public string events_url { get; set; }
            public string received_events_url { get; set; }
            public string type { get; set; }
            public bool site_admin { get; set; }
        }

        public class Reactions
        {
            public string url { get; set; }
            public int total_count { get; set; }

            [JsonPropertyName("+1")]
            public int _One { get; set; }

            [JsonPropertyName("-1")]
            public int One { get; set; }
            public int laugh { get; set; }
            public int hooray { get; set; }
            public int confused { get; set; }
            public int heart { get; set; }
            public int rocket { get; set; }
            public int eyes { get; set; }
        }

        public class GitHubIssue
        {
            public string url { get; set; }
            public string repository_url { get; set; }
            public string labels_url { get; set; }
            public string comments_url { get; set; }
            public string events_url { get; set; }
            public string html_url { get; set; }
            public int id { get; set; }
            public string node_id { get; set; }
            public int number { get; set; }
            public string title { get; set; }
            public User user { get; set; }
            public List<object> labels { get; set; }
            public string state { get; set; }
            public bool locked { get; set; }
            public object assignee { get; set; }
            public List<object> assignees { get; set; }
            public object milestone { get; set; }
            public int comments { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public object closed_at { get; set; }
            public string author_association { get; set; }
            public object active_lock_reason { get; set; }
            public string body { get; set; }
            public Reactions reactions { get; set; }
            public string timeline_url { get; set; }
            public object performed_via_github_app { get; set; }
        }
        #endregion

        public static int MaxPages { get; private set; } = 10;
        private static HttpClient httpClient = new HttpClient();

        public static async IAsyncEnumerable<GitHubIssue> GetIssues([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var url = $"https://api.github.com/repos/ramtinak/InstagramApiSharp/issues?page=";
            var page = 0;

            while(page <= MaxPages && url != null)
            {
                var response = await httpClient.GetAsync(url + page, cancellationToken);
                var json = await response.Content.ReadAsStringAsync();
                var issues = JsonSerializer.Deserialize<List<GitHubIssue>>(json);

                Console.WriteLine($"Retrived data page {page}");

                foreach(var issue in issues)
                {
                    yield return issue;
                }

                page++;
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        [Test]
        public async Task Try()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            await foreach(var issue in GetIssues().WithCancellation(cancellationTokenSource.Token))                
            {
                Console.Write(issue.ToString());
            }
        }
    }
}
