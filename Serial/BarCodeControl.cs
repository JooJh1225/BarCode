using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ZXing;

namespace BarCode
{
    internal class BarCodeControl
    {
        public Tuple<BitmapImage, string> GenBarCode(string content, BarcodeFormat barcodeFormat)
        {
            MainViewModel main = new MainViewModel();
            string BarcodeData;
            int? checksum = null;
            //바코드 생성
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = barcodeFormat;

            if (barcodeFormat == BarcodeFormat.EAN_13)
            {
                int length = content.Length;

                int sum = 0;
                if (content.Length == 12)
                {
                    for (int i = length - 1; i >= 0; i -= 2)
                    {
                        int digit = content[i] - '0';
                        if (digit < 0 || digit > 9)
                        {
                            throw new ArgumentException("Contents should only contain digits, but got '" + content[i] + "'");
                        }
                        sum += digit;
                    }
                    sum *= 3;
                    for (int i = length - 2; i >= 0; i -= 2)
                    {
                        int digit = content[i] - '0';
                        if (digit < 0 || digit > 9)
                        {
                            throw new ArgumentException("Contents should only contain digits, but got '" + content[i] + "'");
                        }
                        sum += digit;
                    }
                    checksum = (1000 - sum) % 10;
                }
                else if (content.Length == 13)
                {
                    checksum = null;
                }
            }
            BarcodeData = content + checksum.ToString();
            writer.Options = new ZXing.Common.EncodingOptions
            {
                Height = 300,
                Width = 1000,
            };

            Bitmap barcodeImage = null;
            try
            {
               barcodeImage = writer.Write(BarcodeData);
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("텍스트의 길이가 12자리여야 합니다.");
                throw e;
            }
            //비트맵 변환
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)barcodeImage).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return new Tuple<BitmapImage, string>(image, BarcodeData);

        }
    }
}