using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace P8_API.Services
{
    public class EmissionService : IEmissionService
    {
        public async Task<double> retrieveEmission(string licenseplate)
        {
            string url = "https://www.nummerplade.net/nummerplade/" + licenseplate + ".html";

            // Load default configuration
            var config = Configuration.Default.WithDefaultLoader();
            // Create a new browsing context
            var context = BrowsingContext.New(config);
            // This is where the HTTP request happens, returns <IDocument> that // we can query later
            var document = await context.OpenAsync(url);

            var km_l_string = document.GetElementById("samlet_beregnet_braendstofforbrug").OuterHtml;
            //var fuelType = document.All.Where(m => m.LocalName == "h6" && m.GetAttribute("InnerHtml").StartsWith("Drivkraft"));
            //double km_l = Convert.ToDouble(km_l_string.Replace(" km/l", ""));
            var fuelType = "Benzin";

            Debug.WriteLine(km_l_string);
            Debug.WriteLine(fuelType);

            double km_l = 10.0;
            return calculateEmission(100/km_l, fuelType);
        }

        private double calculateEmission(double l_100km, string fuelType)
        {
            if (fuelType == "Benzin")
                return l_100km * 2392 / 100;
            else if (fuelType == "Diesel")
                return l_100km * 2640 / 100;

            return 127.0;
        }
    }
}
