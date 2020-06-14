using Business_Layer;
using Data_Access_Layer;
using Data_Access_Layer.interfaces;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Service_Layer
{
    public class Service
    {
        static ITweetDao tweetDao = new TweetDao();
        static IStateDao stateDao = new StateDao();

        public List<GMapPolygon> GetPolygons()
        {
            return stateDao.GetPolygons();
        }
        public Dictionary<string, State> GetStates()
        {
            return stateDao.GetStates();
        }
        public State GetState(string code)
        {
            return stateDao.GetState(code);
        }
        public Dictionary<string, State> GroupTweetsByState(List<Tweet> tweets)
        {
            return stateDao.GroupTweetsByState(tweets);
        }
        public string GetCode(Business_Layer.Point point)
        {
            return stateDao.GetCode(point);
        }
        public Dictionary<State, double> GetStatesSentiments(Dictionary<string, State> states)
        {
            return stateDao.GetStatesSentiments(states);
        }
        //---------------------------------------------------------------------TweetDAO
        public void ParseTweets(string path)
        {
            tweetDao.ParseTweets(path);
        }
        public List<Tweet> GetTweets()
        {
            return tweetDao.GetTweets();
        }

        public Tweet GetTweet(string text)
        {
            return tweetDao.GetTweet(text);
        }
        public List<Tweet> GetTweetsSentiments(List<Tweet> tweets)
        {
            return tweetDao.GetTweetsSentiments(tweets);
        }
        public List<Color> GetColors(Dictionary<State,double> statesSentiments)
        {
            List<Color> colors = new List<Color>();
            foreach(State state in statesSentiments.Keys)
            {
                Color color = GetColor(statesSentiments[state]);
                colors.Add(color);
            }
            return colors;

        }
        public Color GetColor(double weight)
        {
            Color color;
            if (weight > 0)
            {
                int addColor = 255 - Convert.ToInt32(weight * 500);
                if (addColor < 0)
                {
                    addColor = 128;
                }
                color = Color.FromArgb(200, 0, addColor, 0);
                return color;
            }
            if (weight < 0)
            {
                int addColor = 255 + Convert.ToInt32(weight * 500);
                if (addColor < 0)
                {
                    addColor = 128;
                }
                color = Color.FromArgb(200, addColor, 0, 0);
                return color;
            }
            color = Color.White;
            return color;
        }
    }
}
