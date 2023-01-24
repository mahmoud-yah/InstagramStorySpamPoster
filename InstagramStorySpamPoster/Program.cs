using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using System.Net;

namespace InstagramStorySpamPoster
{
    internal class Program
    {
        private static UserSessionData? user;
        private static String proxyHost = "http://194.107.92.150";
        private static String proxyPort = "63109";
        private static String proxyUserName = "pKFuebaA";
        private static String proxyPassword = "aL31SMvg";
        static void Main(string[] args)
        {
            logInWithEmailAndPassword();
            Console.ReadKey();
        }

        static async void logInWithEmailAndPassword()
        {
            user = new UserSessionData
            {
                UserName = "testing.instaapi",
                Password = "Aabb1998"
            };
            var proxy = new WebProxy()
            {
                Address = new Uri($"{proxyHost}:{proxyPort}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = true,
                //Credentials = new NetworkCredential(userName:"pKFuebaA",password:"aL31SMvg")
            };
            var httpClientHandler = new HttpClientHandler() { Proxy=proxy,};
            var _instaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseHttpClientHandler(httpClientHandler)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .Build();
            if (!_instaApi.IsUserAuthenticated)
            {
                Console.WriteLine($"Logging In as {user.UserName}");
                var logInResult = await _instaApi.LoginAsync();
                if (!logInResult.Succeeded)
                {
                    Console.WriteLine($"Unable to login: {logInResult.Info.Message}");
                    return;
                }
                Console.WriteLine("Getting full name..");
                var name = _instaApi.GetLoggedUser().LoggedInUser.FullName;
                Console.WriteLine(name);
            }
            
            var image = new InstaImage { Uri = @"c:\Users\m-y-6\Desktop\instagram\story.jpg" };

            var storyOptions = new InstaStoryUploadOptions();
            
            storyOptions.Mentions.Add(new InstaStoryMentionUpload { X=0.4,Y=0.4,Z=0,Width= 0.7972222, Height= 0.21962096, Rotation=0,Username= "mysweetydevil" ,});
            storyOptions.Mentions.Add(new InstaStoryMentionUpload { X = 0.6, Y = 0.6, Z = 0, Width = 0.7972222, Height = 0.21962096, Rotation = 0, Username = "julichka098", });
            //InstaStorySliderUpload()

            var result = await _instaApi.StoryProcessor.UploadStoryPhotoWithUrlAsync(image,"Cute Cat",new Uri("https://www.google.com"),storyOptions);
            Console.WriteLine(result.Succeeded
                ? $"Story created: {result.Value.Media.Pk}"
                : $"Unable to upload photo story: {result.Info.Message}");
            
        }

    }
}
    
    