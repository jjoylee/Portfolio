define(function (require) {
  var $ = require("jquery");
  var App = require("app");
  var Backbone = require("backbone");
  var _ = require('underscore');
  var PlusMinusRow = require("plugins/approval/forms/PlusMinusRow"); 

var Integration = Backbone.View.extend({
		initialize : function(options){
			this.options = options || {};
			this.docModel = this.options.docModel;
			this.variables = this.options.variables;
			this.infoData = this.options.infoData;
		},

		render : function() {
			var self = this;
			$(".cur input").on("change",function(){
					self.calAmount();
			});
			$(".edu_cur input").on("change",function(){
					self.calAmount();
			});
			 $('#dynamic_table').PlusMinusRow({
			        /* 필수 옵션 */
			        tableId : "dynamic_table",
			        plusBtnId : "plus",
			        minusBtnId : "minus",
			        copyRowClass : "copyRow",
			        minusRowCallback : function() {
			        	self.calAmount();
			        }
			 });
		},

    calAmount : function () {
    	var self = this;
    	var sum_cur1 = 0;
    	var sum_cur2 = 0;
    	$("#dynamic_table tr ").each(function(i, e){
    		if ($(e).find('.edu_cur input')[0]) {
    			var edu_cur = parseFloat($(e).find('.edu_cur input').val().replace(/\,/g,""));
    			sum_cur1 = edu_cur;
    		}
    			$(".sum_cur").text(self._convertCurrencyFormat(sum_cur1));
    		
    		if ($(e).find('.cur input')[0]) {
    			var cur = parseFloat($(e).find('.cur input').val().replace(/\,/g,""));
    			if(!cur) cur = 0;
    			sum_cur2 += cur;
    		}  
    	});
		$(".sum_cur").text(self._convertCurrencyFormat(sum_cur1+sum_cur2));
    },

    _convertCurrencyFormat : function(value) { 
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
		}
	});

	return Integration;
	});