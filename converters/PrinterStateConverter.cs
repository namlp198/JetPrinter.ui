using KGKJetPrinterLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace JetPrinter.ui.converters
{
    public class PrinterStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.PrintHeadState)value;

            switch (state)
            {
                case KGKJetPrinter.PrintHeadState.Unknown:
                    return "unknown";
                case KGKJetPrinter.PrintHeadState.Stopping:
                    return "Dừng in";
                case KGKJetPrinter.PrintHeadState.StoppingAndCoverOpen:
                    return "Dừng in và đầu in mở";
                case KGKJetPrinter.PrintHeadState.PreparationOfRunning:
                    return "Chuẩn bị in ...";
                case KGKJetPrinter.PrintHeadState.PreparationOfRunningAndCoverOpen:
                    return "Chuẩn bị in và đầu in mở";
                case KGKJetPrinter.PrintHeadState.Running:
                    return "Đang in";
                case KGKJetPrinter.PrintHeadState.PreparationOfStopping:
                    return "Chuẩn bị dừng in ...";
                case KGKJetPrinter.PrintHeadState.Maintenance:
                    return "Bảo trì máy in";
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

            var state = (KGKJetPrinterLib.KGKJetPrinter.PrintHeadState)value;

            switch (state)
            {
                case KGKJetPrinter.PrintHeadState.Unknown:
                    return "/JetPrinter.ui;component/resources/KGKImage/CIRCLE_GRAY.png";
                case KGKJetPrinter.PrintHeadState.Stopping:
                case KGKJetPrinter.PrintHeadState.StoppingAndCoverOpen:
                case KGKJetPrinter.PrintHeadState.PreparationOfStopping:
                case KGKJetPrinter.PrintHeadState.PreparationOfRunning:
                case KGKJetPrinter.PrintHeadState.PreparationOfRunningAndCoverOpen:
                    return "/JetPrinter.ui;component/resources/KGKImage/CIRCLE_YELLOW.png";
                case KGKJetPrinter.PrintHeadState.Running:
                    return "/JetPrinter.ui;component/resources/KGKImage/CIRCLE_GREEN.png";
                case KGKJetPrinter.PrintHeadState.Maintenance:
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

            var state = (KGKJetPrinterLib.KGKJetPrinter.PrintHeadState)value;

            switch (state)
            {
                case KGKJetPrinter.PrintHeadState.Unknown:
                case KGKJetPrinter.PrintHeadState.Stopping:
                case KGKJetPrinter.PrintHeadState.StoppingAndCoverOpen:
                case KGKJetPrinter.PrintHeadState.PreparationOfStopping:
                case KGKJetPrinter.PrintHeadState.PreparationOfRunningAndCoverOpen:
                case KGKJetPrinter.PrintHeadState.PreparationOfRunning:
                case KGKJetPrinter.PrintHeadState.Running:
                    return "/JetPrinter.ui;component/resources/KGKImage/KGK_OK.png";
                case KGKJetPrinter.PrintHeadState.Maintenance:
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

    public class PrinterStateToBackgroundButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.PrintHeadState)value;

            switch (state)
            {
                case KGKJetPrinter.PrintHeadState.Stopping:
                case KGKJetPrinter.PrintHeadState.StoppingAndCoverOpen:
                    return new SolidColorBrush(Colors.Green);
                case KGKJetPrinter.PrintHeadState.Running:
                    return new SolidColorBrush(Colors.OrangeRed);
                case KGKJetPrinter.PrintHeadState.Unknown:
                case KGKJetPrinter.PrintHeadState.PreparationOfStopping:
                case KGKJetPrinter.PrintHeadState.PreparationOfRunningAndCoverOpen:
                case KGKJetPrinter.PrintHeadState.PreparationOfRunning:
                case KGKJetPrinter.PrintHeadState.Maintenance:
                    return new SolidColorBrush(Colors.Gray);
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PrinterStateToTextButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.PrintHeadState)value;

            switch (state)
            {
                case KGKJetPrinter.PrintHeadState.Stopping:
                case KGKJetPrinter.PrintHeadState.StoppingAndCoverOpen:
                    return "Bắt đầu in";
                case KGKJetPrinter.PrintHeadState.Running:
                    return "Dừng in";
                case KGKJetPrinter.PrintHeadState.PreparationOfStopping:
                case KGKJetPrinter.PrintHeadState.PreparationOfRunningAndCoverOpen:
                case KGKJetPrinter.PrintHeadState.PreparationOfRunning:
                    return "Đợi ...";
                case KGKJetPrinter.PrintHeadState.Unknown:
                case KGKJetPrinter.PrintHeadState.Maintenance:
                    return "Chưa rõ";
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
