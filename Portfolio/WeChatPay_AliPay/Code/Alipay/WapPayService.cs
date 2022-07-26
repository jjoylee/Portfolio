namespace Lib.Payment.Alipay.Service
{
    public class WapPayService : IWapPayService
    {
        [Autowire]
        public IOrderDao OrderDao { get; set; }
        [Autowire]
        public IPgResultDao PgResultDao { get; set; }
        [Autowire]
        public ICloseService CloseService { get; set; }
        [Autowire]
        public IAlipayGatewayService AlipayGatewayService { get; set; }

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
                throw new BizException("AlipayWapPay", e.Message);
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
            var outTradeNo = "outTradeNo"; // random으로 생성함
            var request = GetRequest(orderItem, host, outTradeNo);
            var response = AlipayGatewayService.Get(orderItem.SiteId).Execute(request);
            // request 정보 등 insert
            InsertPgResult(orderItem, request.GatewayData.ToXml(), response.ToJson(), outTradeNo); 
            return response.Url;
        }

        private WapPayRequest GetRequest(OrderItem orderItem, string host, string outTradeNo)
        {
            // 실제 사용한 데이터는 감추기 위해 임의로 넣음.
            var request = new WapPayRequest();
            request.AddGatewayData(new WapPayModel()
            {
                Body = "body",
                TotalAmount = orderItem.TotalAmount,
                Subject = "subject",
                OutTradeNo = outTradeNo,
                QuitUrl = $"quit.url"
            });
            //결제 후에 이동할 페이지
            request.ReturnUrl = $"return.url";
            return request;
        }

        // ... 생략
    }
}
