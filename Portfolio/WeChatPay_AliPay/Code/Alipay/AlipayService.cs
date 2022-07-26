namespace Lib.Payment.Alipay.Service
{
    public class AlipayService : IAlipayService
    {
        [Autowire]
        public IWebPayService WebPayService { get; set; }

        [Autowire]
        public IWapPayService WapPayService { get; set; }

        public string WebPay(string host, int orderId)
        {
            return WebPayService.Request(host, orderId);
        }

        public string WapPay(string host, int orderId)
        {
            return WapPayService.Request(host, orderId);
        }
    }
}
