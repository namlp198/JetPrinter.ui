using KGKJetPrinterLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace JetPrinter.ui.converters
{
    public class PrinterStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.PrinterState)value;

            switch (state)
            {
                case KGKJetPrinter.PrinterState.Unknown:
                    return "unknown";
                case KGKJetPrinter.PrinterState.NotPrinting:
                    return "Dừng in";
                case KGKJetPrinter.PrinterState.Printing:
                    return "Đang in";
                case KGKJetPrinter.PrinterState.Fault:
                    return "Máy in lỗi";
                default:
                    return "unknown";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PrinterStateToIndicatorImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.PrinterState)value;

            switch (state)
            {
                case KGKJetPrinter.PrinterState.Unknown:
                    return "/JetPrinter.ui;component/resources/KGKImage/CIRCLE_GRAY.png";
                case KGKJetPrinter.PrinterState.NotPrinting:
                    return "/JetPrinter.ui;component/resources/KGKImage/CIRCLE_YELLOW.png";
                case KGKJetPrinter.PrinterState.Printing:
                    return "/JetPrinter.ui;component/resources/KGKImage/CIRCLE_GREEN.png";
                case KGKJetPrinter.PrinterState.Fault:
                    return "/JetPrinter.ui;component/resources/KGKImage/CIRCLE_RED.png";
                default:
                    return "/JetPrinter.ui;component/resources/KGKImage/CIRCLE_GRAY.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PrinterStateToPrinterImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.PrinterState)value;

            switch (state)
            {
                case KGKJetPrinter.PrinterState.Unknown:
                case KGKJetPrinter.PrinterState.NotPrinting:
                case KGKJetPrinter.PrinterState.Printing:
                    return "/JetPrinter.ui;component/resources/KGKImage/KGK_OK.png";
                case KGKJetPrinter.PrinterState.Fault:
                    return "/JetPrinter.ui;component/resources/KGKImage/KGK_NG.png";
                default:
                    return "/JetPrinter.ui;component/resources/KGKImage/KGK_OK.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
