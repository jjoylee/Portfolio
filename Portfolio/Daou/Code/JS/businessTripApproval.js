/* 해성옵틱스 - 출장품의서에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var PlusMinusRow = require("plugins/approval/forms/PlusMinusRow");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		events: {
			'change .amount input' : 'calcSumAndTotal',
			'change .price input' : 'calcSumAndTotal'
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

			$("#dynamic_table").PlusMinusRow({
				tableId : "dynamic_table",
				plusBtnId : "plus",
				minusBtnId : "minus",
				copyRowClass : "copyRow",
				rowspanClass : "rowSpan"
			});
		},

		calcSumAndTotal : function(){
			NodeList.prototype.forEach = Array.prototype.forEach;
			var total = 0;
			document.querySelectorAll(".amount").forEach(function(item){
				var currentRow = item.parentElement; 
				currentRow.querySelector(".sum").innerHTML = ""; // 소계 초기화
				var amount = item.querySelector("input").value; // 일자/수량
				var price = currentRow.querySelector(".price input").value.replace(/\,/g,""); // 단가 
				if(amount && !isNaN(amount) && price){
					//소계 계산
					currentRow.querySelector(".sum").innerHTML = (amount * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
					total += amount * price;
				}
			});
			// 합계계산
			document.querySelector("#total_sum").innerHTML = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
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
