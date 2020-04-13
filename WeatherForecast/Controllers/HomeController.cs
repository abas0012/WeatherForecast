using System;
using System.Collections.Generic;
using System.Linq;
using System.Net; //This is Required for Webclient class
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization; //This is Required for JavaScriptSerializer method
using WeatherForecast.Models;

namespace WeatherForecast.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Action method that will return the "Weather" view
        public ActionResult Weather()
        {
            return View();
        }

        //This is the Method to fetch the response/data from OPENWEATHERMAP.ORG through the API
        [HttpPost]
        public String WeatherDetail(string City)
        {

            //Assign API KEY which received from OPENWEATHERMAP.ORG  
            string appId = "5989c360a72a7faf8a5d317a708cd92f"; //home.openweathermap.org/api_keys

            //API path with CITY parameter and other parameters.  
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", City, appId);
            //string url = "http://api.openweathermap.org/data/2.5/weather?q=" + City + "&units=metric&cnt=1&APPID=" + appId;

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                //********************//  
                //     JSON RECIVED   
                //********************//  
                //{"coord":{ "lon":72.85,"lat":19.01},  
                //"weather":[{"id":711,"main":"Smoke","description":"smoke","icon":"50d"}],  
                //"base":"stations",  
                //"main":{"temp":31.75,"feels_like":31.51,"temp_min":31,"temp_max":32.22,"pressure":1014,"humidity":43},  
                //"visibility":2500,  
                //"wind":{"speed":4.1,"deg":140},  
                //"clouds":{"all":0},  
                //"dt":1578730750,  
                //"sys":{"type":1,"id":9052,"country":"IN","sunrise":1578707041,"sunset":1578746875},  
                //"timezone":19800,  
                //"id":1275339,  
                //"name":"Mumbai",  
                //"cod":200}  

                //Converting to OBJECT from JSON string.  
                RootObject weatherInfo = (new JavaScriptSerializer()).Deserialize<RootObject>(json);

                //Special VIEWMODEL design to send only required fields not all fields which received from   
                //www.openweathermap.org api  
                ResultViewModel rslt = new ResultViewModel();

                rslt.Country = weatherInfo.sys.country;
                rslt.City = weatherInfo.name;
                rslt.Lat = Convert.ToString(weatherInfo.coord.lat);
                rslt.Lon = Convert.ToString(weatherInfo.coord.lon);
                rslt.Description = weatherInfo.weather[0].description;
                rslt.Humidity = Convert.ToString(weatherInfo.main.humidity);
                rslt.Temp = Convert.ToString(weatherInfo.main.temp);
                rslt.TempFeelsLike = Convert.ToString(weatherInfo.main.feels_like);
                rslt.TempMax = Convert.ToString(weatherInfo.main.temp_max);
                rslt.TempMin = Convert.ToString(weatherInfo.main.temp_min);
                rslt.WeatherIcon = weatherInfo.weather[0].icon;

                //Converting OBJECT to JSON String   
                var jsonstring = new JavaScriptSerializer().Serialize(rslt);

                //Return JSON string.  
                return jsonstring;
            }
        }
    }
}