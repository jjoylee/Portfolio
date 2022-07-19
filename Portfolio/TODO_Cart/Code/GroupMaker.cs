
public class GroupMaker : IGroupMaker
{
    [Autowire]
    public IGroupKeyGetter GroupKeyGetter { get; set; }
        
    [Autowire]
    public IGroupChecker GroupChecker { get; set; }

    // 카트에 담긴 item들로 group list 생성( 같이 주문할 수 있는 item들 끼리 grouping )
    public IList<CartGroupView> Make(IList<CartItemUserView> list)
    {
        var groups = new List<CartGroupView>();
        foreach (var item in list)
        {
            // 이미 해당 item이 속할 그룹이 만들어져 있으면 그 그룹의 list에 추가, 아니면 새로운 그룹을 만든 후 거기에 추가
            var group = GetGroup(groups, item);
            group.List.Add(item);
        }
        return groups;
    }
    
    private CartGroupView GetGroup(List<CartGroupView> groups, CartItemUserView item)
    {
        if (GroupChecker.GroupForCartItemExist(groups, item)) return groups.Where(s => GroupChecker.IsSameGroup(s.Key, item)).First();
        return AddNewGroup(groups, item);
    }

    private CartGroupView AddNewGroup(List<CartGroupView> groups, CartItemUserView item)
    {
        var group = new CartGroupView() { Key = GroupKeyGetter.Get(item) };
        groups.Add(group);
        return group;
    }
}

