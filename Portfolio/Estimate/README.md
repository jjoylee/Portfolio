## 상품 카테고리 값에 따라 다른 문의 양식 표시하는 기능 & 문의 CRUD 기능 구현

### Skill : C#, jQuery, AngularJS

### 기능 구현

#### 1. BoardForm 테이블(id, viewForm, writeForm) 생성 후 데이터 INSERT   
writeForm : 문의를 작성 또는 수정할 때 사용하는 html  
viewForm : 작성한 문의 내용을 보여줄 때 사용하는 html  
   
#### 2. 상품 카테고리 select box 값과 표시할 form을 매핑해주는 테이블 생성 및 데이터 INSERT

#### 3. 문의를 작성 또는 수정할 때, 선택한 상품 카테고리 값에 따라 formId로 writeForm을 조회해 화면에 표시 

AngularJS ng-include로 select box 값이 변할 떄마다 parameter 값 다르게 해서 REQUEST 전송 

``` javascript
$scope.GetWriteTemplateUrl = function () {
   return "/Customer/GetEstimateWriteForm?tabType=" + $scope.tabType + "&topList=" + $scope.topList;
};
```

``` html
<div class="option_customer boardForm" id="estimateContents" >
   <ng-include src="GetWriteTemplateUrl()"></ng-include>
</div>
```

form 다시 조회해서 로드

```C#
public ContentResult GetEstimateWriteForm(string tabType, string topList)
{
   var boardForm = BoardFormService.FindByFormByTabTypeAndTopList(tabType, topList);
   return Content(boardForm.WriteForm, "text/html", System.Text.Encoding.UTF8);
}
```

#### 4. 작성한 문의 내용을 보여줄 때는 viewForm을 조회
#### 5. 문의 양식에 입력한 값들은 json으로 전달해 db 컬럼에 INSERT

```javascript
JSON.stringify({ "formData": $scope.formData })
```

#### 6. 문의 내용을 수정하거나 조회할 때도 json으로 작업. AngularJS로 데이터 바인딩.

https://www.adprint.jp/Customer/Estimate
