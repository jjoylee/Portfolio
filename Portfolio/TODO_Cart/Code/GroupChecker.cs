
public class GroupChecker : IGroupChecker
{
    [Autowire]
    public IGroupKeyGetter GroupKeyGetter { get; set; }

    public bool GroupForCartItemExist(IList<CartGroupView> groups, CartItemUserView item)
    {
        return groups.Any(a => IsSameGroup(a.Key, item));
    }

    public bool IsSameGroup(string key, CartItemUserView item)
    {
        return key == GroupKeyGetter.Get(item);
    }
}
