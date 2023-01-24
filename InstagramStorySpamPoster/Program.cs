using System.Net;

namespace InstagramStorySpamPoster;

internal class Program
{
    static void Main()
    {
        var username = "testing.instaapi";
        var password = "Aabb1998";
        var storyPath = @".\story.jpg";
        var mentions = new string[] { "mysweetydevil" , "julichka098" };
        var url = "https://www.google.com";
        var caption = "Cute Cat22";

        var proxyHost = "socks5";
        var proxyPort = "63109";
        var proxyUserName = "pKFuebaA";
        var proxyPassword = "aL31SMvg";
        var proxyHandler = new HttpClientHandler()
        {
            Proxy = new WebProxy()
            {
                Address = new Uri($"{proxyHost}:{proxyPort}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = true,
                //Proxy file format: host:port:username:password
                Credentials = new NetworkCredential(userName: proxyUserName, password: proxyPassword)
            },
            UseProxy = true
        };

        var account = InstaSpam.Login(username, password, proxyHandler).Result;
        account?.Post(storyPath, mentions, url, caption);

        Console.WriteLine("Done");
        Console.ReadKey();
    }
}

