using BSOA.Test.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BSOA.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Sample s = new Sample()
            {
                Age = 39,
                Count = 512,
                IsActive = true,
                Position = 16 * 1024 * 1024,
                Data = new byte[] { 0, 1, 2, 3 }
            };

            string json = JsonConvert.SerializeObject(s);
            Sample t = JsonConvert.DeserializeObject<Sample>(json);

            JsonTextReader r = new JsonTextReader(new StringReader(json));
            while(r.Read())
            {
                JsonToken token = r.TokenType;
                object value = r.Value;
            }

            bool equal = s.Equals(t);

            List<Sample> samples = new List<Sample>()
            {
                s, new Sample()
            };

            json = JsonConvert.SerializeObject(samples);
            List<Sample> rt = JsonConvert.DeserializeObject<List<Sample>>(json);
            bool equal2 = s.Equals(rt[0]);
        }
    }
}
