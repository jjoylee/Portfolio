/* 해성옵틱스 - 폐기품의서 에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var PlusMinusRow = require("plugins/approval/forms/PlusMinusRow");
	var Integration = Backbone.View.extend({
			el: '#document_content',

			events: {
				'change #rate input' : 'calcCostAndSum',
				'change .unitPrice input' : 'calcCostAndSum',
				'change .amount input' : 'calcCostAndSum',
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
					plusRowCallback : function(){
						$(".copiedRow:last").find(".cost").text(""); // td 초기화
					},
					minusRowCallback: function() {
						self.calcCostAndSum();
					}
				});
			},

			calcCostAndSum : function(){
				var self = this;
				var rate = $("#rate input").val().replace(/\,/g,"");
				$(".cost, #sum").text(""); // 초기화
				if(rate){
					var sum = 0;
					$(".cost").each(function(){
						var $currentRow = $(this).closest("tr");
						var unitPrice = $($currentRow).find(".unitPrice input").val().replace(/\,/g,"");
						var amount = $($currentRow).find(".amount input").val();
						if(unitPrice && amount && !isNaN(amount)){
							var cost = unitPrice * amount * rate;
							sum += cost;
							$(this).text(self.convertCurrencyFormat(cost));
						}
					});
					$("#sum").text(self.convertCurrencyFormat(sum))
				}
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
