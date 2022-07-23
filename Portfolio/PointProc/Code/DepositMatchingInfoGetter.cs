public class DepositMatchingInfoGetter : IDepositMatchingInfoGetter
{
    [Autowire]
    public IOrderDao OrderDao { get; set; }

    [Autowire]
    public IMemberDao MemberDao { get; set; }

    [Autowire]
    public IDepositDao DepositDao { get; set; }

    public DepositMatchingInfo Get(int id, string orderIds)
    {
        var depositTargetOrderList = GetDepositTargetOrderList(orderIds);
        var memberId = GetMemberId(depositTargetOrderList);
        var currentMemberPoint = GetCurrentPointByMemberId(memberId);
        var totalOrderAmount = GetTotalOrderAmount(depositTargetOrderList);
        var depositProcPrice = GetDepositProcPrice(id);
        return new DepositMatchingInfo  {
            TotalOrderAmount = totalOrderAmount, CurrentMemberPoint = currentMemberPoint,
            DepositProcPrice = depositProcPrice, MemberId = memberId
        };
    }

    private int GetCurrentPointByMemberId(string memberId)
    {
        return MemberDao.GetCurrentPointByMemberId(memberId);
    }

    private int GetDepositProcPrice(int id)
    {
        var deposit = DepositDao.GetNotConfirmedDepositById(id);
        if (deposit == null) throw new Exception("주문을 확인해주세요");        
        return deposit.ProcAmt;
    }

    private int GetTotalOrderAmount(IList<OrderItem> depositTargetOrderList)
    {
        return depositTargetOrderList.Sum(t => t.OrderAmount);
    }

    private string GetMemberId(IList<OrderItem> orderList)
    {
        var memberList = GetMemberList(orderList);
        if (MemberIsNotSame(memberList)) throw new TqoonBizException("다중 선택한 주문 건의 주문자가 동일해야 합니다.");
        return memberList[0];
    }

    private bool MemberIsNotSame(IList<string> memberList)
    {
        return memberList.Count() != 1;
    }

    private IList<string> GetUserList(IList<OrderItem> orderList)
    {
        return orderList.Select(t => t.MemberId).Distinct().ToList();
    }

    private IList<OrderItem> GetDepositTargetOrderList(string orderIds)
    {
        var ids = orderIds.Split('/').Select(t => Convert.ToInt32(t)).ToArray();
        var orderList = OrderDao.FindDepositConfirmTargetByOrderIds(ids);
        if (orderList.Count() != ids.Count()) throw new Exception("DepositConfirmValidation");
        return orderList;
    }
}