namespace Lib.Payment.Wechatpay.Service
{
    // 위챗페이 시스템에서 해당 주문 상태 조회
    public class OrderQueryService : IOrderQueryService
    {
        [Autowire]
        public IWechatpayGatewayService WechatPayGatewayService { get; set; }
        [Autowire]
        public IPayStateSyncService PayStateSyncService { get; set; }

        public QueryResponse Request(string outTradeNo, int orderId, int siteId)
        {
            try
            {
                var request = GetRequest(outTradeNo);
                var response = WechatPayGatewayService.Get(siteId).Execute(request);
                if (response.ReturnCode == "FAIL") throw new Exception("失败，请重新尝试");
                if (response.ResultCode == "FAIL") throw new Exception($"失败，请重新尝试({response.ErrCodeDes})");
                if (response.TradeState == "SUCCESS") PayStateSyncService.Sync(outTradeNo, response.ToJson(), orderId, (response.TotalAmount / 100));
                return response;
            }
            catch (Exception e)
            {
                throw new BizException("OrderQueryService", e.Message);
            }
        }

        private QueryRequest GetRequest(string outTradeNo)
        {
            var request = new QueryRequest();
            request.AddGatewayData(new QueryModel()
            {
                OutTradeNo = outTradeNo
            });
            return request;
        }
    }
}
