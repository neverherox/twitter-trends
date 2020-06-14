using Business_Layer;
using Data_Access_Layer.interfaces;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Collections.Generic;

namespace Data_Access_Layer
{
    public class StateDao:IStateDao
    {
        private DB db = DB.GetInstance();

        public Dictionary<string, State> GetStates()
        {
            return db.states;
        }
        public State GetState(string code)
        {
            if (db.states.ContainsKey(code))
            {
                return db.states[code];
            }
            return null;
        }
        public Dictionary<string, State> GroupTweetsByState(List<Tweet> tweets)
        {
            foreach(State state in db.states.Values)
            {
                state.Tweets.Clear();
            }
            foreach (Tweet tweet in tweets)
            {
                string code = GetCode(tweet.Point);
                State state = GetState(code);

                if (state != null)
                {
                    state.Tweets.Add(tweet);
                    tweet.State = state;
                }
            }
            return db.states;
        }
        public string GetCode(Point point)
        {
            foreach (GMapPolygon polygon in db.polygons)
            {
                if (polygon.IsInside(new PointLatLng(point.Coordinates[1], point.Coordinates[0])))
                {
                    return polygon.Name;
                }
            }
            return string.Empty;
        }
        public Dictionary<State, double> GetStatesSentiments(Dictionary<string, State> states)
        {
            int count = 0;
            double weight = 0;
            Dictionary<State, double> statesSentiments = new Dictionary<State, double>();
            foreach (State state in states.Values)
            {
                weight = 0;
                count = 0;
                foreach (Tweet tweet in state.Tweets)
                { 
                    weight += tweet.Weight;
                    count++;
                }
                if (count == 0)
                {
                    statesSentiments.Add(state, 0);
                }
                else
                {
                    statesSentiments.Add(state, weight / count);
                }
            }
            return statesSentiments;
        }
        public List<GMapPolygon> GetPolygons()
        {
            return db.polygons;
        }
    }
}
