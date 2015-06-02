﻿
namespace StoreAppTest.Convertors
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;
    using Model;


    public class PriceChangeToImageConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <summary>
        /// Изменяет источник данных перед их передачей целевому объекту для отображения в пользовательском интерфейсе.
        /// </summary>
        /// <returns>
        /// Значение, передаваемое целевому свойству зависимостей.
        /// </returns>
        /// <param name="value">Исходные данные, передаваемые целевому объекту.</param><param name="targetType">Тип <see cref="T:System.Type"/> данных, ожидаемый целевым свойством зависимостей.</param><param name="parameter">Необязательный параметр для использования в логике преобразователя.</param><param name="culture">Язык и региональные параметры преобразования.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as PriceChangeReportItem;
            if (item.NewPrice > item.PreviewsPrice)
                //return new BitmapImage(new Uri("StoreAppTest;component/Assets/Images/1431889171_Up.png", UriKind.Relative));
                return new BitmapImage(new Uri("../../Assets/Images/1431889171_Up.png", UriKind.Relative));

            if (item.NewPrice == item.PreviewsPrice)
                return null;

            return new BitmapImage(new Uri("../../Assets/Images/1431889161_Down.png", UriKind.Relative));

        }

        /// <summary>
        /// Изменяет целевые данные перед их передачей исходному объекту.Этот метод вызывается только в привязках <see cref="F:System.Windows.Data.BindingMode.TwoWay"/>.
        /// </summary>
        /// <returns>
        /// Значению, которое следует передать исходному объекту.
        /// </returns>
        /// <param name="value">Целевые данные, передаваемые исходному объекту.</param><param name="targetType">Тип <see cref="T:System.Type"/> данных, ожидаемый исходным объектом.</param><param name="parameter">Необязательный параметр для использования в логике преобразователя.</param><param name="culture">Язык и региональные параметры преобразования.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    //public class PriceChangeToImageConverter : IMultiValueConverter
    //{
    //    #region Implementation of IMultiValueConverter

    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return null;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        return null;
    //    }

    //    #endregion
    //}
}
