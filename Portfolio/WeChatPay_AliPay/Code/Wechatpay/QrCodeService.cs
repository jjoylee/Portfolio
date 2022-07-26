namespace Lib.Payment.Wechatpay.Service
{
    public class QrCodeService : IQrCodeService
    {
        public string ToBase64(string codeUrl)
        {
            var image = MakeQrCodeImage(codeUrl);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                byte[] _imageBytes = ms.ToArray();
                var base64String = Convert.ToBase64String(_imageBytes);
                return "data:image/png;base64," + base64String;
            }
        }

        private Bitmap MakeQrCodeImage(string codeUrl)
        {
            var qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;
            return qrCodeEncoder.Encode(codeUrl, Encoding.Default);
        }
    }
}