public class GroupKeyGetter : IGroupKeyGetter
{
    public string Get(CartItemUserView item)
    {
        return "KEY로 사용할 값";
    }
}
