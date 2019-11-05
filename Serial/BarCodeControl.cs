using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using QRCoder;

namespace BarCode
{
    class BarCodeControl
    {
        public BitmapImage GenBarCode(string src)
        {
            //바코드 생성
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrData = qrGenerator.CreateQrCode(src, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            //비트맵 변환
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)qrCodeImage).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        
    }
}
