// 입금 처리 화면에서 보여줄 정보
public class DepositProcInfo
{
    public DepositMatchingInfo DepositMatchingInfo { get; set; } // 총 주문금액, 고객 보유 포인트
    public ProcType ProcType { get; set; } // 입금 처리 CASE
    public int ReturnPoint { get; set; } // 돌려줄 포인트 (총 주문금액보다 입금액이 많을 때)
    public int DeductionPoint { get; set; } // 차감할 포인트 (총 주문금액보다 입금액이 적을 떄)
}