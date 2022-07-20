public class CartGroupService : ICartGroupService
{
    [Autowire]
    public IGroupMaker GroupMaker { get; set; }

    // 카트에서 같이 주문할 수 있는 상품들끼리 grouping 
    public IList<CartGroupView> Grouping(IList<CartItemUserView> list)
    {
        return GroupMaker.Make(list);
    }

    // 카트에 담긴 모든 상품을 한번에 주문할 수 있으면 true > 즉 group이 하나.
    public bool AllSameGroup(IList<CartItemUserView> list)
    {
        return Grouping(list).Count() == 1;
    }   
}
