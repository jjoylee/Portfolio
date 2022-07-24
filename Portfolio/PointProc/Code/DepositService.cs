public class DepositService : IDepositService
{

    [Autowire]
    public IDepositConfirmService DepositConfirmService { get; set; }
    
    [Autowire]
    public IDepositSaveService DepositSaveService { get; set; }
    
    [Autowire]
    public IDepositProcInfoService DepositProcInfoService { get; set; }

    /* 
        submitInfo 기준으로 입금 처리
        입금 처리 시작 포인트
    */
    public void ConfirmDeposit(int id, string orderIds, string memo, DepositMatchingInfo submitInfo, string adminId)
    {
        DepositConfirmService.ConfirmDeposit(id, orderIds, memo, submitInfo, adminId);
    }

    // 입금 메모, 주문번호 update
    public void SaveDeposit(int id, string orderIds, string memo)
    {
        DepositSaveService.SaveDeposit(id, orderIds, memo);
    }

    /** 
        입금 처리 전 주문금액, 고객 포인트 조회, 입금 처리 가능한지 상태 파악 (적용버튼 클릭)
        적용 버튼을 누르면 DepositProcInfo 객체를 만들어 return
        입금 처리할 때(처리완료/포인트처리 버튼 클릭) ConfirmDeposit의 submitInfo로 넘김
        GetDepositProcInfo → ConfirmDeposit
    **/
    public DepositProcInfo GetDepositProcInfo(int id, string orderIds)
    {
        return DepositProcInfoService.GetDepositProcInfo(id, orderIds);
    }
}
