using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using P8_API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly IAppSettings _appSettings;
        private List<TransitStop> _data = new List<TransitStop>();
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="settings">AppSettings</param>
        public GoogleService(IAppSettings appsettings)
        {
            _appSettings = appsettings;
        }

        public bool NearbyTransit(int range, double lattitude, double longitude)
        {
            string latString = lattitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
            string longString = longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);

            string url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latString},{longString}&radius={range+5}&type=transit_station&key={_appSettings.GoogleAPIKey}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";

            WebResponse webResponse = request.GetResponse();
            JObject responseJson;

            using (var reader = new StreamReader(webResponse.GetResponseStream()))
            {
                string webContent = reader.ReadToEnd(); // do something fun...
                responseJson = JObject.Parse(webContent);
            }

            save_json(responseJson["status"].ToString() == "OK", lattitude, longitude);

            return responseJson["status"].ToString() == "OK";
        }

        private void save_json(bool result, double lattitude, double longitude)
        {
            _data.Add(new TransitStop()
            {
                Result = result,
                Lattitude = lattitude,
                Longitude = longitude
            });

            string json = JsonConvert.SerializeObject(_data.ToArray());

            // Write string to file
            File.WriteAllText(@"test.json", json);
        }
    }
}
