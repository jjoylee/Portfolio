namespace Lib.Payment.Wechatpay.Service
{
    public class OrderCompleteCheckService : IOrderCompleteCheckService
    {
        [Autowire]
        public IOrderQueryService OrderQueryService { get; set; }

        [Autowire]
        public IPgResultDao PgResultDao { get; set; }

        [Autowire]
        public IOrderDao OrderDao { get; set; }

        public bool Check(int orderId)
        {
            var orderItem = OrderDao.FindById(orderId);
            var beforeItem = PgResultDao.FindItem(orderId, orderItem.SiteId);
            if (beforeItem == null || string.IsNullOrEmpty(beforeItem.PaymentId)) return false;
            var response = OrderQueryService.Request(beforeItem.PaymentId, orderId, orderItem.SiteId);
            return response.TradeState == "SUCCESS";
        }
    }
}
