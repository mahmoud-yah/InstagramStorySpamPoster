using InstagramApiSharp.API.Builder;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using System.Net.Http;
using System.Net;

namespace InstagramStorySpamPoster;

internal static class InstaSpam
{
    private static readonly Random random = new();
    private static double GetRand { get => Math.Round(random.NextDouble() * (0.9 - 0.1) + 0.1, 1); }

    internal static async Task<IInstaApi?> Login(string username, string password, HttpClientHandler httpClientHandler)
    {
        var userSessionData = new UserSessionData
        {
            UserName = username,
            Password = password
        };

        // api build
        var instaApi = InstaApiBuilder.CreateBuilder()
            .SetUser(userSessionData)
            .UseHttpClientHandler(httpClientHandler)
            .UseLogger(new DebugLogger(LogLevel.Exceptions))
            .Build();

        // login
        Console.WriteLine($"Logging In as {userSessionData.UserName}");
        var logInResult = await instaApi.LoginAsync();
        if (!logInResult.Succeeded)
        {
            Console.WriteLine($"Error: Unable to login: {logInResult.Info.Message}");
            return null;
        }

        return instaApi;
    }

    internal static async void Post(this IInstaApi instaApi, string storyPath, string[] mentions, string url, string caption)
    {
        var image = new InstaImage { Uri = storyPath };

        var storyOptions = new InstaStoryUploadOptions();
        foreach (var mention in mentions)
            storyOptions.Mentions.Add(new InstaStoryMentionUpload
            {
                X = GetRand,
                Y = GetRand,
                Z = 0,
                Width = GetRand,
                Height = GetRand,
                Rotation = 0,
                Username = mention,
            });

        var result =
            await instaApi.StoryProcessor.UploadStoryPhotoWithUrlAsync(
                image,
                caption,
                new Uri(url),
                storyOptions);

        Console.WriteLine(result.Succeeded
            ? $"Story created: {result.Value.Media.Pk}"
            : $"Unable to upload photo story: {result.Info.Message}");
    }
}
