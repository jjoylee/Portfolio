public class DepositConfirmService : IDepositConfirmService
{
    [Autowire]
    public IDepositProcInfoService DepositProcInfoService { get; set; }
    
    /// <summary>
    /// 입금확인 로직
    /// 포인트 처리 및 validation 체크 후 입금처리
    /// </summary>

    [Transaction]
    public void ConfirmDeposit(int id, string orderIds, string memo, DepositMatchingInfo submitInfo, string adminId)
    {
        DepositConfirmOrders(id, orderIds, submitInfo, adminId);
        DepositDao.ConfirmDeposit(id, orderIds, memo, adminId);  
    }

    private void DepositConfirmOrders(int id, string orderIds, DepositMatchingInfo submitInfo, string adminId)
    {
        var currentInfo = DepositProcInfoService.GetDepositProcInfo(id, orderNums);
        if (InfoIsChanged(currentInfo, submitInfo)) throw new Exception("주문을 확인해주세요.");
        DoDepositConfirmProc(orderIds, adminId);
        DoPointProc(currentInfo, id, orderIds.Split('/')[0]);
    }

    private void DoDepositConfirmProc(string orderIds, string adminId)
    {
        foreach (var orderId in orderIds.Split('/'))
        {
            DepositConfirm(Convert.ToInt32(orderId), adminId);
        }
    }

    private bool InfoIsChanged(DepositProcInfo currentInfo, DepositMatchingInfo submitInfo)
    {
        return !currentInfo.DepositMatchingInfo.InfoIsSame(submitInfo);
    }

    protected virtual void DoPointProc(DepositProcInfo info, int id, string orderId)
    {
        var memberId = info.DepositMatchingInfo.MemberId;
        if (info.ProcType == ProcType.DEDUCTIONPOINTPROC) AlterPoint(memberId,"OUT", info.DeductionPoint);
        if (info.ProcType == ProcType.RETURNPOINTPROC) AlterPoint(memberId, "IN", info.ReturnPoint);
    }

    // ... 생략
}