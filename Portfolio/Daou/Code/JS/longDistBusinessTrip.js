/* 더유 - 시외출장여비신청에서 사용하는 js */
define(function(require) {
	var $ = require("jquery");
	var app = require("app");
	var Backbone = require("backbone");
	var Integration = Backbone.View.extend({
		el: '#document_content',

		events: {
					'change .km input' : function(event){
						this.calcTotalDist(event);
						this.calcTranCostSum();
						this.calcAllDist();
						this.calcTotalSum();
					},
					'change #toll input' : 'calcTotalSum',
					'change #room input' : 'calcTotalSum'
		},
		initialize : function(options){
			this.options = options || {};
			this.docModel = this.options.docModel;
			this.variables = this.options.variables;
			this.infoData = this.options.infoData;
			NodeList.prototype.forEach = Array.prototype.forEach;
		},

		render : function(){
			/* docStatus == 'Create'or 'TempSave' 일때 불리는 함수. (2.0.0 이전의 쓰던 연동코드를  구현) */
		},

		//합계 계산 - 통행료 + 숙박비 + 계
		calcTotalSum : function(){
			var tranCostSum = parseInt(document.querySelector("#tranCost_sum").innerHTML.replace(/\,/g,""));
			var toll = parseInt(document.querySelector("#toll input").value.replace(/\,/g,""));
			var room = parseInt(document.querySelector("#room input").value.replace(/\,/g,""));
			if(isNaN(tranCostSum)) tranCostSum = 0;
			if(isNaN(toll)) toll = 0;
			if(isNaN(room)) room = 0;
			document.querySelector("#total_sum").innerHTML = this.convertCurrencyFormat(tranCostSum + room + toll);
		},

		// 총거리 값 계산 (각 행의 km 합)
		calcTotalDist : function(event){
			var currentRow = (event.target).parentNode.parentElement;
			var total = 0;
			currentRow.querySelectorAll(".km input").forEach(function(item){
				var km = parseFloat(item.value.replace(/\,/g,""));
				if(!isNaN(km)) total += km;
			});
			currentRow.querySelector(".totalDist").innerHTML = this.convertCurrencyFormat(Math.round(total));
			this.calcTranCost(currentRow, Math.round(total)); // 교통비 값 계산
		},

		// 교통비 값 계산 
		calcTranCost : function(currentRow, totalDist){
			var tranCost = (totalDist > 40) ? ((totalDist-40)/10*1800) : 0;
			currentRow.querySelector(".tranCost").innerHTML = this.convertCurrencyFormat(Math.round(tranCost));
		},

		//계 부분 값 계산 - 교통비 합
		calcTranCostSum : function(){
			var tranCostSum = this.eachFunc(".tranCost")[0];
			document.querySelector("#tranCost_sum").innerHTML = this.convertCurrencyFormat(tranCostSum);
		},

		//출장거리 값 계산 - 총거리 합
		calcAllDist : function(){
			var result = this.eachFunc(".totalDist"); // 출장거리, 시내출장 값이 들어가있는 array
			document.querySelector("#totalDist_sum input").value = this.convertCurrencyFormat(result[0]); // 출장거리
			document.querySelector("#inside input").value = this.convertCurrencyFormat(result[1]); // 시내출장
			document.querySelector("#outside1 input").value = this.convertCurrencyFormat(result[0]-result[1]); //시외출장
			document.querySelector("#outside2 input").value = this.convertCurrencyFormat((result[0]-result[1])*180); //시외출장 2번째 
		},

		// (total - each loop 합 계산), (inside - 시내출장 계산)
		// 시내출장 (if 총거리 > 40) 총거리 = 40 (else) 총거리 = 총거리 값으로 total 구하기
		eachFunc : function(selector){
			var total = 0 , inside = 0;
			document.querySelectorAll(selector).forEach(function(item){
				var eachVal = parseFloat(item.innerHTML.replace(/\,/g,""));
				if(!isNaN(eachVal)) {
					total += eachVal;
					inside += (eachVal > 40) ? 40 : eachVal; 
				}
			});
			return [total,inside];
		},

		renderViewMode : function(){
			/* 읽기모드에서 함수가 필요한 경우 구현 */
		},
		convertCurrencyFormat : function(value) {
			return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
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
