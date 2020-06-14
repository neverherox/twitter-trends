using Business_Layer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Data_Access_Layer
{
    public class TweetsParser
    {
        private StreamReader streamReader;
        public List<Tweet> Parse(string path)
        {
            List<Tweet> tweets = new List<Tweet>();
            string allTweets;
            using (streamReader = new StreamReader(path))
            {
                allTweets = streamReader.ReadToEnd();
            }
            string[] strTweets = allTweets.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var pointsPattern = new Regex(@"[+-]?([0-9]*[.])?[0-9]+,\s[+-]?([0-9]*[.])?[0-9]+");
            var datePattern = new Regex(@"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}");
            var messagePattern = new Regex(@"\t.+");
            var regex = new Regex(@"\t_\t\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}\t");
            foreach (var item in strTweets)
            {
                Match point = pointsPattern.Match(item); //достаю точки
                Match date = datePattern.Match(item); //достаю дату и время
                Match message = messagePattern.Match(item);       // достаю сообщение          
                string result = regex.Replace(message.Value, ""); // привожу сообщение в прилежный вид
                string[] LatLng = point.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); //разбиаю точки на две
                string[] date1 = date.Value.Split(new char[] { '-', ':', ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваю дату
                int[] date2 = DateToInt(date1); //перевожу дату в инт
                Point tweetPoint = new Point();
                tweetPoint.Coordinates.Add(Convert.ToDouble(LatLng[1].Replace('.', ',')));
                tweetPoint.Coordinates.Add(Convert.ToDouble(LatLng[0].Replace('.', ',')));
                tweets.Add(new Tweet
                {
                    Point = tweetPoint,
                    date = new DateTime(date2[0], date2[1], date2[2], date2[3], date2[4], date2[5]),
                    Text = result
                }); //добавляю твит
            }
            return tweets;
        }
        private int[] DateToInt(string[] date)
        {
            int[] dateInt = {Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]),
                Convert.ToInt32(date[3]), Convert.ToInt32(date[4]), Convert.ToInt32(date[5]) };
            return dateInt;
        }
    }
}
