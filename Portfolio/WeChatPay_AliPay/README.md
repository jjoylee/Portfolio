
<br>

## 📌 위챗페이, 알리페이 결제 연동

위챗페이, 알리페이 결제 라이브러리 제공받아 사이트에 연동  
모바일에서 사용할 떄는 앱으로 이동해서 결제 할 수 있다.  
웹에서 결제할 때는 화면에 표시되는 QRCode를 스캔해 결제할 수 있다.

<br>

## 📌 주요 코드

```C#
        public string Request(int orderId)
        {
            try
            {
                var orderItem = OrderDao.FindById(orderId);
                CloseBeforeRequest(orderId, orderItem.SiteId);
                return NewRequest(orderItem);
            }
            catch (Exception e)
            {
                throw new BizException("WechatpayScanPay", e.Message);
            }
        }
```

[QrCodeService.cs](./Code/Wechatpay/QrCodeService.cs)

``` C#
  // 위챗에서 전달받은 codeUrl로 qrcode만들어 화면에 표시하기
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

[주요 코드 링크](./Code)

<br>

## 📌 결과

<img src="./Image/alipay.gif" width="700" height="400">
