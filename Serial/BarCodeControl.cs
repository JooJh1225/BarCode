using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using QRCoder;
using ZXing;

namespace BarCode
{
    class BarCodeControl
    {
        public BitmapImage GenBarCode(string content,BarcodeFormat barcodeFormat )
        {
            //바코드 생성
            BarcodeWriter writer = new BarcodeWriter();

            writer.Format = barcodeFormat;
            writer.Options = new ZXing.Common.EncodingOptions
            {
                Height = 1000,
                Width = 1000,

            };
            Bitmap barcodeImage = writer.Write(content);
            //비트맵 변환
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)barcodeImage).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        
    }
}
