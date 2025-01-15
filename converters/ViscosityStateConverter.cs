using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using KGKJetPrinterLib;

namespace JetPrinter.ui.converters
{
    public class ViscosityStateToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.VisicosityState)value;

            switch (state)
            {
                case KGKJetPrinter.VisicosityState.Normal:
                    return "Bình Thường";
                case KGKJetPrinter.VisicosityState.Low:
                    return "Thấp";
                case KGKJetPrinter.VisicosityState.High:
                    return "Cao";
                case KGKJetPrinter.VisicosityState.NotPerformed:
                    return "Không thể điều chỉnh";
                default:
                    return "Unknow";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ViscosityStateToForeground : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.VisicosityState)value;

            switch (state)
            {
                case KGKJetPrinter.VisicosityState.Normal:
                    return new SolidColorBrush(Colors.Green);
                case KGKJetPrinter.VisicosityState.Low:
                case KGKJetPrinter.VisicosityState.High:
                    return new SolidColorBrush(Colors.Orange);
                case KGKJetPrinter.VisicosityState.NotPerformed:
                    return new SolidColorBrush(Colors.OrangeRed);
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
