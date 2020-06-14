using System;
using System.Collections.Generic;
using System.IO;

namespace Data_Access_Layer
{
    public class SentimentsParser
    {
        private StreamReader streamReader;
        public Dictionary<string,double> Parse(string path)
        {
            Dictionary<string, double> sentiments = new Dictionary<string, double>();
            using (streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var values = line.Split(',');
                    var weight = Convert.ToDouble(Convert.ToDouble(values[1].Replace('.', ',')));
                    sentiments.Add(values[0], weight);
                }
            }
            return sentiments;
        }
    }
}
