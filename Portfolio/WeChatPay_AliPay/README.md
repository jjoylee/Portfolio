
<br>

## 📌 위챗페이, 알리페이 결제 연동

위챗페이, 알리페이 결제 라이브러리 제공받아 연동  
모바일 브라우저에서는 앱으로 이동해 결제할 수 있다. (WapPay)  
PC에서 결제할 때는 화면에 표시되는 QR Code를 스캔해 결제할 수 있다.

<br>

## 📌 주요 코드


### 알리페이 결제 페이지 표시

```C#
    public ActionResult OrderStart(int orderId)
    {
        //.. 생략
        //시작점
        if(MobileUtil.IsMobileBrowsers(Request)) return Redirect($"/Pg/AlipayWapPayPage?orderId={orderItem.Id}");
        return Redirect($"/Pg/AlipayWebPayPage?orderId={orderItem.Id}");
    }
```

[WebPayService.cs](./Code/Alipay/WebPayService.cs)

```C#
    public string Request(string host, int orderId)
    {
        try
        {
            var orderItem = OrderDao.FindById(orderId);
            // 알리페이 시스템에 기존에 생성한 주문 데이터 있으면 close
            CloseBeforeRequest(orderId, orderItem.SellerJoinerId);
            // 알리페이 시스템에 새로운 주문 생성
            return NewRequest(orderItem, host);
        }
        catch (Exception e)
        {
           throw new BizException("AlipayWebPay", e.Message);
        }
    }

```

[PgController.cs](./Code/Controller/PgController.cs)

```C#
    // 알리페이 결제방법 선택하면 여기로 redirect
    public ActionResult AlipayWebPayPage(int orderId)
    {
        try
        {
            // 알리페이 api로 html string 받아서 load(qrcode 표시)
            var html = AlipayService.WebPay(Request.Url.Host, orderId);
            return Content(html, "text/html", Encoding.UTF8);
        }
        catch (BizException e)
        {
            return View("Error", model: e.Message);
        }
    }
```

<br>

### 위챗페이의 경우 QrCode 직접 생성

[QrCodeService.cs](./Code/Wechatpay/QrCodeService.cs)

```C#
    // 위챗페이 api로 전달받은 codeUrl 사용
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
        //.. qrCodeEncoder 설정 생략
        return qrCodeEncoder.Encode(codeUrl, Encoding.Default);
    }
```

<br>

### 결제 완료 후 주문 결제 상태 변경

[NotifyService.cs](./Code/Alipay/NotifyService.cs)

```C#
    // notifyUrl 설정을 기반으로 결제 완료 noti가 오는 곳에서 해당 서비스 호출
    int fee = ((int)notifyResponse.TotalAmount);
    if (orderItem.TotalAmount != fee)
    {
        Logger.Debug($"{orderItem.Id} 주문의 가격과 노티의 가격이 다릅니다.");
        return true;
    }

    // 입금 처리해도 괜찮으면 response 데이터를 db에 넣는다.
    // 주문 상태를 결제 완료로 변경
    InsertPgResultItem(orderItem, fee, outTradeNo, json); 
    return true;    
```

<br>

[주요 코드 링크](./Code)

<br>

## 📌 결과

<img src="./Image/alipay.gif" width="700" height="400">
