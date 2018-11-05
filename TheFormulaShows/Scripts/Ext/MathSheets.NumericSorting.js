﻿
var MathSheets = MathSheets || {};

MathSheets.NumericSorting = MathSheets.NumericSorting || (function () {

		// 答题验证(正确:true  错误:false)
		_NumericSortingCorrecting = function (pindex, pelement) {
			var inputAry = new Array();
			// 答案數組
			var answerAry = ($(pelement).val() || '').split(',');
			var isOK = true;
			$("input[id*='inputNs']").each(function (index, element) {
				inputAry.push($(element));
				if (parseInt($(element).val()) != answerAry[index]) {
					isOK = false;
				}
			});

			// 验证输入值是否与答案一致
			if (!isOK) {
				// 对错图片显示和隐藏
				$('#imgOKNumericSorting' + pindex).show();
				$('#imgNoNumericSorting' + pindex).hide();
				$.each(inputAry, function (index, inputObj) { inputObj.attr("disabled", "disabled"); });
				// 正确:true
				return true;
			} else {
				// 对错图片显示和隐藏
				$('#imgOKNumericSorting' + pindex).hide();
				$('#imgNoNumericSorting' + pindex).show();
				// 错误:false
				return false;
			}
		},

		// 打印設置
		printSetting = function () {
			$("input[id*='hidNsAnswer']").each(function (pindex, pelement) {
				$("input[id*='inputNs']").each(function (index, element) {
					$(element).replaceWith("<button id=\"btnNs" + pindex + "L" + index + "\" type=\"button\" class=\"btn btn-default btn-circle button-addBorder\"></button>");
				});
			});
		},

		// 打印后頁面設定
		printAfterSetting = function () {
			$("input[id*='hidNsAnswer']").each(function (pindex, pelement) {
				$("button[id*='btnNs']").each(function (index, element) {
					$(element).replaceWith("<input id=\"inputNs" + pindex + "L" + index + "\" type=\"text\" placeholder=\" ?? \" class=\"form-control input-addBorder\" style=\"width: 50px; text-align:center;\" disabled=\"disabled\" onkeyup=\"if(!/^\d+$/.test(this.value)) this.value='';\" />");
				});
			});
		},

		// 设定页面所有输入域为可用状态(數字排序)
		ready = function () {
			$("input[id*='inputNs']").each(function (index, element) {
				$(element).removeAttr("disabled");
			});
		},

		theirPapers = function () {
			$("input[id*='hidNsAnswer']").each(function (pindex, pelement) {
				// 答题验证
				if (!_NumericSortingCorrecting(pindex, pelement)) {
					// 答题错误时,错误件数加一
					__isFault++;
				} else {
					__isRight++;
				}
			});
		},

		// 订正(运算比大小)
		makeCorrections = function () {
			var fault = 0;
			$("input[id*='hidNsAnswer']").each(function (pindex, pelement) {
				// 答题验证
				if (!_NumericSortingCorrecting(pindex, pelement)) {
					// 答题错误时,错误件数加一
					fault++;
				}
			});
			return fault;
		};

	return {
		printSetting: printSetting,
		printAfterSetting: printAfterSetting,
		ready: ready,
		makeCorrections: makeCorrections,
		theirPapers: theirPapers
	};
}());