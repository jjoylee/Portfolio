public class DepositService : IDepositService
{

    [Autowire]
    public IDepositConfirmService DepositConfirmService { get; set; }
    
    [Autowire]
    public IDepositSaveService DepositSaveService { get; set; }
    
    [Autowire]
    public IDepositProcInfoService DepositProcInfoService { get; set; }

    // 입금 처리
    public void ConfirmDeposit(int id, string orderIds, string memo, DepositMatchingInfo submitInfo, string adminId)
    {
        DepositConfirmService.ConfirmDeposit(id, orderIds, memo, submitInfo, adminId);
    }

    // 입금 메모, 주문번호 update
    public void SaveDeposit(int id, string orderIds, string memo)
    {
        DepositSaveService.SaveDeposit(id, orderIds, memo);
    }

    // prev 내용 가져오기
    public DepositProcInfo GetDepositProcInfo(int id, string orderIds)
    {
        return DepositProcInfoService.GetDepositProcInfo(id, orderIds);
    }
}