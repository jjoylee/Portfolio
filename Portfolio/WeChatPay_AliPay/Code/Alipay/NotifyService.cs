namespace Lib.Payment.Alipay.Service
{
    // notifyUrl 설정을 기반으로 결제 완료 noti가 오는 곳에서 해당 서비스 호출
    public class NotifyService : INotifyService
    {
        [Autowire]
        public IOrderDao OrderDao { get; set; }

        [Autowire]
        public IAlipayGatewayService AlipayGatewayService { get; set; }

        [Autowire]
        public IPgResultDao PgResultDao { get; set; }

        public void Notify()
        {
            Notify notify = new Notify(AlipayGatewayService.GetAll());
            notify.PaySucceed += Notify_PaySucceed;
            notify.RefundSucceed += Notify_RefundSucceed;
            notify.UnknownNotify += Notify_UnknownNotify;
            notify.UnknownGateway += Notify_UnknownGateway;
            notify.Received();
        }

        private bool Notify_PaySucceed(object sender, PaySucceedEventArgs e)
        {
            string json = new JavaScriptSerializer().Serialize(e.NotifyResponse);
            Logger.Debug($"Notify_PaySucceed - {json}");

            var notifyResponse = ((NotifyResponse)e.NotifyResponse);
            var outTradeNo = notifyResponse.OutTradeNo;

            var pgResultItem = PgResultDao.FindItemByPaymentId(outTradeNo);
            if (pgResultItem == null)
            {
                Logger.Debug($"{outTradeNo}는 없는 outTradeNo입니다.");
                return true;
            }

            var orderItem = OrderDao.FindById(pgResultItem.OrderId);

            if (orderItem.IsPaid())
            {
                Logger.Debug("이미 지불된 주문입니다.");
                return true;
            }

            int fee = ((int)notifyResponse.TotalAmount);
            if (orderItem.TotalAmount != fee)
            {
                Logger.Debug($"{orderItem.Id} 주문의 가격과 노티의 가격이 다릅니다.");
                return true;
            }

            InsertPgResultItem(orderItem, fee, outTradeNo, json); // 데이터 insert 후 주문 입금 완료로 상태 변경
            Logger.Debug(json);
            return true;
        }

        // ... 생략 
    }
}
