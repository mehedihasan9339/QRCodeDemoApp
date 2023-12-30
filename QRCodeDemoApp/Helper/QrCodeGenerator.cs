using Microsoft.EntityFrameworkCore.Design.Internal;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;

namespace QRCodeDemoApp.Helper
{
    public class QrCodeGenerator
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public QrCodeGenerator(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string GenerateAndSaveQrCode(string data)
        {
            // Create a QR code writer
            var qrCodeWriter = new ZXing.QrCode.QRCodeWriter();

            // Encode the data into a BitMatrix
            var bitMatrix = qrCodeWriter.encode(data, ZXing.BarcodeFormat.QR_CODE, 300, 300);

            // Create a Bitmap from the BitMatrix
            var bitmap = BitMatrixToBitmap(bitMatrix);

            // Save the bitmap to a unique file in the wwwroot/QRCode folder
            string fileName = SaveImageToQRCodeFolder(bitmap);

            return fileName;
        }

        private Bitmap BitMatrixToBitmap(ZXing.Common.BitMatrix bitMatrix)
        {
            int width = bitMatrix.Width;
            int height = bitMatrix.Height;
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bitmap.SetPixel(x, y, bitMatrix[x, y] ? Color.Black : Color.White);
                }
            }

            return bitmap;
        }

        private string SaveImageToQRCodeFolder(Bitmap bitmap)
        {
            string wwwrootPath = _hostingEnvironment.WebRootPath;
            string qrCodeFolder = Path.Combine(wwwrootPath, "QRCode");

            if (!Directory.Exists(qrCodeFolder))
            {
                Directory.CreateDirectory(qrCodeFolder);
            }

            // Generate a unique file name based on the current timestamp
            string fileName = $"QrCode_{DateTime.Now:yyyyMMddHHmmssfff}.png";
            string filePath = Path.Combine(qrCodeFolder, fileName);

            // Save the image to the file in PNG format
            bitmap.Save(filePath, ImageFormat.Png);

            return fileName;
        }
    }
}
