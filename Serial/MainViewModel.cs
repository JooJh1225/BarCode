using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BarCode
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public DispatcherTimer timer = new DispatcherTimer();

        public BarCodeControl bc = new BarCodeControl();

        public RandomStream rs = new RandomStream();

        private ObservableCollection<model> barCodeList = new ObservableCollection<model>();

        private Double timerDouble;

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
        }

        public ICommand ButtonClk { protected get; set; }

        public void aa()
        {
            string rnd = rs.rnd(8);
            model md = new model();
            BitmapImage qrcode = bc.GenBarCode(rnd);
            md.Bitimg = qrcode;
            md.BarCodeText = rnd;
            BarCodeList.Add(md);
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
            viewModel.timer.Tick += new EventHandler(timer_Tick);
            viewModel.timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            viewModel.aa();
        }
    }
}