using System.Collections.Generic;
namespace Business_Layer
{
    public class State
    {
        public List<Polygon> Polygons { get; set; }
        public string Code { get; set; }
        public List<Tweet> Tweets { get; set; } = new List<Tweet>();
    }
}
