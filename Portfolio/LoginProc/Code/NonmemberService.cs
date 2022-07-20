public class NonmemberService : INonmemberService
{
    [Autowire(Type = typeof(EmailNonmemberFinder))]
    public INonmemberFinder EmailNonmemberFinder { get; set; }

    [Autowire(Type = typeof(OrderIdNonmemberFinder))]
    public INonmemberFinder OrderCodeNonmemberFinder { get; set; }

    public MemberItem FindByEmail(string email, string password)
    {
        return EmailNonmemberFinder.Find(email, password);
    }

    // 주문번호로 비회원 로그인
    public MemberItem FindByOrderId(string orderId, string password)
    {
        return OrderIdNonmemberFinder.Find(orderId, password);
    }
    // ... 생략
}