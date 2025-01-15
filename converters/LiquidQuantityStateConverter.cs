using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using KGKJetPrinterLib;

namespace JetPrinter.ui.converters
{
    public class LiquidQuantityStateToIndicatorImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.LiquidQuantity)value;

            switch (state)
            {
                case KGKJetPrinter.LiquidQuantity.Low:
                    return "/JetPrinter.ui;component/resources/KGKImage/low_tank.png";
                case KGKJetPrinter.LiquidQuantity.Full:
                    return "/JetPrinter.ui;component/resources/KGKImage/full_tank.png";
                case KGKJetPrinter.LiquidQuantity.Empty:
                    return "/JetPrinter.ui;component/resources/KGKImage/empty_tank.png";
                case KGKJetPrinter.LiquidQuantity.SensorTrouble:
                    return "";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
