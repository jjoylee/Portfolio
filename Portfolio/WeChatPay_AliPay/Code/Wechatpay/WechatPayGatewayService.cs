namespace Lib.Payment.Wechatpay.Service
{
    public class WechatpayGatewayService : IWechatpayGatewayService
    {
        [Autowire]
        public ISitePgInfoDao SitePgInfoDao { get; set; }

        public WechatpayGateway Get(int joinerId)
        {
            var item = SitePgInfoDao.FindItemByPayTypeAndSiteId("WECHATPAY", joinerId);
            if (item == null || string.IsNullOrEmpty(item.Key)) throw new Exception("No Wechat Configuration Data");
            var dataDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Key);
            var wechatpayMerchant = new Merchant
            {

                AppId = dataDic["appid.key"],
                MchId = dataDic["mchid.ky"],
                Key = dataDic["key.key"],
                AppSecret = dataDic["appsecret.key"],

                //인증서
                SslCertPath = HttpContext.Current.Server.MapPath($"sslcertpath"),
                SslCertPassword = dataDic["sslcertpassword.key"],
                NotifyUrl = "notify.url" 
            };
            return new WechatpayGateway(wechatpayMerchant);
        }

        public Gateways GetAll()
        {
            var _gateways = new Gateways();
            var list = SitePgInfoDao.FindWechatpayKeyList();
            foreach (var item in list)
            {
                if (item == null || string.IsNullOrEmpty(item.AppId)) continue;
                var wechatpayMerchant = new Merchant
                {
                    AppId = item.AppId,
                    MchId = item.MchId,
                    Key = item.Key
                };
                _gateways.Add(new WechatpayGateway(wechatpayMerchant));
            }
            return _gateways;
        }
    }
}
