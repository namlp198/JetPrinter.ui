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
using static KGKJetPrinterLib.KGKJetPrinter;

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
        private int m_nCurrentMessageNo = -1;
        public int CurrentMessageNo
        {
            get => m_nCurrentMessageNo;
            set
            {
                if (m_nCurrentMessageNo != value)
                {
                    m_nCurrentMessageNo = value;
                    OnPropertyChanged("CurrentMessageNo");
                }
            }
        }
        private string m_sCurrentMessage = string.Empty;
        public string CurrentMessage
        {
            get => m_sCurrentMessage;
            set
            {
                m_sCurrentMessage = value;
                OnPropertyChanged("CurrentMessage");
            }
        }
        private string m_sPrinterState = string.Empty;
        public string PrinterState
        {
            get => m_sPrinterState;
            set
            {
                if(m_sPrinterState != value)
                {
                    m_sPrinterState = value;
                    OnPropertyChanged("PrinterState");
                }
            }
        }

        public KGKJetPrinter KGKPrinter { get => m_printer; }

        private KGKJetPrinter.PrintHeadState m_printHeadState;
        public KGKJetPrinter.PrintHeadState PrintHeadState
        {
            get => m_printHeadState;
            set
            {
                m_printHeadState = value;
                OnPropertyChanged("PrintHeadState");
            }
        }

        private KGKJetPrinter.ConnectionState m_connectionState;
        public KGKJetPrinter.ConnectionState ConnectionState
        {
            get => m_connectionState;
            set
            {
                m_connectionState = value;
                OnPropertyChanged("ConnectionState");
            }
        }

        public KGKJetPrinterView(string ip)
        {
            InitializeComponent();

            this.DataContext = this;
            CurrentMessageNo = 7;

            m_printer = new KGKJetPrinter(ip, m_nKgkPort);
            m_printer.ConnectionStateChanged += M_printer_ConnectionStateChanged;
            m_printer.PrinterStateChanged += M_printer_PrinterStateChanged;
            m_printer.Connected += M_printer_Connected;

            m_printer.ConnectPrinter();
        }

        private void M_printer_Connected(KGKJetPrinter sender)
        {
            ConnectionState = KGKJetPrinter.ConnectionState.Connected;
            if(m_printer.SelectMessage(m_nCurrentMessageNo))
            {
                CurrentMessage = "Bản tin số " + m_nCurrentMessageNo;
            }
        }

        private void M_printer_PrinterStateChanged(KGKJetPrinter sender, PrintHeadState printHeadState, PrintHeadHeaterState heaterState,
            LiquidQuantity inkTankState, LiquidQuantity solventState, LiquidQuantity mainTankState, VisicosityState visState)
        {
            switch (printHeadState)
            {
                case KGKJetPrinter.PrintHeadState.Unknown:
                    PrintHeadState = KGKJetPrinter.PrintHeadState.Unknown;
                    break;
                case KGKJetPrinter.PrintHeadState.Stopping:
                    PrintHeadState = KGKJetPrinter.PrintHeadState.Stopping;
                    break;
                case KGKJetPrinter.PrintHeadState.StoppingAndCoverOpen:
                    PrintHeadState = KGKJetPrinter.PrintHeadState.StoppingAndCoverOpen;
                    break;
                case KGKJetPrinter.PrintHeadState.PreparationOfRunning:
                    PrintHeadState = KGKJetPrinter.PrintHeadState.PreparationOfRunning;
                    break;
                case KGKJetPrinter.PrintHeadState.PreparationOfRunningAndCoverOpen:
                    PrintHeadState = KGKJetPrinter.PrintHeadState.PreparationOfRunningAndCoverOpen;
                    break;
                case KGKJetPrinter.PrintHeadState.Running:
                    PrintHeadState = KGKJetPrinter.PrintHeadState.Running;
                    break;
                case KGKJetPrinter.PrintHeadState.PreparationOfStopping:
                    PrintHeadState = KGKJetPrinter.PrintHeadState.PreparationOfStopping;
                    break;
                case KGKJetPrinter.PrintHeadState.Maintenance:
                    PrintHeadState = KGKJetPrinter.PrintHeadState.Maintenance;
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
                    ConnectionState = KGKJetPrinter.ConnectionState.Closed;
                    break;
                case KGKJetPrinter.ConnectionState.Open:
                    ConnectionState = KGKJetPrinter.ConnectionState.Open;
                    break;
                case KGKJetPrinter.ConnectionState.Listening:
                    ConnectionState = KGKJetPrinter.ConnectionState.Listening;
                    break;
                case KGKJetPrinter.ConnectionState.ConnectionPending:
                    ConnectionState = KGKJetPrinter.ConnectionState.ConnectionPending;
                    break;
                case KGKJetPrinter.ConnectionState.ResolvingHost:
                    ConnectionState = KGKJetPrinter.ConnectionState.ResolvingHost;
                    break;
                case KGKJetPrinter.ConnectionState.HostResolved:
                    ConnectionState = KGKJetPrinter.ConnectionState.HostResolved;
                    break;
                case KGKJetPrinter.ConnectionState.Connecting:
                    ConnectionState = KGKJetPrinter.ConnectionState.Connecting;
                    break;
                case KGKJetPrinter.ConnectionState.Connected:
                    ConnectionState = KGKJetPrinter.ConnectionState.Connected;
                    //m_printer.GetMessageCurrent();
                    break;
                case KGKJetPrinter.ConnectionState.Closing:
                    ConnectionState = KGKJetPrinter.ConnectionState.Closing;
                    break;
                case KGKJetPrinter.ConnectionState.Error:
                    ConnectionState = KGKJetPrinter.ConnectionState.Error;
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
            if (m_printer.GetPrinterState == KGKJetPrinter.PrintHeadState.Stopping ||
                m_printer.GetPrinterState == KGKJetPrinter.PrintHeadState.StoppingAndCoverOpen)
            {
                m_printer.StartPrinting();
            }
            else if(m_printer.GetPrinterState == KGKJetPrinter.PrintHeadState.Running)
            {
                m_printer.StopPrinting();
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

        private void btnPushMessage_Click(object sender, RoutedEventArgs e)
        {
            if(m_printer.UpdateTextModuleNoChangeAttributes(MessageContent))
            {
                tbContentMessage.Text = MessageContent;
            }
        }
    }
}
