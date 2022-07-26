namespace Lib.Payment.Alipay.Service
{
    public class CloseService : ICloseService
    {
        [Autowire]
        public IOrderQueryService OrderQueryService { get; set; }

        [Autowire]
        public IAlipayGatewayService AlipayGatewayService { get; set; }

        public void Request(string outTradeNo, int orderId, int siteId)
        {
            if (!CanClose(outTradeNo, orderId, siteId)) return;
            var request = GetRequest(outTradeNo);
            var response = AlipayGatewayService.Get(siteId).Execute(request);
            if (response.Code != "10000") throw new Exception($"支付失败，请重新尝试");
        }

        private bool CanClose(string outTradeNo, int orderId, int siteId)
        {
            var reponse = OrderQueryService.Request(outTradeNo, orderId, siteId);
            if (reponse.TradeStatus == "TRADE_SUCCESS") throw new Exception("已完成支付");
            return reponse.TradeStatus == "WAIT_BUYER_PAY";
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
