
## 최근 본 상품을 최대 5개 보여주기

## 구현

DB, Session, Cookie 중 Cookie에 데이터를 저장하는 방법으로 개발.

## Problem Solving

1. Client가 상품 페이지를 요청한다.

2-1. 최근 본 상품 정보를 저장한 쿠키가 존재하지 않으면 정보를 담을 모델 형식의 빈 리스트를 만든다. 

2-2. 최근 본 상품 정보를 저장한 쿠키가 존재한다면(JSON) 해당 정보를 꺼내서 모델 리스트로 변환한다.    

3. 해당 페이지의 상품과 url 정보를 모델로 만들어 리스트에 넣는다. 

4. 리스트를 JSON형식으로 변환한다.

5. 쿠키에 JSON 데이터를 넣는다.

6. 쿠키를 Client에 전달한다.

 

* Service(RecentlyViewedService) 메소드 설명



1. GetRecentlyViewedItemFromCookie

request 쿠키에 들어있는 최근 본 상품 정보를 List<RecentlyViewedItem>로 만들어 반환하는 함수

쿠키에 데이터를 넣는 프로세스에서 2번 과정을 처리


``` C#

    ```
