public class DepositSaveService : IDepositSaveService
{
    [Autowire]
    public IDepositDao DepositDao { get; set; }

    public void SaveDeposit(int id, string orderIds, string memo)
    {
        DepositDao.SaveDeposit(id, orderIds, memo);
    }
}