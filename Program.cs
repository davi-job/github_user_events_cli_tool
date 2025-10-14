using System.Net;
using System.Net.Http.Json;
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

        // Todo: Output formatting
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);

        Console.WriteLine("\nPress Enter to try again...");
        Console.ReadLine();

        args = [];
        continue;
    }

    Console.ReadLine();
    running = false;
}