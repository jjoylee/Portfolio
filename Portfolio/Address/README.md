📌 나라마다 다른 주소 입력 양식 제공 & 주소 저장하기

한국 주소 : 도로명 주소 + 세부 주소  
미국 주소 : 주 + 도시 + 세부 주소  

해결해야 할 문제 
1. 나라마다 다른 주소 체계를 가지고 있다. 주문할 떄 주소를 받아 db에 저장해야 한다.  
2. 나라마다 다른 주소 양식을 보여줘야 한다.

📌 해결 

1. 주소를 json으로 변환 후 db에 저장. 어떤 나라의 주소라도 하나의 컬럼에 저장할 수 있다.
2. AngularJs templateUrl을 사용해 나라에 따라 다른 주소 입력 폼 html을 load 할 수 있다.

``` javascript
 templateUrl: ['$NationConfig', function ($NationConfig) {
    var nation = $NationConfig.Nation;
    return "common address form url" + '/Address.tmpl.' + nation + + '.html';
 }],
```
