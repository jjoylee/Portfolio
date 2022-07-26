namespace Lib.Payment.Alipay.Service
{
    public class WebPayService : IWebPayService
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
                CloseBeforeRequest(orderId, orderItem.SellerJoinerId);
                return NewRequest(orderItem, host);
            }
            catch (Exception e)
            {
                throw new BizException("AlipayWebPay", e.Message);
            }
        }

        private void CloseBeforeRequest(int orderId, int siteId)
        {
            var beforeItem = PgResultDao.FindItem(orderId, siteId);
            if (beforeItem == null || string.IsNullOrEmpty(beforeItem.PaymentId)) return;
            CloseService.Request(beforeItem.PaymentId, orderId, joinerId);
        }

        private string NewRequest(OrderItem orderItem, string host)
        {
            var outTradeNo = "outTradeNo"; // random으로 생성함
            var request = GetRequest(orderItem, host, outTradeNo);
            var response = AlipayGatewayService.Get(orderItem.SiteId).Execute(request);
            // request 정보 등 insert
            InsertPgResult(orderItem, request.GatewayData.ToXml(), response.ToJson(), outTradeNo);
            return response.Html;
        }

        private WebPayRequest GetRequest(OrderItem orderItem, string host, string outTradeNo)
        {
            var request = new WebPayRequest();
            request.AddGatewayData(new WebPayModel()
            {
                Body = "body",
                TotalAmount = orderItem.TotalAmount,
                Subject = "subejct",
                OutTradeNo = outTradeNo
            });
            //결제 후에 이동할 페이지
            request.ReturnUrl = $"returnUrl";
            return request;
        }

        // .. 생략
    }
}
