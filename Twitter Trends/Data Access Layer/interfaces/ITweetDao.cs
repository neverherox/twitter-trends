using Business_Layer;
using System.Collections.Generic;

namespace Data_Access_Layer.interfaces
{
    public interface ITweetDao
    {
        List<Tweet> GetTweets();
        Tweet GetTweet(string text);
        List<Tweet> GetTweetsSentiments(List<Tweet> tweets);
        double GetTweetSentiment(Tweet tweet);
        void ParseTweets(string path);
    }
}
