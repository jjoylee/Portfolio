## 상품 카테고리 값에 따라 다른 문의 양식 표시하는 기능 & 문의 CRUD 기능 구현

### Skill : C#, jQuery, AngularJS

### 기능 구현

1. BoardForm 테이블(id, formHtml) 생성 후 필요한 form html 데이터 INSERT 
2. 상품 카테고리와 표시할 form을 매핑해주는 테이블(categoryid, formId) 생성 및 데이터 INSERT
3. 선택한 상품 카테고리 값에 따라 formId로 표시할 form html을 select 해서 화면에 표시 
    - angularJS ng-include로 select box 값이 변할 떄마다 queryString 다르게 해서 url 호출
    - form 다시 조회해서 로드
4. 문의 form에 입력한 값들은 json으로 db column에 insert
5. 문의 내용 update하거나 조회할 때도 json으로 작업. AngularJS로 데이터 바인딩.

https://www.adprint.jp/Customer/Estimate
