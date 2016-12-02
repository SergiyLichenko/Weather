using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Common;
using Windows.UI.Popups;
using System.Collections.ObjectModel;
using Windows.UI.Input;
using System.Threading;
using Common.GoogleAPI;
using Windows.UI.Xaml.Controls.Primitives;

namespace Weather
{
    enum ParticleType
    {
        Snow = 0,
        Rain = 1
    }
    public sealed partial class MainPage : Page
    {
        private const int countOfParticles = 7;
        private const int timerSpeed = 50;
        private const string GoogleMapsPath = "https://maps.google.com/maps?q=";
        private const string messege1 = "Weather service is not available now, try later or change your location";
        private const string messege2 = "Cannot load weather icon";
        private const string messege3 = "Cannot load country icon";
        private const string messege4 = "Select city";
        private const string defaultCity = "Kiev";

        private ParticleType particleType;
        private Controller controller;
        private GoogleObject googleData;
        private Semaphore semaphore;//only 1 thread can access DX layer
        private Random random;
        private Timer timer;
        private int widthOfWindow;


        #region Properties
        public Random MyRandom
        {
            get
            {
                if (this.random == null)
                    random = new Random();
                return random;
            }
        }

        public GoogleObject GoogleData
        {
            get
            {
                return this.googleData;
            }
            private set
            {
                if (value is GoogleObject)
                    this.googleData = value;
            }
        }

        private Semaphore MySemaphore
        {
            get
            {
                if (this.semaphore == null)
                    this.semaphore = new Semaphore(1, 1);
                return this.semaphore;
            }
        }

        private Controller MyController
        {
            get
            {
                if (this.controller == null)
                    controller = new Controller();
                return controller;
            }
        }
        #endregion


        public MainPage()
        {
            this.InitializeComponent();

            ProgressRing_Load.IsActive = true;
            ProgressRing_Load.Visibility = Visibility.Visible;

            this.particleType = ParticleType.Snow;
            (LineChart.Series[0] as LineSeries).LegendItems.Clear();
            this.widthOfWindow = Convert.ToInt32(Window.Current.Bounds.Right);
        }


        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetWeather(defaultCity);
            }
            catch (Exception)
            {
                var dialog = new MessageDialog(messege1);
                await dialog.ShowAsync();
            }
            if (this.particleType == ParticleType.Snow)
                this.DirectXPanel.CreateParticles(countOfParticles, -1);

            timer = new Timer(timerCallback, null, 0, timerSpeed);
            this.WebView_Map.Navigate(new Uri(GoogleMapsPath + defaultCity));
            ProgressRing_Load.IsActive = false;
            ProgressRing_Load.Visibility = Visibility.Collapsed;
        }

        private async void GetWeather(string name)
        {
            Root root = await MyController.GetWeather(name);
            if (root == null)
            {
                var dialog = new MessageDialog(messege1);
                await dialog.ShowAsync();
                return;
            }

            ManageView(root);
            controller.FillObservableCollection(root);
            ListView_Forecast_SelectionChanged(null, null);
        }

        private async void ManageView(Root root)
        {
            try
            {
                this.Image_WeatherIcon.Source = controller.GetWeatherIcon(root.list[0].weatherDescription[0].Icon + ".png");
            }
            catch (Exception)
            {
                var dialog = new MessageDialog(messege2);
                await dialog.ShowAsync();
            }

            try
            {
                this.Image_Country.Source = controller.GetFlagIcon(root.city.Country.ToLower() + ".gif");//
            }
            catch (Exception)
            {
                var dialog = new MessageDialog(messege3);
                await dialog.ShowAsync();
            }

            this.TextBlock_DailyTemperature.Visibility = Visibility.Visible;
            this.Image_Clouds.Visibility = Visibility.Visible;
            this.Image_Humidity.Visibility = Visibility.Visible;
            this.Image_Pressure.Visibility = Visibility.Visible;
            this.Image_Rain.Visibility = Visibility.Visible;
            this.Image_Wind.Visibility = Visibility.Visible;
            if (this.Image_MapUndo.Visibility == Visibility.Collapsed)
                this.Image_MapDo.Visibility = Visibility.Visible;

            this.TextBlock_Latitude.Text = "(Cord: " + root.city.Coord.Latitude + ";";
            this.TextBlock_Longitude.Text = root.city.Coord.Longitude + ")";

            this.TextBlock_Date.Text = "Last update: " + root.list[0].TimeOfDate;
            this.TextBlock_CityName.Text = root.city.Name;
            this.TextBlock_WeatherTemperature.Text = root.list[0].mainInfo.Temperature.ToString() + " °C";
            this.TextBlock_Description.Text = root.list[0].weatherDescription[0].Description;

            this.TextBlock_WeatherName.Text = root.list[0].weatherDescription[0].Main;
            this.TextBlock_Wind.Text = "Wind: " + root.list[0].wind.Speed + " m/sec";

            this.TextBlock_Clounds.Text = "Clouds: " + root.list[0].clouds.Persantage + " %";
            if (root.list[0].rain != null)
                this.TextBlock_Rain.Text = "Rain: " + root.list[0].rain.Volume + " mm";
            this.TextBlock_Humidity.Text = "Humidity: " + root.list[0].mainInfo.Humidity + " %";
            this.TextBlock_Pressure.Text = "Pressure: " + root.list[0].mainInfo.Pressure + " hPa";
        }



        private void ListView_Forecast_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
                if ((sender as ListView).SelectedIndex == -1)
                    return;

            List<string[]> forecastList = null;
            if (sender == null && e == null)
                forecastList = controller.ForecastDays[0].Temperature;
            else
                forecastList = controller.ForecastDays[(sender as ListView).SelectedIndex].Temperature;

            (LineChart.Series[0] as LineSeries).ItemsSource = forecastList;
        }

        private void Page_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint point = e.GetCurrentPoint(this);
            try
            {
                MySemaphore.WaitOne();
                this.DirectXPanel.TrackPos((int)Math.Round(point.Position.X), (int)Math.Round(point.Position.Y));
                MySemaphore.Release();
            }
            catch { }
        }

        private void timerCallback(object state)
        {
            MySemaphore.WaitOne();
            try
            {
                if (this.particleType == ParticleType.Snow)
                    this.DirectXPanel.CreateParticles(countOfParticles, -1);
                else
                    this.DirectXPanel.CreateParticles(countOfParticles, this.widthOfWindow);

                this.DirectXPanel.Move();

                this.DirectXPanel.TrackPos(-1, -1);
            }
            catch (Exception) { }

            MySemaphore.Release();
        }

        private async void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            this.ListBox_Cities.Visibility = Visibility.Visible;
            this.ListBox_Cities.Items.Clear();

            GoogleData = await controller.SearchGoogle(this.TextBox_InputCity.Text);
            foreach (var item in googleData.results)
                this.ListBox_Cities.Items.Add(item.formatted_address);
        }

        private void ListBox_Cities_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.ListBox_Cities.Visibility = Visibility.Collapsed;

            ProgressRing_Load.IsActive = true;
            ProgressRing_Load.Visibility = Visibility.Visible;

            string cityName = GoogleData.results[this.ListBox_Cities.SelectedIndex].formatted_address;
            this.TextBox_InputCity.Text = cityName;
            GetWeather(cityName);

            ProgressRing_Load.IsActive = false;
            ProgressRing_Load.Visibility = Visibility.Collapsed;
        }


        private async void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (this.TextBox_InputCity.Text == String.Empty)
            {
                var dialog = new MessageDialog(messege4);
                await dialog.ShowAsync();
                return;
            }

            ProgressRing_Load.IsActive = true;
            ProgressRing_Load.Visibility = Visibility.Visible;

            this.ListView_Forecast.Visibility = Visibility.Collapsed;
            this.Grid_Graph.Visibility = Visibility.Collapsed;
            this.Image_MapDo.Visibility = Visibility.Collapsed;
            this.WebView_Map.Visibility = Visibility.Visible;
            this.Image_MapUndo.Visibility = Visibility.Visible;

            string url = GoogleMapsPath;
            foreach (var item in this.TextBox_InputCity.Text.Split(' '))
                url += item + "+";
            url = url.Remove(url.Length - 1, 1);

            this.WebView_Map.Navigate(new Uri(url));

            ProgressRing_Load.IsActive = false;
            ProgressRing_Load.Visibility = Visibility.Collapsed;
        }

        private void Image_MapUndo_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.ListView_Forecast.Visibility = Visibility.Visible;
            this.Image_MapDo.Visibility = Visibility.Visible;
            this.Grid_Graph.Visibility = Visibility.Visible;
            this.WebView_Map.Visibility = Visibility.Collapsed;
            this.Image_MapUndo.Visibility = Visibility.Collapsed;
        }

        private void ToggleButton_Switcher_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton))
                return;

            if ((sender as ToggleButton).IsChecked == true)
            {
                MySemaphore.WaitOne();
                this.DirectXPanel.Clear((int)ParticleType.Rain);
                MySemaphore.Release();
                this.PointerMoved -= Page_PointerMoved;
                this.timer.Change(0, timerSpeed / 20);
                this.particleType = ParticleType.Rain;
            }
            else
            {
                MySemaphore.WaitOne();
                this.DirectXPanel.Clear((int)ParticleType.Snow);
                MySemaphore.Release();

                this.PointerMoved += Page_PointerMoved;
                this.timer.Change(0, timerSpeed);
                this.particleType = ParticleType.Snow;
            }
        }
    }

}
