namespace Project.Areas.Page.Controllers
{
    public class PgController : AbstractPageController
    {
        [Autowire]
        public IWechatpayService WechatpayService { get; set; }

        [Autowire]
        public IAlipayService AlipayService { get; set; }

        [Autowire]
        public IQrCodeService QrCodeService { get; set; }

        //.. 생략

        public ActionResult WechatpayScanPay(int orderId)
        {
            try
            {
                var codeUrl = WechatpayService.ScanPay(orderId);
                var image = QrCodeService.ToBase64(codeUrl);
                return Json(new { image });
            }
            catch (Exception e)
            {
                throw new BizException("QrCode", e.Message);
            }
        }

        public ActionResult WechatpayWapPayPage(int orderId)
        {
            try
            {
                var mWebUrl = WechatpayService.WapPay(Request.Url.Host, orderId);
                return Redirect(mWebUrl);
            }
            catch (BizException e)
            {
                return View("Error", model: e.Message);
            }
        }

        public ActionResult AlipayWebPayPage(int orderId)
        {
            try
            {
                var html = AlipayService.WebPay(Request.Url.Host, orderId);
                return Content(html, "text/html", Encoding.UTF8);
            }
            catch (BizException e)
            {
                return View("Error", model: e.Message);
            }
        }

        public ActionResult AlipayWapPayPage(int orderId)
        {
            try
            {
                var redirectUrl = AlipayService.WapPay(Request.Url.Host, orderId);
                return Redirect(redirectUrl);
            }
            catch (TqoonBizException e)
            {
                return View("Error", model: e.Message);
            }
        }

        //.. 생략
    }
}