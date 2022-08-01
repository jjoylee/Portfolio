/* 해성옵틱스 - 신규양식에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		initialize : function(options){
			this.options = options || {};
			this.docModel = this.options.docModel;
			this.variables = this.options.variables;
			this.infoData = this.options.infoData;
		},

		render : function(){
			//처음 문서 생성 시 기안자 정보 값들 setting
			if(GO.util.store.get('document.docStatus') == 'CREATE')  this.setDefault();
		},

		setDefault : function(){
			$("#dept input").val($("#draftDept").val());
			$("#user input").val($("#draftUser").val());
			$("#no input").val($("#empNo").val());
			$("#mobile input").val($("#mobileNo").val());
			$("#pos input").val($("#position").val());
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
