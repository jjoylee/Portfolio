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
			 $('#dynamic_table').PlusMinusRow({
			        /* 필수 옵션 */
			        tableId : "dynamic_table",
			        plusBtnId : "plus",
			        minusBtnId : "minus",
			        copyRowClass : "copyRow"
			 });
			
	   
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