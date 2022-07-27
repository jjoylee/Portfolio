namespace Lib.Payment.Wechatpay.Service
{
    // PC에서 위챗페이로 결제할 때 사용
    public class ScanPayService : IScanPayService
    {
        [Autowire]
        public IOrderDao OrderDao { get; set; }

        [Autowire]
        public IPgResultDao PgResultDao { get; set; }

        [Autowire]
        public IWechatpayGatewayService WechatpayGatewayService { get; set; }

        [Autowire]
        public IOrderQueryService OrderQueryService { get; set; }

        [Autowire]
        public ICloseService CloseService { get; set; }
        
        public string Request(int orderId)
        {
            try
            {
                var orderItem = OrderDao.FindById(orderId);
                // 위챗페이 시스템에 기존에 생성한 주문 데이터 있으면 close
                CloseBeforeRequest(orderId, orderItem.SiteId);
                // 위챗페이 시스템에 새로운 주문 생성
                return NewRequest(orderItem);
            }
            catch (Exception e)
            {
                throw new BizException("WechatpayScanPay", e.Message);
            }
        }

        private void CloseBeforeRequest(int orderId, int siteId)
        {
            var beforeItem = PgResultDao.FindItem(orderId, siteId);
            if (beforeItem == null || string.IsNullOrEmpty(beforeItem.PaymentId)) return;
            CloseService.Request(beforeItem.PaymentId, orderId, siteId); 
        }

        private string NewRequest(OrderItem orderItem)
        {
            var outTradeNo = "outTradeNo"; // random으로 생성함
            var request = GetReqeust(outTradeNo, "body", orderItem.TotalAmount);
            var response = WechatpayGatewayService.Get(orderItem.SiteId).Execute(request);
            if (response.ReturnCode == "FAIL") throw new Exception("二维码生成失败，请重新尝试");
            if (response.ResultCode == "FAIL") throw new Exception($"二维码生成失败，请重新尝试 ({response.ErrCodeDes})");
            // request 정보 등 insert
            InsertPgResult(orderItem, request.GatewayData.ToXml(), response.ToJson(), outTradeNo);
            return response.CodeUrl;
        }

        private ScanPayRequest GetReqeust(string outTradeNo, string body, int totalAmount)
        {
            var request = new ScanPayRequest();
            request.AddGatewayData(new ScanPayModel()
            {
                Body = body,
                TotalAmount = totalAmount * 100,
                OutTradeNo = outTradeNo
            });
            return request;
        }

        //.. 생략
    }
}
