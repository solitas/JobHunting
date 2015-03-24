using System;
using System.Globalization;
using System.Windows.Data;
using JobHunting.Model;

namespace JobHunting.Convert
{
    public class RecruitmentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object obj1 = values[2];
            object obj2 = values[3];

            var recruitment = new Recruitment
            {
                Company = values[0] as string,
                Site = values[1] as string,
                StartDate = obj1==null ? new DateTime() : (DateTime) values[2],
                EndDate = obj2 == null ? new DateTime() : (DateTime)values[3],
            };

            return recruitment;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
