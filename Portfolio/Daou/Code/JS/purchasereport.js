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
			$(".amount input").on("change",function(){
					self.calAmount();
			});
			$(".price input").on("change",function(){
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
			        },
			        plusRowCallback : function() {
			        	self.calAmount();
			        }
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
		    $("#cal input").val(year + "-" + month + "-" + date + day);
		},

	    calAmount : function () {
	    	var self = this;
	    	var cur = 0;
	    	var sum_amount = 0;
	    	var sum_price = 0;
	    	var sum_cur = 0;
	
	    	$("#dynamic_table tr").each(function(i, e){
	    		 if ($(e).find('.amount')[0]) {
	    			var amount = parseInt($(e).find('.amount input').val());
	    			var price = parseFloat($(e).find('.price input').val().replace(/\,/g,""));
	    	
	    			if (isNaN(amount)) amount = 0;
	    			if (isNaN(price)) price = 0;
	    			
	    			cur = amount * price;
	    			
	    			$(e).find(".cur").text(self._convertCurrencyFormat(cur));
	    			
	    			sum_amount = sum_amount + amount;
	       	        sum_price = sum_price + price;
	       	        sum_cur = sum_cur + cur;
	    		}  
	    	});
			
	    	$(".sum_amount").text(sum_amount);
	    	$(".sum_price").text(self._convertCurrencyFormat(sum_price));
	    	$(".sum_cur").text(self._convertCurrencyFormat(sum_cur));
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