/* 해성옵틱스 - 법인카드 사용 신청서에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var PlusMinusRow = require("plugins/approval/forms/PlusMinusRow");
	var Integration = Backbone.View.extend({
			el: '#document_content',
			var month = ['12월','1월','2월','3월','4월','5월','6월','7월','8월','9월','10월','11월','12월','1월']
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
					copyRowClass : "copyRow"
				});
				// 로그인한 사용자, 소속, 현재 날짜 setting
				if(GO.util.store.get('document.docStatus') == 'CREATE'){ this.setDefault(); }
			},

			setDefault : function(){
				$("#dept input").val($("#draftDept").val());
				$("#user input").val($("#draftUser").val());
				this.setDate();
			},
			setDate : function() {
				var dayArr = [ "일", "월", "화", "수", "목", "금", "토" ];
				var today = new Date();
				var year = today.getFullYear();
				var month = today.getMonth() + 1;
				var date = today.getDate();
				var day = "(" + dayArr[today.getDay()] + ")";
	 			if (month < 10) { month = "0" + month; }
	 			if (date < 10) { date = "0" + date; }
				$("#date input").val(year + "-" + month + "-" + date + day);
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