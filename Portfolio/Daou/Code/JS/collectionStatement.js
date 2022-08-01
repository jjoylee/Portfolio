/* 더유 - 수금명세서에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		events: {
			'change .type select' : 'calcTypeTotal',
			'change .cost input' : 'calcTypeTotal',
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
		
		calcTypeTotal : function(){

			var typeObj = { 
				"카드" : { "name" : "card", "sum" : 0 }, 
				"현금" : { "name" : "cash", "sum" : 0 }, 
				"무통장" : { "name" : "bankBook", "sum" : 0 }
			}; 

			document.querySelectorAll(".type").forEach(function(item){
				var currentRow = item.parentElement;
				var cost = parseInt(currentRow.querySelector(".cost input").value.replace(/\,/g, ""));
				if(!isNaN(cost)) typeObj[item.querySelector("select").value].sum += cost;
			});
			
			var total = 0;
			for(prop in typeObj){
				document.querySelector("#" + typeObj[prop].name + " input").value = this.convertCurrencyFormat(typeObj[prop].sum);
				total += typeObj[prop].sum;
				typeObj[prop].sum = 0;
			}

			document.querySelector("#subTotal input").value = this.convertCurrencyFormat(total);
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
