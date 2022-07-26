
<br>

## 📌 위챗페이, 알리페이 결제 연동

위챗페이, 알리페이 결제 라이브러리 제공받아 사이트에 연동  


## 

위챗에서 전달받은 codeUrl로 qrcode만들어 화면에 표시하기

``` C#
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
      // QRCodeEncoder 설정 생략
      return qrCodeEncoder.Encode(codeUrl, Encoding.Default);
  }
```

<br>

## 📌 결과

<img src="./Image/alipay.gif" width="700" height="400">
