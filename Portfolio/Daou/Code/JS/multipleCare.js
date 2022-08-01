/* 더유 - 복수요양기관에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		events: {
			'change .cost input' : 'calcCostSum',
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
		
		calcCostSum : function(){
			var total = 0;
			document.querySelectorAll('.cost input').forEach(function(item){
				var cost = parseInt(item.value.replace(/\,/g,""));
				if(!isNaN(cost)) total += cost;
			});
			document.querySelector("#cost_sum input").value = this.convertCurrencyFormat(total);
		},

		convertCurrencyFormat : function(value) {
			return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
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
