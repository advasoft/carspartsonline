
namespace StoreAppTest.Convertors
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class DebtStatusToColorConverter : IValueConverter
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
            bool val = (bool)value;
            if (val)
                return new SolidColorBrush(Colors.LightGray);
            else
                return null;
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
}
