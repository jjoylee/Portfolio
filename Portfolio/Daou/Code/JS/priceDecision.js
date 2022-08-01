define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var PlusMinusRow = require("plugins/approval/forms/PlusMinusRow");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		events: {
			'change .original input' : 'calcDiff',
			'change .change input' : 'calcDiff',
			'change .diff input' : 'calcDiff'
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
				copyRowNoClass : "copyRowNo",
			});
		},

		calcDiff : function(event){
			$(".diff input").off();
			var currentRow = (event.target).parentNode.parentElement; // 부모 tr 구하기 
			var original = currentRow.querySelector(".original input").value.replace(/\,/g,""); // 기존단가
			var change = currentRow.querySelector(".change input").value.replace(/\,/g,""); // 변경단가
			currentRow.querySelector(".diff input").value = ""; // 차이금액 초기화
			if(original && change){
				// 차이금액 계산
				currentRow.querySelector(".diff input").value = (original - change).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
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
