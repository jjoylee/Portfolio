namespace Lib.Payment.Wechatpay.Service
{
    // 위챗페이 환불을 위한 api
    public class RefundService : IRefundService
    {
        [Autowire]
        public IOrderDao OrderDao { get; set; }

        [Autowire]
        public IPgResultDao PgResultDao { get; set; }

        [Autowire]
        public IOrderQueryService OrderQueryService { get; set; }

        [Autowire]
        public IWechatpayGatewayService WechatPayGatewayService { get; set; }

        public void Request(int orderId, int siteId, int refundPrice)
        {
            try
            {
                var outRefundNo = "outRefundNo"; // random으로 생성함
                var request = GetRequest(outRefundNo, orderId, siteId, refundPrice);
                var response = WechatPayGatewayService.Get(siteId).Execute(request);
                if (response.ReturnCode == "FAIL") throw new Exception($"退款失败，请重新尝试");
                if (response.ResultCode == "FAIL") throw new Exception($"退款失败，请重新尝试 ({response.ErrCodeDes})");
            }
            catch (Exception ex)
            {
                throw new BizException("WechatpayRefund", ex.Message);
            }
        }

        private RefundRequest GetRequest(string outRefundNo, int orderId, int siteId, int refundPrice)
        {
            var item = PgResultDao.FindItem(orderId, siteId, "PAID");
            if (item == null) throw new Exception("您还没有已付款记录");
            var orderQueryResponse = OrderQueryService.Request(item.PaymentId, orderId, siteId);
            var request = new RefundRequest();
            request.AddGatewayData(new RefundModel()
            {
                RefundAmount = refundPrice * 100,
                OutRefundNo = outRefundNo,
                TotalAmount = orderQueryResponse.TotalAmount,
                OutTradeNo = item.PaymentId
            });
            return request;
        }
    }
}