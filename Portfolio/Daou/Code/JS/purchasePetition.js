/* 해성옵틱스 - 품의서(구매품의서)에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var PlusMinusRow = require("plugins/approval/forms/PlusMinusRow");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		events: {
			'change .budget input' : 'calcBalance',
			'change .complete input' : 'calcBalance',
		},

		initialize : function(options){
			this.options = options || {};
			this.docModel = this.options.docModel;
			this.variables = this.options.variables;
			this.infoData = this.options.infoData;
		},

		render : function(){
			var self = this;
			$('.viewModeHiddenPart').show();

			//처음 문서 생성 시 현재 date setting
			if(GO.util.store.get('document.docStatus') == 'CREATE') self.setDate();

			$("#dynamic_table").PlusMinusRow({
				tableId : "dynamic_table",
				plusBtnId : "plus",
				minusBtnId : "minus",
				copyRowClass : "copyRow",
				plusRowCallback : function(){
					//줄 추가 시 현재 date setting
					$(".purchaseDate:last input").val(self.getDate());
					$(".copiedRow:last").find(".balance").text("")
				},
				minusRowCallback : function(){
					self.calcCostSum();
				}
			});

			$(".amount input, .rate input, .currency select, .price input, .cost input").on("change",function(){
				self.setCost(this);
				self.calcCostSum();
				self.calcBalance(this);
			});
		},
		
		// 금액 값 setting
		setCost : function(target){
			$(".cost input").off();
			var currentRow = target.parentNode.parentElement;
			var curr = currentRow.querySelector(".currency select").value;
			var amount = parseInt(currentRow.querySelector(".amount input").value);
			var price = parseInt(currentRow.querySelector(".price input").value.replace(/\,/g,""));
			var rate = parseInt(currentRow.querySelector(".rate input").value.replace(/\,/g,""));
			if(isNaN(amount)) amount = 0;
			if(isNaN(price)) price = 0;
			if(isNaN(rate)) rate = 0;
			currentRow.querySelector(".cost input").value = this.convertCurrencyFormat(this.calcCost(curr,amount,price,rate));
		},

		// 금액 값 계산 (USD,JPY,CNY - 환율 * 단가* 수량 = 금액,  KOR - 단가 * 수량  =금액)
		calcCost : function(curr, amount, price, rate){
			var currType = [["USD","JPY","CNY"],["KOR"]];
			if(currType[0].indexOf(curr) == -1)	return price * amount;
			return rate * price * amount;
		},

		setDate : function(){
			$(".purchaseDate input").val(this.getDate());
		},

		getDate : function() {
			var dayArr = [ "일", "월", "화", "수", "목", "금", "토" ];
		    var today = new Date();
		    var year = today.getFullYear();
		    var month = today.getMonth() + 1;
		    var date = today.getDate();
		    var day = "(" + dayArr[today.getDay()] + ")";
		    if (month < 10) { month = "0" + month; }
		    if (date < 10) { date = "0" + date; }
		    return year + "-" + month + "-" + date + day;
		},

		//예산잔액 계산 ( 예산금액 - 금액 - 기품의완료금액 )
		calcBalance : function(event){
			if(event.target) var currentRow = (event.target).parentNode.parentElement;
			else var currentRow = event.parentNode.parentElement;
			var budget = parseInt(currentRow.querySelector(".budget input").value.replace(/\,/g,""));
			currentRow.querySelector(".balance").innerHTML = ""; //예산금액 초기화
			if(!isNaN(budget)){
				var cost = parseInt(currentRow.querySelector(".cost input").value.replace(/\,/g,"")); // 금액
				var complete = parseInt(currentRow.querySelector(".complete input").value.replace(/\,/g,"")); //기품의완료금액
				if(isNaN(cost)) cost = 0;
				if(isNaN(complete)) complete = 0;
				currentRow.querySelector(".balance").innerHTML = this.convertCurrencyFormat(budget - cost - complete);
			}
		},

		// 금액 합계 계산
		calcCostSum : function(){
			NodeList.prototype.forEach = Array.prototype.forEach;
			var total = 0;
			document.querySelectorAll(".cost input").forEach(function(item){
				var cost = parseInt(item.value.replace(/\,/g,""));
				if(!isNaN(cost)) total += cost;
			});
			document.querySelector("#cost_sum").innerHTML = this.convertCurrencyFormat(total);
		},

		convertCurrencyFormat : function(value) {
			return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
		},
	
		renderViewMode : function(){
			$('.viewModeHiddenPart').hide();
		},
			
		onEditDocument : function(){
			this.render();
		},

		beforeSave :function() {
		},

		afterSave :function() {
		},
			
		validate :function() {
			return true;
		},

		getDocVariables : function(){
			/* getDocVariables 사용 하려면 return 소스 코드 구현 */ 
		}
	});
	return Integration;
});
