public class CartGroupService : ICartGroupService
{
    [Autowire]
    public IGroupMaker GroupMaker { get; set; }

    public IList<CartGroupView> Grouping(IList<CartItemUserView> list)
    {
        return GroupMaker.Make(list);
    }

    public bool AllSameGroup(IList<CartItemUserView> list)
    {
        return Grouping(list).Count() == 1;
    }   
}
