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
    public class ConnectionStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.ConnectionState)value;

            switch (state)
            {
                case KGKJetPrinter.ConnectionState.Closed:
                    return "Closed";
                case KGKJetPrinter.ConnectionState.Open:
                    return "Open";
                case KGKJetPrinter.ConnectionState.Listening:
                    return "Đang lắng nghe...";
                case KGKJetPrinter.ConnectionState.ConnectionPending:
                    return "ConnectionPending...";
                case KGKJetPrinter.ConnectionState.ResolvingHost:
                    return "ResolvingHost...";
                case KGKJetPrinter.ConnectionState.HostResolved:
                    return "HostResolved";
                case KGKJetPrinter.ConnectionState.Connecting:
                    return "Đang kết nối máy in...";
                case KGKJetPrinter.ConnectionState.Connected:
                    return "Kết nối máy in thành công";
                case KGKJetPrinter.ConnectionState.Closing:
                    return "Đang đóng...";
                case KGKJetPrinter.ConnectionState.Error:
                    return "Lỗi kết nối!";
                default:
                    return "unknown";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConnectionStateToImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.ConnectionState)value;

            switch (state)
            {
                case KGKJetPrinter.ConnectionState.Closed:
                case KGKJetPrinter.ConnectionState.Open:
                case KGKJetPrinter.ConnectionState.Listening:
                case KGKJetPrinter.ConnectionState.ConnectionPending:
                case KGKJetPrinter.ConnectionState.ResolvingHost:
                case KGKJetPrinter.ConnectionState.HostResolved:
                case KGKJetPrinter.ConnectionState.Connecting:
                case KGKJetPrinter.ConnectionState.Closing:
                case KGKJetPrinter.ConnectionState.Error:
                    return "/JetPrinter.ui;component/resources/KGKImage/CONNECT_OFF.png";
                case KGKJetPrinter.ConnectionState.Connected:
                    return "/JetPrinter.ui;component/resources/KGKImage/CONNECT_ON.png";
                default:
                    return "/JetPrinter.ui;component/resources/KGKImage/CONNECT_OFF.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConnectionStateToButtonBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Transparent);

            var state = (KGKJetPrinterLib.KGKJetPrinter.ConnectionState)value;

            switch (state)
            {
                case KGKJetPrinter.ConnectionState.Closed:
                case KGKJetPrinter.ConnectionState.Open:
                case KGKJetPrinter.ConnectionState.Listening:
                case KGKJetPrinter.ConnectionState.ConnectionPending:
                case KGKJetPrinter.ConnectionState.ResolvingHost:
                case KGKJetPrinter.ConnectionState.HostResolved:
                case KGKJetPrinter.ConnectionState.Connecting:
                case KGKJetPrinter.ConnectionState.Closing:
                case KGKJetPrinter.ConnectionState.Error:
                    return new SolidColorBrush(Colors.Green); ;
                case KGKJetPrinter.ConnectionState.Connected:
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

    public class ConnectionStateToButtonContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var state = (KGKJetPrinterLib.KGKJetPrinter.ConnectionState)value;

            switch (state)
            {
                case KGKJetPrinter.ConnectionState.Closed:
                case KGKJetPrinter.ConnectionState.Open:
                case KGKJetPrinter.ConnectionState.Listening:
                case KGKJetPrinter.ConnectionState.ConnectionPending:
                case KGKJetPrinter.ConnectionState.ResolvingHost:
                case KGKJetPrinter.ConnectionState.HostResolved:
                case KGKJetPrinter.ConnectionState.Connecting:
                case KGKJetPrinter.ConnectionState.Closing:
                case KGKJetPrinter.ConnectionState.Error:
                    return "KẾT NỐI";
                case KGKJetPrinter.ConnectionState.Connected:
                    return "NGẮT KẾT NỐI";
                default:
                    return "UNKNOWN";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConnectionStateToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0.3;

            var state = (KGKJetPrinterLib.KGKJetPrinter.ConnectionState)value;

            switch (state)
            {
                case KGKJetPrinter.ConnectionState.Closed:
                case KGKJetPrinter.ConnectionState.Open:
                case KGKJetPrinter.ConnectionState.Listening:
                case KGKJetPrinter.ConnectionState.ConnectionPending:
                case KGKJetPrinter.ConnectionState.ResolvingHost:
                case KGKJetPrinter.ConnectionState.HostResolved:
                case KGKJetPrinter.ConnectionState.Connecting:
                case KGKJetPrinter.ConnectionState.Closing:
                case KGKJetPrinter.ConnectionState.Error:
                    return 0.3;
                case KGKJetPrinter.ConnectionState.Connected:
                    return 1.0;
                default:
                    return 0.3;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
