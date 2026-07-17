using System.Drawing;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace XrayGUI
{
    public class QRCodeReader
    {
        public string? ReadFromFile(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException(imagePath);

            using Bitmap bitmap = new Bitmap(imagePath);

            var luminanceSource = new BitmapLuminanceSource(bitmap);

            var reader = new BarcodeReaderGeneric
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true
                }
            };

            var result = reader.Decode(luminanceSource);

            return result?.Text;
        }
    }
}
