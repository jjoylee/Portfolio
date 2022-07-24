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
        var depositTargetOrderList = GetDepositTargetOrderList(orderIds); // 해당 입금 내역으로 입금처리할 주문들
        var memberId = GetMemberId(depositTargetOrderList); // 주문의 고객
        var currentMemberPoint = GetCurrentPointByMemberId(memberId); // 현재 보유 포인트
        var totalOrderAmount = GetTotalOrderAmount(depositTargetOrderList); // 총 주문금액
        var depositProcPrice = GetDepositProcPrice(id); // 입금액, id == 입금내역 id
        return new DepositMatchingInfo  {
            TotalOrderAmount = totalOrderAmount, CurrentMemberPoint = currentMemberPoint,
            DepositProcPrice = depositProcPrice, MemberId = memberId
        };
    }

    private IList<OrderItem> GetDepositTargetOrderList(string orderIds)
    {
        var ids = orderIds.Split('/').Select(t => Convert.ToInt32(t)).ToArray();
        var orderList = OrderDao.FindDepositConfirmTargetByOrderIds(ids);
        if (orderList.Count() != ids.Count()) throw new Exception("DepositConfirmValidation");
        return orderList;
    }

    private string GetMemberId(IList<OrderItem> orderList)
    {
        var memberList = GetMemberList(orderList);
        if (MemberIsNotSame(memberList)) throw new TqoonBizException("다중 선택한 주문 건의 주문자가 동일해야 합니다.");
        return memberList[0];
    }

    private IList<string> GetMemberList(IList<OrderItem> orderList)
    {
        return orderList.Select(t => t.MemberId).Distinct().ToList();
    }

    // 입금처리할 주문들의 고객이 모두 동일한지 체크, 모두 동일해야 입금 처리 가능
    private bool MemberIsNotSame(IList<string> memberList)
    {
        return memberList.Count() != 1;
    }

    private int GetCurrentPointByMemberId(string memberId)
    {
        return MemberDao.GetCurrentPointByMemberId(memberId);
    }

    private int GetTotalOrderAmount(IList<OrderItem> depositTargetOrderList)
    {
        return depositTargetOrderList.Sum(t => t.OrderAmount);
    }

    private int GetDepositProcPrice(int id)
    {
        var deposit = DepositDao.GetNotConfirmedDepositById(id);
        if (deposit == null) throw new Exception("주문을 확인해주세요");        
        return deposit.ProcAmt;
    }
}