namespace Lib.Payment.Wechatpay.Service
{
    public class WapPayService : IWapPayService
    {
        [Autowire]
        public IWechatpayGatewayService WechatPayGatewayService { get; set; }

        [Autowire]
        public IOrderDao OrderDao { get; set; }

        [Autowire]
        public IPgResultDao PgResultDao { get; set; }

        [Autowire]
        public IOrderQueryService OrderQueryService { get; set; }

        [Autowire]
        public ICloseService CloseService { get; set; }

        [Transaction]
        public string Request(string host, int orderId)
        {
            try
            {
                var orderItem = OrderDao.FindById(orderId);
                CloseBeforeRequest(orderId, orderItem.SiteId);
                return NewRequest(orderItem, host);
            }
            catch (Exception e)
            {
                throw new BizException("WechatpayWapPay", e.Message);
            }
        }
        
        private void CloseBeforeRequest(int orderId, int siteId)
        {
            var beforeItem = PgResultDao.FindItem(orderId, siteId);
            if (beforeItem == null || string.IsNullOrEmpty(beforeItem.PaymentId)) return;
            CloseService.Request(beforeItem.PaymentId, orderId, siteId);
        }

        private string NewRequest(OrderItem orderItem, string host)
        {
            var outTradeNo = "outTradeNo" // random으로 생성함
            var request = GetRequest(host, outTradeNo, "body", orderItem.TotalAmount);
            var response = WechatPayGatewayService.Get(orderItem.SiteId).Execute(request);
            if (response.ReturnCode == "FAIL") throw new Exception($"支付失败，请重新尝试");
            if (response.ResultCode == "FAIL") throw new Exception($"支付失败，请重新尝试 ({response.ErrCodeDes})");
            // request 정보 등 insert
            InsertPgResult(orderItem, request.GatewayData.ToXml(), response.ToJson(), outTradeNo);
            var redirectParam = $"&redirect_url={HttpUtility.UrlEncode($"http://{host}/RedirectPage")}";
            return response.MwebUrl + redirectParam;
        }

        private WapPayRequest GetRequest(string host, string outTradeNo, string body, int totalAmount)
        {
            var request = new WapPayRequest();
            request.AddGatewayData(new WapPayModel()
            {
                Body = body,
                TotalAmount = totalAmount * 100,
                OutTradeNo = outTradeNo,
                SceneInfo = "SceneInfo"
            });
            return request;
        }

        //.. 생략
    }
}
