using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;


namespace InstagramStorySpamPoster
{
    internal class Program
    {
        private static UserSessionData? user;
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
            var _instaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .Build();
            if (!_instaApi.IsUserAuthenticated)
            {
                Console.WriteLine($"Logging In as {user.UserName}");
                var logInResult = await _instaApi.LoginAsync();
                if (!logInResult.Succeeded)
                {
                    Console.WriteLine($"Unable to login: {logInResult.Info.Message}");
                }
                Console.WriteLine("Getting full name..");
                var name = _instaApi.GetLoggedUser().LoggedInUser.FullName;
                Console.WriteLine(name);
            }
            
            var image = new InstaImage { Uri = @"c:\Users\m-y-6\Desktop\instagram\story.jpg" };

            var storyOptions = new InstaStoryUploadOptions();
            storyOptions.Mentions.Add(new InstaStoryMentionUpload { X=0.5,Y=0.5,Z=0,Width=0.79722,Height=0.2196,Rotation=0,Username= "mysweetydevil" ,});
            
            var result = await _instaApi.StoryProcessor.UploadStoryPhotoAsync(image,"Cute Cat",storyOptions);
            Console.WriteLine(result.Succeeded
                ? $"Story created: {result.Value.Media.Pk}"
                : $"Unable to upload photo story: {result.Info.Message}");
            
        }

    }
}
    
    