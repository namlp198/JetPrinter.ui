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

        public delegate void PrintDoneHanlder(List<string> data);
        public event PrintDoneHanlder PrintCompletedEvent;

        public delegate void ReportPrinterHandler(string data);
        public event ReportPrinterHandler ReportFromPrinterEvent;

        private List<string> m_lstProductionShift = new List<string>() { "Ca 1", "Ca 2", "Ca 3" };
        private List<string> m_listPrintCompleteData = new List<string>();

        private KGKJetPrinter m_printer;
        private long m_nKgkPort = 1024;
        private string m_dateTimeSelected = DateTime.Now.ToString("ddMMyy");
        private int m_nCurrentMessageNo = -1;
        private int m_nTextModule = -1;

        private bool m_bUseTimerCheckPrintCount = true;
        private bool m_bUseTimerCheckPrintState = true;
        private bool m_bIsResetPrintCount = true;

        private string m_strStartTimePrint = string.Empty;
        private string m_strEndTimePrint = string.Empty;
        private string m_strDeliveryCode = string.Empty;
        public string DeliveryCode
        {
            get => m_strDeliveryCode;
            set => m_strDeliveryCode = value;
        }

        public KGKJetPrinterView()
        {

        }
        public KGKJetPrinterView(string ip, int printerOrder)
        {
            InitializeComponent();

            this.DataContext = this;

            Initialize(ip, printerOrder);
        }

        private void Initialize(string ip, int printerOrder)
        {
            CurrentMessageNo = 2; // select message no 2
            PrinterOrder = printerOrder;
            PrinterName = "MÁY IN " + printerOrder;
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
        public void SetParamsDefault(int nTextModule, bool bUseTimCheckPrintState, bool bUseTimCheckPrintCount, 
                                    int nDelayTimCheckPrintState, int nDelayTimCheckPrintCount, bool bIsResetPrintCount)
        {
            TextModule = nTextModule;
            IsResetPrintCount = bIsResetPrintCount;

            m_printer.UseTimerCheckPrintState = bUseTimCheckPrintState;
            m_printer.UseTimerCheckPrintCount = bUseTimCheckPrintCount;
            m_printer.DelayTimeCheckPrintState = nDelayTimCheckPrintState;
            m_printer.DelayTimeCheckPrintCount = nDelayTimCheckPrintCount;
        }
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

        private int m_nPrinterOrder;
        public int PrinterOrder
        {
            get => m_nPrinterOrder;
            set => m_nPrinterOrder = value;
        }

        public int TextModule
        {
            get => m_nTextModule;
            set => m_nTextModule = value;
        }

        private bool m_bPrintDone = true;
        public bool PrintDone
        {
            get => m_bPrintDone;
            set
            {
                m_bPrintDone = value;
                OnPropertyChanged("PrintDone");
            }
        }
        public bool UseTimerCheckPrintCount
        {
            get => m_bUseTimerCheckPrintCount;
            set
            {
                if (m_bUseTimerCheckPrintCount != value)
                {
                    m_bUseTimerCheckPrintCount = value;
                    OnPropertyChanged("UseTimerCheckPrintCount");

                    if (m_bUseTimerCheckPrintCount)
                    {
                        tbLabelPrintCount.Visibility = Visibility.Visible;
                        tbPrintCount.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tbLabelPrintCount.Visibility = Visibility.Collapsed;
                        tbPrintCount.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        public bool UseTimerCheckPrintState
        {
            get => m_bUseTimerCheckPrintState;
            set
            {
                if (m_bUseTimerCheckPrintState != value)
                {
                    m_bUseTimerCheckPrintState = value;
                    OnPropertyChanged("UseTimerCheckPrintState");

                    if (m_bUseTimerCheckPrintState)
                    {
                        groupPrintState.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        groupPrintState.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        public bool IsResetPrintCount
        {
            get => m_bIsResetPrintCount;
            set
            {
                m_bIsResetPrintCount = value;
                OnPropertyChanged("IsResetPrintCount");
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

        private KGKJetPrinter.LiquidQuantity m_inkTankState = LiquidQuantity.Unknown;
        public KGKJetPrinter.LiquidQuantity InkTankState
        {
            get => m_inkTankState;
            set
            {
                if (m_inkTankState != value)
                {
                    m_inkTankState = value;
                    OnPropertyChanged("InkTankState");

                    string data = InkTankStateToMessage(m_inkTankState);
                    ReportFromPrinterEvent?.Invoke(data);
                }
            }
        }
        private KGKJetPrinter.LiquidQuantity m_solventTankState = LiquidQuantity.Unknown;
        public KGKJetPrinter.LiquidQuantity SolventTankState
        {
            get => m_solventTankState;
            set
            {
                if (m_solventTankState != value)
                {
                    m_solventTankState = value;
                    OnPropertyChanged("SolventTankState");

                    string data = SolventTankStateToMessage(m_solventTankState);
                    ReportFromPrinterEvent?.Invoke(data);
                }
            }
        }
        private KGKJetPrinter.LiquidQuantity m_mainTankState = LiquidQuantity.Unknown;
        public KGKJetPrinter.LiquidQuantity MainTankState
        {
            get => m_mainTankState;
            set
            {
                if (m_mainTankState != value)
                {
                    m_mainTankState = value;
                    OnPropertyChanged("MainTankState");

                    string data = MainTankStateToMessage(m_mainTankState);
                    ReportFromPrinterEvent?.Invoke(data);
                }
            }
        }

        private KGKJetPrinter.VisicosityState m_viscosityState = VisicosityState.Unknown;
        public KGKJetPrinter.VisicosityState ViscosityState
        {
            get => m_viscosityState;
            set
            {
                if (m_viscosityState != value)
                {
                    m_viscosityState = value;
                    OnPropertyChanged("ViscosityState");

                    string data = VisicosityStateToMessage(m_viscosityState);
                    ReportFromPrinterEvent?.Invoke(data);
                }
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

        private string InkTankStateToMessage(LiquidQuantity liquidQuantity)
        {
            switch (liquidQuantity)
            {
                case LiquidQuantity.Unknown:
                    return "Không rõ";
                case LiquidQuantity.Low:
                    return "Bình mực ở mức thấp";
                case LiquidQuantity.Full:
                    return "Bình mực đã đầy";
                case LiquidQuantity.Empty:
                    return "Bình mực trống rỗng";
                case LiquidQuantity.SensorTrouble:
                    return "Cảm biến gặp vấn đề";
                default:
                    return "Không rõ";
            }
        }
        private string SolventTankStateToMessage(LiquidQuantity liquidQuantity)
        {
            switch (liquidQuantity)
            {
                case LiquidQuantity.Unknown:
                    return "Không rõ";
                case LiquidQuantity.Low:
                    return "Bình dung môi ở mức thấp";
                case LiquidQuantity.Full:
                    return "Bình dung môi đã đầy";
                case LiquidQuantity.Empty:
                    return "Bình dung môi trống rỗng";
                case LiquidQuantity.SensorTrouble:
                    return "Cảm biến gặp vấn đề";
                default:
                    return "Không rõ";
            }
        }
        private string MainTankStateToMessage(LiquidQuantity liquidQuantity)
        {
            switch (liquidQuantity)
            {
                case LiquidQuantity.Unknown:
                    return "Không rõ";
                case LiquidQuantity.Low:
                    return "Bình chính ở mức thấp";
                case LiquidQuantity.Full:
                    return "Bình chính đã đầy";
                case LiquidQuantity.Empty:
                    return "Bình chính trống rỗng";
                case LiquidQuantity.SensorTrouble:
                    return "Cảm biến gặp vấn đề";
                default:
                    return "Không rõ";
            }
        }
        private string VisicosityStateToMessage(VisicosityState visicosityState)
        {
            switch (visicosityState)
            {
                case VisicosityState.Unknown:
                    return "Không rõ";
                case VisicosityState.Normal:
                    return "Độ nhớt bình thường";
                case VisicosityState.Low:
                    return "Độ nhớt thấp";
                case VisicosityState.High:
                    return "Độ nhớt cao";
                case VisicosityState.NotPerformed:
                    return "Không thể điều chỉnh độ nhớt";
                default:
                    return "Không rõ";
            }
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
            switch (inkTankState)
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
                    // update 14022025
                    DateTimeSelected = datetime.ToString("dd/MM/yyyy");
                    //DateTimeSelected = "NSX:" + DateTimeSelected;

                    MessageContent = string.Empty;
                    MessageContent += DateTimeSelected;
                }
            }
        }

        private void btnPushMessage_Click(object sender, RoutedEventArgs e)
        {
            if (m_bPrintDone)
            {
                string s = string.Format("{0}{1} {2}", "Đẩy nội dung này xuống cho ", PrinterName, "?");
                if (MessageBox.Show(s, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (m_printer.UpdateTextModuleNoChangeAttributes(MessageContent, TextModule))
                    {
                        Thread.Sleep(200);
                        if (IsResetPrintCount)
                        {
                            if (m_printer.ResetPrintCounter(CurrentMessageNo))
                            {
                                m_strStartTimePrint = DateTime.Now.ToString("HH:mm:ss");
                                m_printer._LastPrintCount = 0;
                                PrintCount = 0;
                                PrintDone = false;
                                btnPushMessage.Content = "Bản tin đang được in...";
                                btnPushMessage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#00CC00");
                            }
                        }
                        else
                        {
                            m_strStartTimePrint = DateTime.Now.ToString("HH:mm:ss");
                            m_printer._LastPrintCount = 0;
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
                    // delete prev data
                    m_listPrintCompleteData.Clear();

                    string printCount = m_printer.GetPrintCounter();
                    uint num = Convert.ToUInt32(printCount);
                    PrintCount = num;

                    PrintDone = true;
                    btnPushMessage.Content = "Đẩy bản tin";
                    btnPushMessage.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#4887d3");

                    string date = DateTime.Now.ToString("dd-MM-yyyy");
                    m_strEndTimePrint = DateTime.Now.ToString("HH:mm:ss");

                    m_listPrintCompleteData.Add(date);
                    m_listPrintCompleteData.Add(m_strStartTimePrint);
                    m_listPrintCompleteData.Add(m_strEndTimePrint);
                    m_listPrintCompleteData.Add(cbbProductionShift.SelectedItem.ToString());
                    m_listPrintCompleteData.Add(DeliveryCode);
                    m_listPrintCompleteData.Add(MessageContent);
                    m_listPrintCompleteData.Add(printCount);

                    PrintCompletedEvent?.Invoke(m_listPrintCompleteData);
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
