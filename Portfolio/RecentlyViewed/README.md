
<br>

## 📌 최근 본 상품 최대 5개 보여주기

DB, Session, Cookie 중 Cookie에 데이터를 저장하는 방법으로 개발

Newtonsoft를 사용해서 JSON ↔ MODEL 변환

<br>

## 📌 구현

1. Client가 상품 페이지 요청

2. 최근 본 상품 정보를 저장한 쿠키가 존재하지 않으면 정보를 담을 빈 리스트를 만들고, 있으면 해당 정보(JSON)를 꺼내서 모델 리스트로 변환한다.

3. 해당 페이지의 상품과 URL 정보를 모델로 만들어 리스트에 넣는다. 

4. 리스트를 JSON 형태로 변환한 후 쿠키에 JSON 데이터를 넣는다.

6. 쿠키를 Client에 전달한다.

<br>

## 📌 주요 코드

[주요 코드 링크](./Code)

<br>

## 📌 Result

<img src="./Image/recentlyViewed.png" width="700" height="400">

https://www.adprint.jp/OrderMall/Order/TotalOrder.aspx?totalCode=PNPDPF

<br>

## 📌 Reference & Problem Solving

https://jjoystory.tistory.com/2  
https://jjoystory.tistory.com/3
 


