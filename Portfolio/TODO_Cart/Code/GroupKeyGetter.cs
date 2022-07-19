public class GroupKeyGetter : IGroupKeyGetter
{
    // 같이 주문할 수 있는 상품들을 구분할 key 값 (그룹 구분)
    public string Get(CartItemUserView item)
    {
        return "KEY로 사용할 값";
    }
}
