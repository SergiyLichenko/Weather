using System;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

namespace Weather
{
    class Model
    {
        private const string weatherApi = "http://api.openweathermap.org/data/2.5/forecast?";
        private const string weatherApiKey = "d7525ce23444052bbf9fbffe6c332304";

        private const string GoogleApi = "https://maps.googleapis.com/maps/api/geocode/json?address=";
        private const string GoogleApiKey = "AIzaSyA9Ae9529NGz1s7ypkg2juS3JTDEH5cYyI";

        private const string IconPath = "http://openweathermap.org/img/w/";
        private const string CountryPath = "http://www.geonames.org/flags/x/";

        public async Task<string> WeatherRequest(string name)
        {
            try
            {
                var http = new HttpClient();
                var url = String.Format("{0}q={1}&mode=json&appid={2}&units=metric", weatherApi, name, weatherApiKey);
                var response = await http.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        internal async Task<string> GoogleRequest(string[] searchCity)
        {
            try
            {
                var http = new HttpClient();
                var url = GoogleApi;
                foreach (var item in searchCity)
                    url += item + '+';

                url = url.Remove(url.Length - 1, 1);
                url += "&key=" + GoogleApiKey;
                var response = await http.GetAsync(url);

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        internal ImageSource GetWeatherIcon(string iconName)
        {
            try
            {

                string url = IconPath + iconName;
                return new BitmapImage(new Uri(url));
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal ImageSource GetFlagIcon(string iconName)
        {
            try
            {
                return new BitmapImage(new Uri(CountryPath + iconName));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}