using System;

namespace Business_Layer
{
    public class Tweet
    {
        public string Text { get; set; }
        public DateTime date { get; set; }
        public Point Point { get; set; }
        public State State { get; set; }
        public double Weight { get; set; }
    }
}
