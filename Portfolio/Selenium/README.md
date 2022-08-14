<br>

## 📌 Selenium과 C#을 이용한 수집기 개발

### CSS 선택자, XPath를 능숙하게 사용해 원하는 데이터를 추출할 수 있다.

### 1. 박스 상품 수집 프로그램 개발 (2018.03)   

2개의 쇼핑몰 사이트에서 박스 상품 정보를 수집    
회사에서 사용하는 DB 테이블의 필드 형식에 맞게 데이터를 가공해 총 62,473건의 상품 등록    
driver로 직접 element에 접근해서 데이터를 추출하는 방식(FindBy~)으로 데이터 추출    
HtmlAgillityPack을 사용해 html을 파싱한 뒤 노드에 접근해 데이터 추출(속도 향상) 

``` C#
   var doc = new HtmlDocument();
   doc.LoadHtml(driver.PageSource);
   if (doc.DocumentNode.SelectSingleNode(".//span[@class = 'item_number']") == null) {
    throw new Exception("item_number 파싱오류");
   }
```
<br>

### 2. 은행 통장 입금 내역 수집 프로그램 개발 (2020.10)

3개의 은행 사이트에서 입금 내역을 수집헤 DB에 저장   
DB에 저장한 입금 내역을 기반으로 주문 데이터 입금 처리    
은행 사이트 리뉴얼될 때, 크롬 드라이버 업데이트 시 프로그램 수정 & 관리    
CCNET을 사용해 주기적으로 수집 프로그램이 실행되도록 설정    

<br>

### 3. 네이버 스토어 상품 업로드 (2022.04)

DB에 있는 상품 데이터를 네이버 스토어 관리자 사이트에 업로드  
네이버 스토어에 있는 상품 복사 기능을 활용해 동일한 데이터를 반복 입력할 필요 없도록 개발   
업로드할 때 항상 동일한 데이터를 입력해 복사용 상품을 등록    
복사용 상품을 복사하고 이름 등 상품마다 다른 데이터만 수정해서 상품등록을 빠르게 할 수 있다.   

<br>

``` C#
try {
      // 상품 업로드시 항상 동일한 데이터를 입력해 놓은 복사할 때 쓰기위한 상품을 미리 등록해 둠
      // 복사용 상품을 복사
      CopyRegisterStandardGoods();
      
      // 상품마다 다른 정보들 입력(수정)
      SetCategory(info);

      // 안전기준 준수 대상 품목 모달(카테고리에 따라 나오지 않을 수 있다.)
      CloseModalByClickButton("확인");
      SetGoodsName(info);

      SetPrice(info);

      SetRepresentativeImage(dir);
      SetAdditionalImages(dir);

      SetDetailDesc(info);

      ClickUiViewToggleBtn("attribute");
      SetAttribute(info);

      ClickUiViewToggleBtn("provided-notice");
      SetManufacturer(info);

      ClickUiViewToggleBtn("sellerCode");
      SetSellerManagementCode(info);

      Save();
      // 상품속성 미입력 확인(상품에 따라 나오지 않을 수 있다.)
      CloseModalByClickButton("다음에 할래요");
      // 상품등록 완료 모달
      CloseModalByClickButton("상품관리");
}
```
