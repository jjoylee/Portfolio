/* 더유 - 견본품제공에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		events: {
			'change .standard input' : 'calcSubTotal',
			'change .amount input' : 'calcSubTotal',
		},

		initialize : function(options){
			this.options = options || {};
			this.docModel = this.options.docModel;
			this.variables = this.options.variables;
			this.infoData = this.options.infoData;
			NodeList.prototype.forEach = Array.prototype.forEach;
		},

		render : function(){
		},
		
		//소계 계산
		calcSubTotal : function(event){
			var currentRow = (event.target).parentNode.parentElement;
			var amount = parseInt(currentRow.querySelector(".amount input").value);
			var standard = parseInt(currentRow.querySelector(".standard input").value);
			currentRow.querySelector(".subTotal input").value = "";
			if(!isNaN(amount) && !isNaN(standard)) {
				currentRow.querySelector(".subTotal input").value = standard * amount;
			}
			this.calcTotal();
		},

		calcTotal : function(){
			var total = 0;
			document.querySelectorAll(".subTotal input").forEach(function(item){
				var subTotal = parseInt(item.value);
				if(!isNaN(subTotal)) total += subTotal;
			});
			document.querySelector("#total").innerHTML = total;
		},

		renderViewMode : function(){
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
