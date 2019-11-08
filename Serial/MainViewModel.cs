using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Collections.Generic;
using ZXing;

namespace BarCode
{
    internal class MainViewModel : INotifyPropertyChanged
    {


        public DispatcherTimer timer = new DispatcherTimer();

        public BarCodeControl bc = new BarCodeControl();

        public RandomStream rs = new RandomStream();

        private List<Tuple<BarcodeFormat, string>> barcodeFormats = new List<Tuple<BarcodeFormat, string>>()
        {
            new Tuple<BarcodeFormat, string>(BarcodeFormat.QR_CODE, "QRCode"),
            new Tuple<BarcodeFormat, string>(BarcodeFormat.CODE_128, "CODE128"),
            new Tuple<BarcodeFormat, string>(BarcodeFormat.CODE_39, "CODE39")
            //BarcodeFormat.CODE_128,
            //BarcodeFormat.EAN_13,
            //BarcodeFormat.CODE_39
        };

        private ObservableCollection<model> barCodeList = new ObservableCollection<model>();

        private Double timerDouble;

        private int barcodeLength;

        private BarcodeFormat selectedBarcode;

        public List<Tuple<BarcodeFormat, string>> BarcodeFormats
        {
            get { return barcodeFormats; }
            set
            {
                barcodeFormats = value;
            }
        }

        public ObservableCollection<model> BarCodeList
        {
            get { return barCodeList; }
            set
            {
                barCodeList = value;
                OnPropertyChanged(nameof(BarCodeList));
            }
        }

        public Double TimerDouble
        {
            get { return timerDouble; }
            set
            {
                timerDouble = value;
                OnPropertyChanged(nameof(TimerDouble));
            }
        }

        public int BarcodeLength
        {
            get { return barcodeLength; }
            set
            {
                barcodeLength = value;
                OnPropertyChanged(nameof(BarcodeLength));
            }
        }

        public BarcodeFormat SelectedBarcode
        {
            get { return selectedBarcode; }
            set
            {
                selectedBarcode = value;
                OnPropertyChanged(nameof(SelectedBarcode));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

       

        public MainViewModel()
        {
            this.ButtonClk = new ButtonClk(this);
            this.StopTimer = new StopTimer(this);
        }

        public ICommand ButtonClk { protected get; set; }
        public ICommand StopTimer { protected get; set; }

        public void aa(int length)
        {
            string rnd = rs.rnd(length);
            model md = new model();
            BitmapImage qrcode = bc.GenBarCode(rnd, SelectedBarcode);
            md.Bitimg = qrcode;
            md.BarCodeText = rnd;
            BarCodeList.Add(md);
        }

        public void timer_Tick(object sender, EventArgs e)
        {
           aa(BarcodeLength);
        }
    }

    internal class ButtonClk : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private MainViewModel viewModel;

        public ButtonClk(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.OnPropertyChanged(nameof(viewModel.TimerDouble));
            viewModel.timer.Interval = TimeSpan.FromMilliseconds((double)parameter);
            viewModel.timer.Tick += new EventHandler(viewModel.timer_Tick);
            viewModel.timer.Start();
        }

        
    }

    internal class StopTimer : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private MainViewModel viewModel;

        public StopTimer(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.timer.Tick -= new EventHandler(viewModel.timer_Tick);
            viewModel.timer.Stop();            
        }
    }
}