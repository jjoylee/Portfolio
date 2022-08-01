/* 해성옵틱스 - 품의서(분기투자품의서)에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var PlusMinusRow = require("plugins/approval/forms/PlusMinusRow");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		events: {
			'change .expect input' : 'calcBalance',
			'change .complete input' : 'calcBalance',
			'change .progress input' : 'calcBalance'
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
				plusRowCallback : function(){
					$(".copiedRow:last .balance").text("");
				}
			});
		},

		calcBalance : function(event){
			var currentRow = (event.target).parentNode.parentElement; // 부모 tr 구하기 
			var expect = currentRow.querySelector(".expect input").value.replace(/\,/g,""); // 분기투자예상금액
			var complete = currentRow.querySelector(".complete input").value.replace(/\,/g,""); // 기품의 완료금액(누적)
			var progress = currentRow.querySelector(".progress input").value.replace(/\,/g,""); // 본품의 진행금액
			currentRow.querySelector(".balance").innerHTML = ""; // 투자잔액 초기화
			if(expect){
				// 투자잔액 계산
				if(!complete) complete = 0;
				if(!progress) progress = 0;
				currentRow.querySelector(".balance").innerHTML = (expect - complete - progress).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
			}
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
