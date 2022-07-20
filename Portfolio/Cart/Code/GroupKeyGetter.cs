public class GroupKeyGetter : IGroupKeyGetter
{
    /**
        같이 주문할 수 있는 카트 상품들을 구분해주는 key 값(식별자)
        같이 주문할 수 있는 상품, 즉 같은 카트 그룹에 속하는 카트 상품들은 같은 카트 그룹 Key 값을 반환한다.
        예를 들면 상품이름이 같은 카트 상품들끼리만 같이 주문할 수 있다면, 상품이름을 Key값으로 설정하면 된다.
        IGroupKeyGetter를 implements해 다른 기준으로 같이 주문할 수 있는 상품을 구분하는 GroupKeyGetter를 만들 수 있다.
    **/
    public string Get(CartItemUserView item)
    {
        return "KEY로 사용할 값";
    }
}
