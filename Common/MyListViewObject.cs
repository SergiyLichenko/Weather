using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Weather
{
    public class MyListViewObject
    {
        public string Day { get; set; }
        public string DayOfTheWeek { get; set; }
        public string Description { get; set; }
        public BitmapImage Image { get; set; }
        public List<string[]> Temperature { get; set; }
        

        public string MaxTemperature
        {
            get
            {
                double temp=Convert.ToDouble(Temperature[0][0]);
                for (int i = 1; i < Temperature.Count; i++)
                {
                    if (temp < Convert.ToDouble(Temperature[i][0]))
                        temp = Convert.ToDouble(Temperature[i][0]);
                }
                return temp.ToString() + "°";
            }
        }
        public string MinTemperature
        {
            get
            {
                double temp = Convert.ToDouble(Temperature[0][0]);
                for (int i = 1; i < Temperature.Count; i++)
                {
                    if (temp > Convert.ToDouble(Temperature[i][0]))
                        temp = Convert.ToDouble(Temperature[i][0]);
                }
                return temp.ToString() + "°";
            }
        }

        public MyListViewObject()
        {
            Temperature = new List<string[]>();
        }
    }
}
