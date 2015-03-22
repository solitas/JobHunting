using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace JobHunting.Convert
{
    /// <summary>
    /// Gets the appointments for the specified date.
    /// </summary>
    //[ValueConversion(typeof(ObservableCollection<Recruitment>), typeof(ObservableCollection<Recruitment>))]
    //public class AppointmentsConverter : IMultiValueConverter
    //{
    //    #region IMultiValueConverter Members

    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        DateTime date = (DateTime)values[1];

    //        ObservableCollection<Recruitment> appointments = new ObservableCollection<Recruitment>();
    //        foreach (Recruitment appointment in (ObservableCollection<Recruitment>)values[0])
    //        {
    //            if (appointment.DueDate == date)
    //            {
    //                appointments.Add(appointment);
    //            }
    //        }

    //        return appointments;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        return null;
    //    }

    //    #endregion
    //}
}
