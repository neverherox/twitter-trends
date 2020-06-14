using Business_Layer;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Data_Access_Layer
{
    public class StatesParser
    {
        private StreamReader streamReader;
        public Dictionary<string,State> Parse(string path)
        {
            string allStates;
            using (streamReader = new StreamReader(path))
            {
                allStates = streamReader.ReadToEnd();
            }
            Dictionary<string, State> states = JsonConvert.DeserializeObject<Dictionary<string, State>>(allStates);
            return states;        
        }
    }
}
