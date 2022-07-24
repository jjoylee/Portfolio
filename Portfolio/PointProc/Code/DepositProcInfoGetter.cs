public class DepositProcInfoGetter : IDepositProcInfoGetter
{
    // ... 생략 
        
    [Autowire]
    public IDepositMatchingInfoGetter DepositMatchingInfoGetter { get; set; }

    public DepositProcInfo Get(int id, string orderIds)
    {
        var matchingInfo = DepositMatchingInfoGetter.Get(id, orderIds); // 총 주문금액, 고객이 보유한 포인트, 입금액 가져오기
       return GetProcInfo(matchingInfo); // CASE분석 후, 지급할 포인트, 차감할 포인트 계산
    }

    private DepositProcInfo GetProcInfo(DepositMatchingInfo matchingInfo)
    {
        var procType = GetProcType(matchingInfo); // CASE 분석
        var returnPoint = GetReturnPoint(procType, matchingInfo); // 반환할 포인트
        var deductionPoint = GetDeductionPoint(procType, matchingInfo); // 차감할 포인트
        return new DepositProcInfo() { DepositMatchingInfo = matchingInfo , ProcType = procType, ReturnPoint = returnPoint, DeductionPoint = deductionPoint};
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

    private int GetReturnPoint(ProcType procType, DepositMatchingInfo info)
    {
        return (ProcType.RETURNPOINTPROC == procType) ? info.DepositProcPrice - info.TotalOrderAmount : 0;
    }

    private int GetDeductionPoint(ProcType procType, DepositMatchingInfo info)
    {
        return (ProcType.CANNOTPROC == procType || ProcType.DEDUCTIONPOINTPROC == procType) ? info.TotalOrderAmount - info.DepositProcPrice : 0; 
    }   
}