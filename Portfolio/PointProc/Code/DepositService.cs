public class DepositService : IDepositService
{

    [Autowire]
    public IDepositConfirmService DepositConfirmService { get; set; }
    
    [Autowire]
    public IDepositSaveService DepositSaveService { get; set; }
    
    [Autowire]
    public IDepositProcInfoService DepositProcInfoService { get; set; }

    public void ConfirmDeposit(int id, string orderIds, string memo, DepositMatchingInfo submitInfo, string adminId)
    {
        DepositConfirmService.ConfirmDeposit(id, orderIds, memo, submitInfo, adminId);
    }

    public void SaveDeposit(int id, string orderIds, string memo)
    {
        DepositSaveService.SaveDeposit(id, orderIds, memo);
    }

    public DepositProcInfo GetDepositProcInfo(int id, string orderIds)
    {
        return DepositProcInfoService.GetDepositProcInfo(id, orderIds);
    }
}