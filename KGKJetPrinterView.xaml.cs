using KGKJetPrinterLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

        private List<string> m_lstProductionShift = new List<string>() { "Ca 1", "Ca 2", "Ca 3"};
        private KGKJetPrinter m_printer;
        private long m_nKgkPort = 1024;
        private string m_dateTimeSelected = DateTime.Now.ToString("ddMMyy");
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
        private uint m_nPrintCount = 0;
        public uint PrintCount
        {
            get => m_nPrintCount;
            set
            {
                if (m_nPrintCount != value)
                {
                    m_nPrintCount = value;
                    OnPropertyChanged("PrintCount");
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
                if (m_sPrinterState != value)
                {
                    m_sPrinterState = value;
                    OnPropertyChanged("PrinterState");
                }
            }
        }
        private string m_sPrinterName = string.Empty;
        public string PrinterName
        {
            get => m_sPrinterName;
            set
            {
                m_sPrinterName = value;
            }
        }

        private bool m_bPrintDone = true;
        public bool PrintDone
        {
            get => m_bPrintDone;
            set
            {
                m_bPrintDone= value;
                OnPropertyChanged("PrintDone");
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

        private KGKJetPrinter.LiquidQuantity m_inkTankState;
        public KGKJetPrinter.LiquidQuantity InkTankState
        {
            get => m_inkTankState;
            set
            {
                m_inkTankState = value;
                OnPropertyChanged("InkTankState");
            }
        }
        private KGKJetPrinter.LiquidQuantity m_solventTankState;
        public KGKJetPrinter.LiquidQuantity SolventTankState
        {
            get => m_solventTankState;
            set
            {
                m_solventTankState = value;
                OnPropertyChanged("SolventTankState");
            }
        }
        private KGKJetPrinter.LiquidQuantity m_mainTankState;
        public KGKJetPrinter.LiquidQuantity MainTankState
        {
            get => m_mainTankState;
            set
            {
                m_mainTankState = value;
                OnPropertyChanged("MainTankState");
            }
        }

        private KGKJetPrinter.VisicosityState m_viscosityState;
        public KGKJetPrinter.VisicosityState ViscosityState
        {
            get => m_viscosityState;
            set
            {
                m_viscosityState = value;
                OnPropertyChanged("ViscosityState");
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

        public KGKJetPrinterView(string ip, int orderPrinter)
        {
            InitializeComponent();

            this.DataContext = this;
            CurrentMessageNo = 2; // select message no 2
            PrinterName = "MÁY IN " + orderPrinter;
            cbbProductionShift.ItemsSource = m_lstProductionShift;
            cbbProductionShift.SelectedIndex = 0;

            m_printer = new KGKJetPrinter(ip, m_nKgkPort);
            m_printer.AutoReconnect = false; // not auto reconnect
            m_printer.ConnectionStateChanged += M_printer_ConnectionStateChanged;
            m_printer.PrinterStateChanged += M_printer_PrinterStateChanged;
            m_printer.Connected += M_printer_Connected;
            m_printer.Disconnected += M_printer_Disconnected;
            m_printer.PrintCountChanged += M_printer_PrintCountChanged;
        }

        private void M_printer_PrintCountChanged(KGKJetPrinter sender)
        {
            PrintCount = m_printer._LastPrintCount;
        }

        private void M_printer_Disconnected(KGKJetPrinter sender)
        {
            CurrentMessage = "";
            PrintCount = 0;
            m_printer._LastPrintCount = 0;
            tbContentMessage.Dispatcher.BeginInvoke(new Action(() =>
            {
                //MessageContent = "";
                tbContentMessage.Text = "";
            }));
        }

        private void M_printer_Connected(KGKJetPrinter sender)
        {
            ConnectionState = KGKJetPrinter.ConnectionState.Connected;
            if (m_printer.SelectMessage(m_nCurrentMessageNo))
            {
                m_printer.CurrentMessageNo = m_nCurrentMessageNo;
                CurrentMessage = "Bản tin số " + m_nCurrentMessageNo;
            }
        }

        private void M_printer_PrinterStateChanged(KGKJetPrinter sender, PrintHeadState printHeadState, PrintHeadHeaterState heaterState,
            LiquidQuantity inkTankState, LiquidQuantity solventTankState, LiquidQuantity mainTankState, VisicosityState visState)
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
            switch(inkTankState)
            {
                case LiquidQuantity.Low:
                    InkTankState = LiquidQuantity.Low;
                    break;
                case LiquidQuantity.Full:
                    InkTankState = LiquidQuantity.Full;
                    break;
                case LiquidQuantity.Empty:
                    InkTankState = LiquidQuantity.Empty;
                    break;
                case LiquidQuantity.SensorTrouble:
                    InkTankState = LiquidQuantity.SensorTrouble;
                    break;
                default:
                    InkTankState = LiquidQuantity.Unknown;
                    break;
            }
            switch (solventTankState)
            {
                case LiquidQuantity.Low:
                    SolventTankState = LiquidQuantity.Low;
                    break;
                case LiquidQuantity.Full:
                    SolventTankState = LiquidQuantity.Full;
                    break;
                case LiquidQuantity.Empty:
                    SolventTankState = LiquidQuantity.Empty;
                    break;
                case LiquidQuantity.SensorTrouble:
                    SolventTankState = LiquidQuantity.SensorTrouble;
                    break;
                default:
                    SolventTankState = LiquidQuantity.Unknown;
                    break;
            }
            switch (mainTankState)
            {
                case LiquidQuantity.Low:
                    MainTankState = LiquidQuantity.Low;
                    break;
                case LiquidQuantity.Full:
                    MainTankState = LiquidQuantity.Full;
                    break;
                case LiquidQuantity.Empty:
                    MainTankState = LiquidQuantity.Empty;
                    break;
                case LiquidQuantity.SensorTrouble:
                    MainTankState = LiquidQuantity.SensorTrouble;
                    break;
                default:
                    MainTankState = LiquidQuantity.Unknown;
                    break;
            }
            switch (visState)
            {
                case VisicosityState.Normal:
                    ViscosityState = VisicosityState.Normal;
                    break;
                case VisicosityState.Low:
                    ViscosityState = VisicosityState.Low;
                    break;
                case VisicosityState.High:
                    ViscosityState = VisicosityState.High;
                    break;
                case VisicosityState.NotPerformed:
                    ViscosityState = VisicosityState.NotPerformed;
                    break;
                default:
                    ViscosityState = VisicosityState.Unknown;
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
            else if (m_printer.GetPrinterState == KGKJetPrinter.PrintHeadState.Running)
            {
                string s = string.Format("{0}{1} {2}", "Dừng in ", PrinterName, "?");
                if (MessageBox.Show(s, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    m_printer.StopPrinting();
                }
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
                    DateTimeSelected = datetime.ToString("ddMMyy");

                    MessageContent = string.Empty;
                    MessageContent += DateTimeSelected;
                }
            }
        }

        private void btnPushMessage_Click(object sender, RoutedEventArgs e)
        {
            if (m_bPrintDone)
            {
                string s = string.Format("{0}{1} {2}", "Đẩy bản tin này cho ", PrinterName, "?");
                if (MessageBox.Show(s, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (m_printer.UpdateTextModuleNoChangeAttributes(MessageContent, 2))
                    {
                        Thread.Sleep(200);
                        if (m_printer.ResetPrintCounter(CurrentMessageNo))
                        {
                            m_printer._LastPrintCount = 0;
                            PrintCount = 0;
                            PrintDone = false;
                            btnPushMessage.Content = "Bản tin đang được in...";
                            btnPushMessage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#00CC00");
                        }
                        tbContentMessage.Text = MessageContent;
                    }
                }
            }
            else
            {
                string s = string.Format("{0}{1}", PrinterName, " đã in xong bản tin này?");
                if (MessageBox.Show(s, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string printCount = m_printer.GetPrintCounter();
                    uint num = Convert.ToUInt32(printCount);

                    PrintDone = true;
                    btnPushMessage.Content = "Đẩy bản tin";
                    btnPushMessage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#4887d3");
                }
            }
        }

        private void btnConnection_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectionState == ConnectionState.Connected)
            {
                m_printer.DisconnectPrinter();
            }
            else
            {
                m_printer.ConnectPrinter();
            }
        }
    }
}
