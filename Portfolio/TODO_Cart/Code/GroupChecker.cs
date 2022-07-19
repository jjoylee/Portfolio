
public class GroupChecker : IGroupChecker
{
    [Autowire]
    public IGroupKeyGetter GroupKeyGetter { get; set; }

    // 해당 카트 아이템(item)이 속할 그룹(같이 주문할 수 있는 카트 상품들)이 이미 만들어져 있는지 체크 
    public bool GroupForCartItemExist(IList<CartGroupView> groups, CartItemUserView item)
    {
        return groups.Any(a => IsSameGroup(a.Key, item));
    }
    
    // key 값으로 해당 그룹에 속하는지 체크
    public bool IsSameGroup(string key, CartItemUserView item)
    {
        return key == GroupKeyGetter.Get(item);
    }
}
