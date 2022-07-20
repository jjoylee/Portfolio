<br>

## 📌 같이 주문할 수 있는 상품들끼리 구분해서 카트 보여주기

✅ BEFORE  
같이 주문할 수 있는 상품들만 카트에 담을 수 있다. 같이 주문할 수 없는 상품을 담으려 할 때 alert 발생.

✅ AFTER   
모든 상품을 카트에 담을 수 있다. 대신 주문 전에 카트에서 같이 주문할 수 있는 상품들끼리 구분해서 보여준다.  
고객은 주문할 상품 그룹을 선택할 수 있다.  
주문 못한 다른 상품 그룹은 다른 주문으로 이어서 할 수 있다. 

<br>

## 📌 주요 코드

https://github.com/jjoylee/portfolio/tree/master/Portfolio/TODO_Cart/Code

### 예시

1. 카트에는 상품 1, 2, 3, 4가 담겨있다고 한다.
2. 상품 1과 2를 같이 주문할 수 있다.
3. 상품 3과 4를 같이 주문할 수 있다.
4. 이는 아래와 같이 표현된다.

- 카트 그룹 리스트 
    - 카트 그룹 1
        - 카트 상품 1
        - 카트 상품 2
    - 카트 그룹 2
        - 카트 상품 3
        - 카트 상품 4

카트 그룹(CartGroupView) = 같이 주문할 수 있는 카트 상품들   
카트 상품(CartItemUserView) = 카트에 넣은 각 (상품)데이터  
화면에 표시를 위한 객체들이라 객체 이름에 View를 붙였다.

### Grouping Flow

1. CartGroupService.Grouping 호출
2. GroupMaker가 빈 카트 그룹 리스트 생성
3. 카트 (상품) 리스트 루프를 돌림
4. GroupChecker로 해당 카트 상품을 위한 카트 그룹 객체가 이미 있는지 체크한다.
5. 있으면 해당 카트 그룹에 카트 상품을 추가한다. 
6. 없으면 새로운 카트 그룹 객체를 만들고, 카트 상품을 추가한다.

GroupKeyGetter      
카트 상품의 카트 그룹 Key(식별자) 값을 반환한다.      

GroupChecker        
GroupKeyGetter를 사용해 이미 있는(만들어진) 카트 그룹과 카트 상품의 그룹이 같은지 체크헌다.     

<br>

## 📌 결과

<img src="https://github.com/jjoylee/portfolio/blob/master/Portfolio/TODO_Cart/Image/Cart.png" width="700" height="400">

http://www.yoki.jp  
