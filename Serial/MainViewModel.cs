using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace BarCode
{
    class MainViewModel : INotifyPropertyChanged
    {

        public BarCodeControl bc = new BarCodeControl();

        public RandomStream rs = new RandomStream();

        private ObservableCollection<model> barCodeList = new ObservableCollection<model>();

        public ObservableCollection<model> BarCodeList
        {
            get { return barCodeList; }
            set
            {
                barCodeList = value;
                OnPropertyChanged(nameof(BarCodeList));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MainViewModel()
        {
            aa();
        }

        public void aa()
        {
            
            for (int i = 0; i < 11; i++)
            {
                string rnd = rs.rnd(8);
                model md = new model();
                BitmapImage qrcode = bc.GenBarCode(rnd);
                md.Bitimg = qrcode;
                md.BarCodeText = rnd;
                BarCodeList.Add(md);             
            };
             
        }



    }
}
