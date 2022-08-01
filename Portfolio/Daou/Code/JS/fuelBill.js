/* 해성옵틱스 - 차량유류비청구서에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var PlusMinusRow = require("plugins/approval/forms/PlusMinusRow");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		events: {
			'change .start input' : 'calcAdd',
			'change .arrive input' : 'calcAdd',
			'change .unitPrice input' : 'calcCostAndFuel',
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

			if(GO.util.store.get('document.docStatus') == 'CREATE') self.setDefault();

			$("#dynamic_table").PlusMinusRow({
				tableId : "dynamic_table",
				plusBtnId : "plus",
				minusBtnId : "minus",
				copyRowClass : "copyRow",
				plusRowCallback : function(){
					$(".addition:last , .cost:last").text("");
				},
				minusRowCallback : function(){
					self.calcAddDepr();
					self.calcFuel();
				}
			});
		},

		setDefault : function(){
			$("#user input").val($("#draftUser").val());
			$("#dept input").val($("#draftDept").val());
			$("#pos input").val($("#position").val());
		}, 

		// 적산 Km 계산 (도착 - 출발)
		calcAdd : function(event){
			var currentRow = (event.target).parentNode.parentElement;
			var start = parseFloat(currentRow.querySelector(".start input").value); // 출발Km
			var arrive = parseFloat(currentRow.querySelector(".arrive input").value); // 도착Km
			currentRow.querySelector(".addition").innerHTML = "";
			if(!isNaN(start) && !isNaN(arrive)){
				if(arrive > start) currentRow.querySelector(".addition").innerHTML = arrive - start;
			}
			this.calcCostAndFuel(event);
			this.calcAddDepr();
		},

		//적산거리(적산KM 합) && 감가상각 계산
		calcAddDepr : function(){
			var total = this.eachFunc(".addition");
			document.querySelector("#addSum").innerHTML = total; // 적산거리
			document.querySelector("#depr").innerHTML = this.convertCurrencyFormat(total*75); //감가상각
			this.calcCostSum();
		},

		// each 루프를 돌아 합 계산하는 함수
		eachFunc : function(selector){
			NodeList.prototype.forEach = Array.prototype.forEach;
			var total = 0;
			document.querySelectorAll(selector).forEach(function(item){
				var eachVal = parseFloat(item.innerHTML.replace(/\,/g,""));
				if(!isNaN(eachVal)) total += eachVal; // text일 경우
			});
			return total;
		},

		// 금액 합계 계산
		calcCostSum : function(){
			var fuel = parseFloat(document.querySelector("#fuel").innerHTML.replace(/\,/g,"")); // 유류비
			var depr = parseFloat(document.querySelector("#depr").innerHTML.replace(/\,/g,"")); // 감가상각
			if(isNaN(fuel)) fuel = 0;
			if(isNaN(depr)) depr = 0;
			document.querySelector("#costSum").innerHTML = this.convertCurrencyFormat(fuel + depr);
		},

		//유류비 계산
		calcFuel : function(){
			document.querySelector("#fuel").innerHTML = this.convertCurrencyFormat(this.eachFunc(".cost"));
			this.calcCostSum();
		},

		// 금액 && 유류비 계산
		calcCostAndFuel : function(event){
			var currentRow = (event.target).parentNode.parentElement;
			var unitPrice = parseFloat(currentRow.querySelector(".unitPrice input").value.replace(/\,/g,"")); // 유류단가
			var addition = parseFloat(currentRow.querySelector(".addition").innerHTML); //적산Km
			currentRow.querySelector(".cost").innerHTML = "";
			if(!isNaN(unitPrice) && !isNaN(addition)) {
				currentRow.querySelector(".cost").innerHTML = this.convertCurrencyFormat(unitPrice * addition);
			}
			this.calcFuel(); 
		},

		convertCurrencyFormat : function(value) {
			var value = value.toString().split(".");
			if(value[1]) return value[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "." + value[1];
			return value[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",")
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
