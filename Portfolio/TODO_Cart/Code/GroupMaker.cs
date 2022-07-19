
public class GroupMaker : IGroupMaker
{
    [Autowire]
    public IGroupKeyGetter GroupKeyGetter { get; set; }
        
    [Autowire]
    public IGroupChecker GroupChecker { get; set; }

    public IList<CartGroupView> Make(IList<CartItemUserView> list)
    {
        var groups = new List<CartGroupView>();
        foreach (var item in list)
        {
            var group = GetGroup(groups, item);
            group.List.Add(item);
        }
        return groups;
    }

    private CartGroupView AddNewGroup(List<CartGroupView> groups, CartItemUserView item)
    {
        var group = new CartGroupView() { Key = GroupKeyGetter.Get(item) };
        groups.Add(group);
        return group;
    }

    private CartGroupView GetGroup(List<CartGroupView> groups, CartItemUserView item)
    {
        if (GroupChecker.GroupForCartItemExist(groups, item)) return groups.Where(s => GroupChecker.IsSameGroup(s.Key, item)).First();
        return AddNewGroup(groups, item);
    }
}

