// 카트 페이지에서 같이 주문할 수 있는 상품들을 구분해서 보여주기 위한 객체 (화면에 표시를 위한 view model)
public class CartGroupView
{
    public string Key { get; set; } = string.Empty;
    
    // 해당 카트 그룹(KEY로 구분)의 카트 상품 리스트
    public IList<CartItemUserView> List { get; set; } = new List<CartItemUserView>(); 
}
