/* 해성옵틱스 - 출장 복귀 명령서에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var PlusMinusRow = require("plugins/approval/forms/PlusMinusRow");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		events: {
			'change .cur input' : 'calcSum',
			'change .cur input' : 'calcSum'
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

			$("#dynamic_table1").PlusMinusRow({
				tableId : "dynamic_table1",
				plusBtnId : "plus1",
				minusBtnId : "minus1",
				copyRowClass : "copyRow1",
				rowspanClass : "rowspan1"
			});

			$("#dynamic_table2").PlusMinusRow({
				tableId : "dynamic_table2",
				plusBtnId : "plus2",
				minusBtnId : "minus2",
				copyRowClass : "copyRow2",
				rowspanClass : "rowspan2",
				minusRowCallback : function(){
					self.calcSum();
				}
			});

			if(GO.util.store.get('document.docStatus') == 'CREATE') self.setDefault();
		},

		setDefault : function(){
			$("#user input").val($("#draftUser").val());
			$("#trip input").val($("#draftUser").val());
			$("#dept input").val($("#draftDept").val());
			$("#pos input").val($("#position").val());
		},

		calcSum : function(){
			NodeList.prototype.forEach = Array.prototype.forEach;
			var total = 0;
			document.querySelectorAll(".cur input").forEach(function(item){
				if(item.value) total += parseInt(item.value.replace(/\,/g, ""));
			});
			document.querySelector("#sum_cur").innerHTML = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
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
