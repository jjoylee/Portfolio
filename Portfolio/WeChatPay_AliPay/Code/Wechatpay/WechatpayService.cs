namespace Lib.Payment.Wechatpay.Service
{
    public class WechatpayService : IWechatpayService
    {
        [Autowire]
        public IScanPayService ScanPayService { get; set; }

        [Autowire]
        public IWapPayService WapPayService { get; set; }

        public string ScanPay(int orderId)
        {
            return ScanPayService.Request(orderId);
        }

        public string WapPay(string host, int orderId)
        {
            return WapPayService.Request(host, orderId);
        }
    }
}
