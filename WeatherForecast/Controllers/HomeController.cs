using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net; //This is Required for Webclient class
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization; //This is Required for JavaScriptSerializer method
using WeatherForecast.Models;
using WeatherForecast.Models.FiveDaysThreeHours;

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

        //Action method that will return the "Forecast" view
        public ActionResult Forecast()
        {
            return View();
        }

        //This is the Method to fetch the CURRENT WEATHER response/data from OPENWEATHERMAP.ORG through the API
        [HttpPost]
        public String WeatherDetail(string City)
        {

            //Assign API KEY which received from OPENWEATHERMAP.ORG  
            string appId = "5989c360a72a7faf8a5d317a708cd92f"; //home.openweathermap.org/api_keys

            //API path with CITY parameter and other parameters.  
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", City, appId); //weather - current

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

        //This is the Method to fetch the CURRENT WEATHER response/data from OPENWEATHERMAP.ORG through the API
        [HttpPost]
        public String ForecastDetail(string City)
        {

            //Assign API KEY which received from OPENWEATHERMAP.ORG  
            string appId = "5989c360a72a7faf8a5d317a708cd92f"; //home.openweathermap.org/api_keys

            //API path with CITY parameter and other parameters.  
            string url = string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&units=metric&APPID={1}", City, appId); //weather - forecast

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                //********************//  
                //     JSON RECIVED   
                //********************//  
                //{
                //    "cod": "200",
                // "message": 0,
                // "cnt": 40,
                // "list": [
                //  {
                //   "dt": 1586757600,
                //   "main": {
                //    "temp": 292.18,
                //    "feels_like": 291.09,
                //    "temp_min": 290.06,
                //    "temp_max": 292.18,
                //    "pressure": 1019,
                //    "sea_level": 1019,
                //    "grnd_level": 1014,
                //    "humidity": 55,
                //    "temp_kf": 2.12

                //            },
                //   "weather": [
                //    {
                //     "id": 802,
                //     "main": "Clouds",
                //     "description": "scattered clouds",
                //     "icon": "03d"

                //                }
                //   ],
                //   "clouds": {
                //    "all": 41
                //   },
                //   "wind": {
                //    "speed": 1.53,
                //    "deg": 180
                //   },
                //   "sys": {
                //    "pod": "d"
                //   },
                //   "dt_txt": "2020-04-13 06:00:00"
                //  }
                //    ],
                // "city": {
                //  "id": 2171948,
                //  "name": "Caulfield",
                //  "coord": {
                //   "lat": -37.8825,
                //   "lon": 145.0229
                //  },
                //  "country": "AU",
                //  "population": 4790,
                //  "timezone": 36000,
                //  "sunrise": 1586724268,
                //  "sunset": 1586764559
                // }
                //}

                //Converting to OBJECT from JSON string.  
                DetailedRootObject weatherInfo = JsonConvert.DeserializeObject<DetailedRootObject>(json);//(new JavaScriptSerializer()).Deserialize<RootDetailedForecast>(json);

                //Special VIEWMODEL design to send only required fields not all fields which received from   
                //www.openweathermap.org api  
                DetailedResultViewModel rslt = new DetailedResultViewModel();

                rslt.Country = weatherInfo.city.country;
                rslt.City = weatherInfo.city.name;
                rslt.Lat = Convert.ToString(weatherInfo.city.coord.lat);
                rslt.Lon = Convert.ToString(weatherInfo.city.coord.lon);
                foreach (var list in weatherInfo.list)
                {
                    rslt.Humidity = Convert.ToString(list.main.humidity);
                    rslt.Temp = Convert.ToString(list.main.temp);
                    rslt.TempFeelsLike = Convert.ToString(list.main.feels_like);
                    rslt.TempMax = Convert.ToString(list.main.temp_max);
                    rslt.TempMin = Convert.ToString(list.main.temp_min);
                    foreach (var weather in list.weather)
                    {
                        rslt.Description = weather.description;
                        rslt.WeatherIcon = weather.icon;
                    }
                }

                //Converting OBJECT to JSON String   
                var jsonstring = JsonConvert.SerializeObject(rslt);//new JavaScriptSerializer().Serialize(rslt);

                //Return JSON string.  
                return jsonstring;
            }
        }
    }
}