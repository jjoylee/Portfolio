    public class DepositProcInfoService : IDepositProcInfoService
    {
        [Autowire]
        public IDepositProcInfoGetter DepositProcInfoGetter { get; set; }

        public DepositProcInfo GetDepositProcInfo(int id, string orderIds)
        {
            return DepositProcInfoGetter.Get(id, orderIds);
        }
    }