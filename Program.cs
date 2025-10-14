using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using GithubContracts;

using HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://api.github.com/users/")
};

httpClient.DefaultRequestHeaders.Add("User-Agent", "GithubEvents-CLI");
httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");

bool running = true;

while (running)
{
    Console.Clear();

    try
    {
        string? username = args.FirstOrDefault()?.Trim();

        while (string.IsNullOrWhiteSpace(username))
        {
            Console.Write("Username: ");
            username = Console.ReadLine()!.Trim();
        }

        HttpResponseMessage response = await httpClient.GetAsync($"{username}/events");

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new ArgumentException($"No user found with the username '{username}'.");

        if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException("Something went wrong processing your request.");

        GitHubEvent[]? userEvents = await response.Content.ReadFromJsonAsync<GitHubEvent[]>();

        if (userEvents == null || userEvents.Length == 0)
            throw new NullReferenceException($"No events by the user {username} were found.");

        Console.WriteLine();
        foreach (GitHubEvent uEvent in userEvents)
        {
            string outputMessage = string.Empty;

            switch (uEvent.Type)
            {
                case "CommitCommentEvent":
                    outputMessage = $"Commented on a commit of {uEvent.Repo.Name}";
                    break;

                case "CreateEvent":
                    var createPayload = uEvent.Payload.Deserialize<CreateDeleteEventPayload>();
                    if (createPayload == null)
                        throw new NullReferenceException("Event payload cannot be null.");

                    outputMessage = $"Created a {createPayload.RefType}";
                    if (createPayload.RefType != "repository")
                        outputMessage += $" in {uEvent.Repo.Name}";
                    break;

                case "DeleteEvent":
                    var deletePayload = uEvent.Payload.Deserialize<CreateDeleteEventPayload>();
                    if (deletePayload == null)
                        throw new NullReferenceException("Event payload cannot be null.");

                    outputMessage = $"Deleted a {deletePayload.RefType} in {uEvent.Repo.Name}";
                    break;

                case "ForkEvent":
                    outputMessage = $"Forked a branch in {uEvent.Repo.Name}";
                    break;

                case "IssueEvent":
                case "IssueCommentEvent":
                    var issuePayload = uEvent.Payload.Deserialize<IssueEventPayload>();
                    if (issuePayload == null)
                        throw new NullReferenceException("Event payload cannot be null.");

                    outputMessage = $"{char.ToUpper(issuePayload.Action[0])}{issuePayload.Action[1..]} an issue in {uEvent.Repo.Name}";
                    break;

                case "PublicEvent":
                    outputMessage = $"Made {uEvent.Repo.Name} public";
                    break;

                case "PullRequestEvent":
                    var pullPayload = uEvent.Payload.Deserialize<PullRequestEventPayload>();
                    if (pullPayload == null)
                        throw new NullReferenceException("Event payload cannot be null.");

                    outputMessage = $"{char.ToUpper(pullPayload.Action[0])}{pullPayload.Action[1..]} a pull request in {uEvent.Repo.Name}";
                    break;

                case "PullRequestReviewEvent":
                    var pullReviewPayload = uEvent.Payload.Deserialize<PullRequestEventPayload>();
                    if (pullReviewPayload == null)
                        throw new NullReferenceException("Event payload cannot be null.");

                    outputMessage = $"{char.ToUpper(pullReviewPayload.Action[0])}{pullReviewPayload.Action[1..]} a review in a pull request in {uEvent.Repo.Name}";
                    break;

                case "PullRequestReviewCommentEvent":
                    var pullCommentPayload = uEvent.Payload.Deserialize<PullRequestEventPayload>();
                    if (pullCommentPayload == null)
                        throw new NullReferenceException("Event payload cannot be null.");

                    outputMessage = $"{char.ToUpper(pullCommentPayload.Action[0])}{pullCommentPayload.Action[1..]} a comment in a pull request review in {uEvent.Repo.Name}";
                    break;

                case "PullRequestReviewThreadEvent":
                    var pullThreadPayload = uEvent.Payload.Deserialize<PullRequestEventPayload>();
                    if (pullThreadPayload == null)
                        throw new NullReferenceException("Event payload cannot be null.");

                    outputMessage = $"Marked a review thread as {pullThreadPayload.Action} in {uEvent.Repo.Name}";
                    break;

                case "PushEvent":
                    var pushPayload = uEvent.Payload.Deserialize<PushEventPayload>();
                    if (pushPayload == null)
                        throw new NullReferenceException("Event payload cannot be null.");

                    outputMessage = $"Pushed {pushPayload.Size} commits to {uEvent.Repo.Name}";
                    break;

                case "WatchEvent":
                    var watchPayload = uEvent.Payload.Deserialize<WatchEventPayload>();
                    if (watchPayload == null)
                        throw new NullReferenceException("Event payload cannot be null.");

                    outputMessage = $"Starred {uEvent.Repo.Name}";
                    break;

                default:
                    outputMessage = "Unsupported event";
                    break;
            }

            Console.WriteLine(outputMessage);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);

        Console.WriteLine("\nPress Enter to try again...");
        Console.ReadLine();

        args = [];
        continue;
    }

    Console.Write("\nPress enter to exit.");
    Console.ReadLine();
    running = false;
}