 // 내역과 입금할 주문 데이터 매칭을 위함
 public class DepositMatchingInfo
{
    public int CurrentMemberPoint { get; set; } // 주문 고객이 보유하고 있는 포인트
    public int TotalOrderAmount { get; set; } // 총 주문금액
    public int DepositProcPrice { get; set; } // 입금액
    public string MemberId { get; set; } // 해당 주문 고객 아이디

    // 기존에 조회했던 정보가 변하지 않았는지 체크하기 위함
    public bool InfoIsSame(DepositMatchingInfo submitInfo)
    {
        return this.CurrentMemberPoint == submitInfo.CurrentMemberPoint
            && this.TotalOrderAmount == submitInfo.TotalOrderAmount
            && this.DepositProcPrice == submitInfo.DepositProcPrice;
    }
}