public class CartGroupView
{
    public string Key { get; set; } = string.Empty;
    public IList<CartItemUserView> List { get; set; } = new List<CartItemUserView>();
}
