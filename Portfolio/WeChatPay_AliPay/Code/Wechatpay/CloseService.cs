namespace Lib.Payment.Wechatpay.Service
{
    public class CloseService : ICloseService
    {
        [Autowire]
        public IWechatpayGatewayService WechatpayGatewayService { get; set; }
        [Autowire]
        public IOrderQueryService OrderQueryService { get; set; }

        public void Request(string outTradeNo, int orderId, int siteId)
        {
            if (!CanClose(outTradeNo, orderId, siteId)) return;
            var request = GetRequest(outTradeNo);
            var response = WechatpayGatewayService.Get(joinerId).Execute(request);
            if (response.ReturnCode == "FAIL") throw new Exception($"支付失败，请重新尝试");
            if (response.ResultCode == "FAIL") throw new Exception($"支付失败，请重新尝试 ({response.ErrCodeDes})");
        }

        private bool CanClose(string outTradeNo, int orderId, int siteId)
        {
            var reponse = OrderQueryService.Request(outTradeNo, orderId, siteId);
            if (reponse.TradeState == "SUCCESS") throw new Exception("已完成支付");
            if (reponse.TradeState == "REFUND") throw new Exception("已退款订单");
            return reponse.TradeState != "CLOSED";
        }

        private CloseRequest GetRequest(string outTradeNo)
        {
            var request = new CloseRequest();
            request.AddGatewayData(new CloseModel()
            {
                OutTradeNo = outTradeNo
            });
            return request;
        }
    }
}
