namespace Lib.Payment.Alipay.Service
{
    public class AlipayGatewayService : IAlipayGatewayService
    {
        [Autowire]
        public ISitePgInfoDao SitePgInfoDao { get; set; }

        public AlipayGateway Get(int siteId)
        {
            var item = SitePgInfoDao.FindItemByPayTypeAndSiteId("ALIPAY", siteId);
            if (item == null || string.IsNullOrEmpty(item.Key)) throw new Exception("No Alibaba Configuration Data");
            var dataDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Key);
            var alipayMerchant = new Merchant
            {
                AppId = dataDic["appid.key"],
                NotifyUrl = "notify.url",
                ReturnUrl = dataDic["returnUrl.key"],
                AlipayPublicKey = dataDic["alipaypublickey.key"],
                Privatekey = dataDic["privatekey.key"]
            };

            return new AlipayGateway(alipayMerchant)
            {
                GatewayUrl = "aliplay.gateway.url";
            };
        }

        public Gateways GetAll()
        {
            var _gateways = new Gateways();
            var list = SitePgInfoDao.FindAlipayKeyList();
            foreach (var item in list)
            {
                if (item == null || string.IsNullOrEmpty(item.AppId)) continue;
                var alipayMerchant = new Merchant
                {
                    AppId = item.AppId,
                    AlipayPublicKey = item.AlipayPublicKey,
                    Privatekey = item.Privatekey
                };
                _gateways.Add(new AlipayGateway(alipayMerchant)
                {
                    GatewayUrl = "aliplay.gateway.url";
                });
            }
            return _gateways;
        }
    }
}
