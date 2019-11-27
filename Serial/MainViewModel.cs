using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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
            new Tuple<BarcodeFormat, string>(BarcodeFormat.CODE_39, "CODE39"),
            new Tuple<BarcodeFormat, string>(BarcodeFormat.EAN_13, "EAN13")
        };

        private bool barcodeLengthReadonlyBool;

        private bool barcodeTextReadonlyBool;

        private bool randomCheckBoxBool;

        private ObservableCollection<model> barCodeList = new ObservableCollection<model>();

        private Double timerDouble;

        private string barcodeLength;

        private BarcodeFormat selectedBarcode;

        private string barcodeText;

        public bool BarcodeLengthReadonlyBool
        {
            get { return barcodeLengthReadonlyBool; }
            set
            {
                barcodeLengthReadonlyBool = value;
                OnPropertyChanged(nameof(BarcodeLengthReadonlyBool));
            }
        }

        public bool BarcodeTextReadonlyBool
        {
            get { return barcodeTextReadonlyBool; }
            set
            {
                barcodeTextReadonlyBool = value;
                OnPropertyChanged(nameof(BarcodeTextReadonlyBool));
            }
        }

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

        public string BarcodeLength
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
                if (SelectedBarcode == BarcodeFormat.EAN_13)
                {
                    if (RandomCheckBoxBool == true)
                    {
                        BarcodeLengthReadonlyBool = true;
                        BarcodeLength = "체크섬 포함 13자리 자동 생성";
                    }
                    else if (RandomCheckBoxBool == false)
                    {
                        BarcodeLengthReadonlyBool = true;
                        BarcodeLength = "체크섬을 제외한 12자리 기입";
                    }
                }
                else
                {
                    BarcodeLengthReadonlyBool = false;
                    BarcodeLength = "1";
                }
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

        public string BarcodeText
        {
            get { return barcodeText; }
            set
            {
                barcodeText = value;
                OnPropertyChanged(nameof(BarcodeText));
            }
        }

        public bool RandomCheckBoxBool
        {
            get { return randomCheckBoxBool; }
            set
            {
                randomCheckBoxBool = value;
                if (value == true)
                {
                    BarcodeText = null;
                    BarcodeTextReadonlyBool = true;
                    if (SelectedBarcode == BarcodeFormat.EAN_13)
                    {
                        BarcodeLength = "체크섬 포함 13자리 자동 생성";
                        BarcodeLengthReadonlyBool = true;
                    }
                }
                else if (value == false)
                {
                    BarcodeTextReadonlyBool = false;
                    if (SelectedBarcode == BarcodeFormat.EAN_13)
                    {
                        BarcodeLength = "체크섬을 제외한 12자리 기입";
                        BarcodeLengthReadonlyBool = true;
                    }
                }
            }
        }

        public MainViewModel()
        {
            this.ButtonClk = new ButtonClk(this);
            this.StopTimer = new StopTimer(this);
        }

        public ICommand ButtonClk { protected get; set; }
        public ICommand StopTimer { protected get; set; }

        public void aa(string length)
        {
            model md = new model();
            if (RandomCheckBoxBool == true)
            {
                if (selectedBarcode == BarcodeFormat.EAN_13)
                {
                    Tuple<BitmapImage, string> qrcode = bc.GenBarCode(rs.rnd(12), SelectedBarcode);
                    md.Bitimg = qrcode.Item1;
                    md.BarCodeText = qrcode.Item2;
                    BarCodeList.Add(md);
                }
                else
                {
                    Tuple<BitmapImage, string> qrcode = bc.GenBarCode(rs.rnd(Convert.ToInt32(length)), SelectedBarcode);
                    md.Bitimg = qrcode.Item1;
                    md.BarCodeText = qrcode.Item2;
                    BarCodeList.Add(md);
                }
            }
            else if (RandomCheckBoxBool == false)
            {
                Tuple<BitmapImage, string> qrcode = bc.GenBarCode(BarcodeText, SelectedBarcode);
                md.Bitimg = qrcode.Item1;
                md.BarCodeText = qrcode.Item2;
                BarCodeList.Add(md);
            }
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
            viewModel.timer.Interval = TimeSpan.FromMilliseconds((double)parameter * 1000);
            viewModel.timer.Tick += new EventHandler(viewModel.timer_Tick);
            viewModel.timer.Start();
            viewModel.BarcodeLengthReadonlyBool = true;
            viewModel.BarcodeTextReadonlyBool = true;

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
            if (viewModel.RandomCheckBoxBool == false)
            {
                viewModel.BarcodeTextReadonlyBool = false;
                if (viewModel.SelectedBarcode == BarcodeFormat.EAN_13)
                {
                    viewModel.BarcodeLengthReadonlyBool = true;
                }
                else
                {
                    viewModel.BarcodeLengthReadonlyBool = false;
                }
            }
            else if (viewModel.RandomCheckBoxBool == true)
            {
                if (viewModel.SelectedBarcode != BarcodeFormat.EAN_13)
                {
                    viewModel.BarcodeLengthReadonlyBool = false;
                }
            }
        }
    }
}