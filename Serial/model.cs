using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BarCode
{
    class model
    {
        private BitmapImage bitimg;
        private string barCodeText;

        public BitmapImage Bitimg { get => bitimg; set => bitimg = value; }
        public string BarCodeText { get => barCodeText; set => barCodeText = value; }
    }
}
