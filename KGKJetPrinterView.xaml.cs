using KGKJetPrinterLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JetPrinter.ui
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>

    public partial class KGKJetPrinterView : UserControl, INotifyPropertyChanged
    {
        public static DependencyProperty MessageContentProperty = DependencyProperty.Register("MessageContent", typeof(string), typeof(KGKJetPrinterView), new PropertyMetadata(default(string), OnMessageContentChanged));

        private static void OnMessageContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KGKJetPrinterView KGKPrinter = d as KGKJetPrinterView;
            if (KGKPrinter != null)
            {
                
            }
        }

        private KGKJetPrinter m_printer;
        private long m_nKgkPort = 1024;
        private string m_dateTimeSelected = DateTime.Now.ToString("ddMMyyyy");
        
        public KGKJetPrinter KGKPrinter { get => m_printer; }

        public KGKJetPrinterView(string ip)
        {
            InitializeComponent();

            this.DataContext = this;

            m_printer = new KGKJetPrinter(ip, m_nKgkPort);
            m_printer.ConnectionStateChanged += M_printer_ConnectionStateChanged;
            m_printer.PrinterStateChanged += M_printer_PrinterStateChanged;

            m_printer.ConnectPrinter();
        }

        private void M_printer_PrinterStateChanged(KGKJetPrinter sender, KGKJetPrinter.PrinterState state)
        {
            switch (state)
            {
                case KGKJetPrinter.PrinterState.Unknown:
                    break;
                case KGKJetPrinter.PrinterState.NotPrinting:
                    break;
                case KGKJetPrinter.PrinterState.Printing:
                    break;
                case KGKJetPrinter.PrinterState.Fault:
                    break;
                default:
                    break;
            }
        }

        private void M_printer_ConnectionStateChanged(KGKJetPrinter sender, KGKJetPrinter.ConnectionState state)
        {
            switch (state)
            {
                case KGKJetPrinter.ConnectionState.Closed:
                    break;
                case KGKJetPrinter.ConnectionState.Open:
                    break;
                case KGKJetPrinter.ConnectionState.Listening:
                    break;
                case KGKJetPrinter.ConnectionState.ConnectionPending:
                    break;
                case KGKJetPrinter.ConnectionState.ResolvingHost:
                    break;
                case KGKJetPrinter.ConnectionState.HostResolved:
                    break;
                case KGKJetPrinter.ConnectionState.Connecting:
                    break;
                case KGKJetPrinter.ConnectionState.Connected:
                    break;
                case KGKJetPrinter.ConnectionState.Closing:
                    break;
                case KGKJetPrinter.ConnectionState.Error:
                    break;
                default:
                    break;
            }
        }

        public string DateTimeSelected
        { 
            get => m_dateTimeSelected;
            set
            {
                m_dateTimeSelected = value;
                OnPropertyChanged("DateTimeSelected");
            }
        }
       
        public string MessageContent
        {
            get
            {
                return (string)GetValue(MessageContentProperty);
            }
            set
            {
                SetValue(MessageContentProperty, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnPrintStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (m_printer.GetPrinterState == KGKJetPrinter.PrinterState.NotPrinting)
            {
                m_printer.StartPrinting();
                btnPrintStartStop.Background = new SolidColorBrush(Colors.Green);
                btnPrintStartStop.Content = "Bắt đầu in";
            }
            else if(m_printer.GetPrinterState == KGKJetPrinter.PrinterState.Printing)
            {
                m_printer.StopPrinting();
                btnPrintStartStop.Background = new SolidColorBrush(Colors.OrangeRed);
                btnPrintStartStop.Content = "Dừng in";
            }
        }

        private void datePickerPrinter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            if (datePicker != null)
            {
                var datetime = datePicker.SelectedDate.Value;
                if (datetime != null)
                {
                    DateTimeSelected = datetime.ToString("ddMMyyyy");

                    MessageContent = string.Empty;
                    MessageContent += DateTimeSelected;
                }
            }
        }
    }
}
