using Business_Layer;
using Data_Access_Layer.interfaces;
using System.Collections.Generic;
namespace Data_Access_Layer
{
    public class TweetDao: ITweetDao
    {
        private DB db = DB.GetInstance();

        public List<Tweet> GetTweets()
        {
            return db.tweets;
        }

        public Tweet GetTweet(string text)
        {
            for (int i = 0; i < db.tweets.Count; i++)
            {
                if (db.tweets[i].Text.Equals(text))
                {
                    return db.tweets[i];
                }
            }
            return null;
        }
        public List<Tweet> GetTweetsSentiments(List<Tweet>tweets)
        {
            foreach(Tweet tweet in tweets)
            {
                double weight = GetTweetSentiment(tweet);
                tweet.Weight = weight;
            }
            return tweets;
        }
        public double GetTweetSentiment(Tweet tweet)
        {
            double weight = 0;
            int count = 0;
            string[] words = tweet.Text.Split(new char[] { ',', '.', '!', ':', '?', ' ' });
            for (int i = 0; i < words.Length; i++)
            {
                if (db.sentiments.ContainsKey(words[i].ToLower()))
                {
                    weight += db.sentiments[words[i].ToLower()];
                    count++;
                }
            }
            if (count == 0)
            {
                return count;
            }
            return weight / count;
        }
        public void ParseTweets(string path)
        {
            db.ParseTweets(path);
        }
    }
}
