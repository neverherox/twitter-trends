using Business_Layer;
using GMap.NET.WindowsForms;
using System.Collections.Generic;

namespace Data_Access_Layer.interfaces
{
    public interface IStateDao
    {
        Dictionary<string, State> GetStates();  
        State GetState(string code);
        Dictionary<string, State> GroupTweetsByState(List<Tweet> tweets);
        string GetCode(Point point);
        Dictionary<State, double> GetStatesSentiments(Dictionary<string, State> states);
        List<GMapPolygon> GetPolygons();
    }
}
