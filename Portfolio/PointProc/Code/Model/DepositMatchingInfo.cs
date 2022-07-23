 public class DepositMatchingInfo
{
    public int CurrentMemberPoint { get; set; }
    public int TotalOrderAmount { get; set; }
    public int DepositProcPrice { get; set; }
    public string MemberId { get; set; }

    public bool InfoIsSame(DepositMatchingInfo submitInfo)
    {
        return this.CurrentMemberPoint == submitInfo.CurrentMemberPoint
            && this.TotalOrderAmount == submitInfo.TotalOrderAmount
            && this.DepositProcPrice == submitInfo.DepositProcPrice;
    }
}