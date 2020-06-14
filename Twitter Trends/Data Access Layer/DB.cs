using Business_Layer;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Collections.Generic;

namespace Data_Access_Layer
{
    public class DB
    {
        public Dictionary<string, State> states;
        public Dictionary<string, double> sentiments;
        public List<Tweet> tweets;
        public List<GMapPolygon> polygons;
        private StatesParser statesParser = new StatesParser();
        private SentimentsParser sentimentsParser = new SentimentsParser();
        private TweetsParser tweetsParser = new TweetsParser();

        private static DB instance;
        private DB()
        {
            ParseTweets("cali_tweets2014.txt");
            ParseStates("states.txt");
            ParseSentiments("sentiments.csv");
            CreatePolygons();
        }
        public void ParseTweets(string tweetsPath)
        {
            tweets = tweetsParser.Parse(tweetsPath);
        }
        public void ParseStates(string statesPath)
        {
            states = statesParser.Parse(statesPath);
            CreatePolygons();
        }
        public void ParseSentiments(string sentimentsPath)
        {
           sentiments = sentimentsParser.Parse(sentimentsPath);
        }

        public static DB GetInstance()
        {
            if (instance == null)
            {
                instance = new DB();
            }
            return instance;
        }
        private void CreatePolygons()
        {
            polygons = new List<GMapPolygon>();
            List<PointLatLng> points = new List<PointLatLng>();
            foreach (State state in states.Values)
            {
                foreach (var polygon in state.Polygons)
                {
                    foreach (Point point in polygon.Points)
                    {
                        points.Add(new PointLatLng(point.Coordinates[1], point.Coordinates[0]));
                    }
                    GMapPolygon mapPolygon = new GMapPolygon(points, state.Code);
                    polygons.Add(new GMapPolygon(points, state.Code));
                    points.Clear();
                }
            }
        }
    }
}

     

