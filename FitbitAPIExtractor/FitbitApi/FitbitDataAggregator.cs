﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace FitbitAPIExtractor.FitbitApi
{
    /// <summary>
    /// Will accept a dataset for one day per activity, and can generate a list of minutes 
    /// containing all activity. 
    /// </summary>
    class FitbitDataAggregator
    {
        public JObject StepsJson { get; set; }
        public JObject CaloriesJson { get; set; }
        public JObject FloorsJson { get; set; }
        public JObject ElevationJson { get; set; }
        public JObject DistanceJson { get; set; }

        public void ParseData()
        {
            Dictionary<string, Minute> minutes = new Dictionary<string, Minute>();

            for (int hour = 0; hour < 24; hour++)
            {
                for (int min = 0; min < 60; min++)
                {
                    string timestamp = hour.ToString("D2") + ":" + min.ToString("D2") + ":00";
                    Console.WriteLine("Parsing " + timestamp);
                    Minute m = ParseMinute(timestamp);

                    minutes.Add(timestamp, m);
                }
            }
        }
            
        protected Minute ParseMinute(string timestamp)
        {
            Minute m = new Minute();

            for (int i = 0; i < StepsJson["activities-steps-intraday"]["dataset"].Count(); i++)
            {
                string jsonTimestamp = StepsJson["activities-steps-intraday"]["dataset"][i]["time"].Value<string>();
                if (jsonTimestamp.Equals(timestamp))
                {
                    m.When = timestamp;

                    int steps = Convert.ToInt32(StepsJson["activities-steps-intraday"]["dataset"][i]["value"]);
                    m.Steps = steps;
                    break;
                }    
            }
            
            return m;
        }

    }
}
