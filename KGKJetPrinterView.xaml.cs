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
        private KGKJetPrinter m_printer;
        private long m_nKgkPort = 1024;
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

        private int m_nIdx;
        public int Index
        { 
            get => m_nIdx;
            set
            {
                m_nIdx = value;
                OnPropertyChanged("Index");
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
            m_printer.StartPrinting();
        }
    }
}
