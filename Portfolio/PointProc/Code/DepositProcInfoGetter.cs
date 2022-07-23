    public class DepositProcInfoGetter : IDepositProcInfoGetter
    {
        // ... 생략 
        
        [Autowire]
        public IDepositMatchingInfoGetter DepositMatchingInfoGetter { get; set; }

        public DepositProcInfo Get(int id, string orderIds)
        {
            var matchingInfo = DepositMatchingInfoGetter.Get(id, orderIds);
            return GetProcInfo(matchingInfo);
        }

        private DepositProcInfo GetProcInfo(DepositMatchingInfo matchingInfo)
        {
            var procType = GetProcType(matchingInfo);
            var returnPoint = GetReturnPoint(procType, matchingInfo);
            var deductionPoint = GetDeductionPoint(procType, matchingInfo);
            return new DepositProcInfo() { DepositMatchingInfo = matchingInfo , ProcType = procType, ReturnPoint = returnPoint, DeductionPoint = deductionPoint};
        }

        private int GetDeductionPoint(ProcType procType, DepositMatchingInfo info)
        {
            return (ProcType.CANNOTPROC == procType || ProcType.DEDUCTIONPOINTPROC == procType) ? info.TotalOrderAmount - info.DepositProcPrice : 0; 
        }

        private int GetReturnPoint(ProcType procType, DepositMatchingInfo info)
        {
            return (ProcType.RETURNPOINTPROC == procType) ? info.DepositProcPrice - info.TotalOrderAmount : 0;
        }

        private ProcType GetProcType(DepositMatchingInfo info)
        {
            if (info.DepositProcPrice == info.TotalOrderAmount) return ProcType.GENERALPROC;
            if (info.TotalOrderAmount > info.DepositProcPrice)
            {
                if (info.TotalOrderAmount <= (info.DepositProcPrice + info.CurrentUserPoint)) return ProcType.DEDUCTIONPOINTPROC;
                return ProcType.CANNOTPROC;
            }
            return ProcType.RETURNPOINTPROC;
        }
    }