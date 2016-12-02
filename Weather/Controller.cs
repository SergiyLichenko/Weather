using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Common;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Linq;
using Common.GoogleAPI;
using Windows.UI.Xaml.Media;

namespace Weather
{
    class Controller
    {
        private ObservableCollection<MyListViewObject> forecastDays;
        private Model model;


        private Model MyModel
        {
            get
            {
                if (model == null)
                    model = new Model();
                return model;
            }
        }
        public ObservableCollection<MyListViewObject> ForecastDays
        {
            get
            {
                if (this.forecastDays == null)
                    this.forecastDays = new ObservableCollection<MyListViewObject>();
                return this.forecastDays;
            }
        }


        public async Task<Root> GetWeather(string name)
        {
            try
            {
                Root data = null;

                while (data == null || data.city == null)
                {
                    var result = await MyModel.WeatherRequest(name.Trim());
                    var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
                    var serializer = new DataContractJsonSerializer(typeof(Root));

                    data = (Root)serializer.ReadObject(memoryStream);
                }
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<GoogleObject> SearchGoogle(string searchCity)
        {
            try
            {
                searchCity = searchCity.Trim();
                var result = await MyModel.GoogleRequest(searchCity.Split(' '));
                var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));

                var serializer = new DataContractJsonSerializer(typeof(GoogleObject));
                var data = (GoogleObject)serializer.ReadObject(memoryStream);

                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void FillObservableCollection(Root root)
        {
            ForecastDays.Clear();
            foreach (var item in root.list)
            {
                MyListViewObject tempElement = null;
                tempElement = ForecastDays.FirstOrDefault(val => val.Day == Convert.ToDateTime(item.TimeOfDate).Day.ToString());

                if (tempElement == null)
                {
                    tempElement = new MyListViewObject();

                    tempElement.Image = (BitmapImage) model.GetWeatherIcon(item.weatherDescription[0].Icon + ".png");  
                    tempElement.Day = Convert.ToDateTime(item.TimeOfDate).Day.ToString();
                    tempElement.DayOfTheWeek = Convert.ToDateTime(item.TimeOfDate).DayOfWeek.ToString();
                    tempElement.Description = item.weatherDescription[0].Description;

                    this.ForecastDays.Add(tempElement);
                }
                tempElement.Temperature.Add(new string[]
                {
                    item.mainInfo.Temperature.ToString(),
                    Convert.ToDateTime(item.TimeOfDate).TimeOfDay.ToString()
                });
            }
        }

        internal ImageSource GetWeatherIcon(string iconName)
        {
            return model.GetWeatherIcon(iconName);
        }

        internal ImageSource GetFlagIcon(string iconName)
        {
            return model.GetFlagIcon(iconName);
        }
    }
}