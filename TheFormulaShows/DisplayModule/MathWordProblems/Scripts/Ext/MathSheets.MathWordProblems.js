﻿
var MathSheets = MathSheets || {};

MathSheets.MathWordProblems = MathSheets.MathWordProblems || (function () {

	// 运算符点击切换(>、<、=)
	_imgProblemsClick = function (element) {
		if ($(element).attr("title") == 'help') {
			$(element).attr("title", 'plus');
			$(element).attr("src", '../Content/image/Plus.png');
			return;
		} else if ($(element).attr("title") == 'plus') {
			$(element).attr("title", 'subtraction');
			$(element).attr("src", '../Content/image/Subtraction.png');
			return;
		} else if ($(element).attr("title") == 'subtraction') {
			$(element).attr("title", 'multiple');
			$(element).attr("src", '../Content/image/Multiple.png');
			return;
		} else if ($(element).attr("title") == 'multiple') {
			$(element).attr("title", 'division');
			$(element).attr("src", '../Content/image/Division.png');
			return;
		} else if ($(element).attr("title") == 'division') {
			$(element).attr("title", 'plus');
			$(element).attr("src", '../Content/image/Plus.png');
			return;
		}
	},

		// 运算符转换(+-*/)
		_signToString = function (title) {
			switch (title) {
				case "plus":
					return "+";
				case "subtraction":
					return "-";
				case "multiple":
					return "*";
				case "division":
					return "/";
				default:
					return "";
			}
		},

		// 計算式各項目的非空檢查
		_checkInputIsEmpty = function (inputArray) {
			var inputL = $(inputArray[0]).val();
			if (inputL == '') {
				return true;
			}

			var inputR = $(inputArray[1]).val();
			if (inputR == '') {
				return true;
			}

			var title = $(inputArray[2]).attr("title");
			if (title == 'help') {
				return true;
			}

			var inputA = $(inputArray[3]).val();
			if (inputA == '') {
				return true;
			}

			// 如果有單位填空項目
			if ($(inputArray[4]).length > 0) {
				if ($(inputArray[4]).val() == '') {
					return true;
				}
			}

			return false;
		},

		// 答案比對
		_isExist = function (index, result) {
			// 計算公式
			var hidValue = $('#hiddenMwp' + index).val();
			// 單位
			var hidUnit = $('#hiddenMwpUnit' + index).val();

			var answers = (hidValue || '').split(',');
			for (var i = 0; i < answers.length; i++) {
				answer = answers[i];
				if (hidUnit != '') {
					answer += '(' + hidUnit + ')';
				}
				// 檢查計算公式是否一致
				if (answer == result)
					return true;
			}
			return false;
		},

		// 答题验证(正确:true  错误:false)
		_mathWordProblemsCorrecting = function (pIndex, pElement) {
			var inputArray = new Array();
			var index = pIndex.toString().PadLeft(2, '0');
			inputArray.push($('#inputMwp' + index + '0'));
			inputArray.push($('#inputMwp' + index + '1'));
			inputArray.push($('#imgMwp' + index));
			inputArray.push($('#inputMwp' + index + '2'));
			// 單位填空項目
			inputArray.push($('#inputMwp' + index + '3'));

			var result = '';
			// 計算式各項目的非空檢查
			if (!_checkInputIsEmpty(inputArray)) {
				// 等式健全的情況下，統計結果
				result = $(inputArray[0]).val() + _signToString($(inputArray[2]).attr("title")) + $(inputArray[1]).val() + "=" + $(inputArray[3]).val();
				// 如果有單位填空項目
				if ($(inputArray[4]).length > 0) {
					result += "(" + $(inputArray[4]).val() + ")";
				}
			}

			// 驗證輸入值是否與答案一致
			if (_isExist(index, result)) {
				// 在錯題集中移除當前項目
				__allFaultInputElementArray.remove({ position: "mathSheetMathWordProblems", id: ('inputMwp' + index + '0') });
				// 對錯圖片顯示和影藏
				$('#imgOKProblems' + index).show();
				$('#imgNoProblems' + index).hide();
				// 移除圖片抖動特效
				$('#imgNoProblems' + index).removeClass("shake shake-slow");
				inputArray.forEach(function (element, index) {
					$(element).attr("disabled", "disabled");
				});
				// 正确:true
				return true;
			} else {
				// 收集所有錯題項目ID
				__allFaultInputElementArray.push({ position: "mathSheetMathWordProblems", id: ('inputMwp' + index + '0') });
				// 对错图片显示和隐藏
				$('#imgOKProblems' + index).hide();
				$('#imgNoProblems' + index).show();
				$('#imgNoProblems' + index).animate({
					width: "40px",
					height: "40px",
					marginLeft: "0px",
					marginTop: "0px"
				}, 1000, function () {
					// 添加圖片抖動特效（只針對錯題）
					$(this).addClass("shake shake-slow");
				});
				// 错误:false
				return false;
			}
		},

		// 打印設置
		printSetting = function () {
			$("img[id*='imgMwp']").each(function (index, element) {
				$(element).replaceWith("<button id=\"btnMwp" + index + "\" type=\"button\" class=\"btn btn-default btn-circle button-addBorder\"></button>");
			});

			$("input[id*='inputMwp']").each(function (index, element) {
				$(element).addClass('input-print');
				$(element).removeAttr('placeholder');
				$(element).removeAttr("disabled");
			});
		},

		// 打印后頁面設定
		printAfterSetting = function () {
			$("button[id*='btnMwp']").each(function (index, element) {
				$(element).replaceWith("<img src=\"../Content/image/help.png\" id=\"imgMwp" + index + "\" style=\"width: 30px; height: 30px; \" title=\"help\" />");
			});

			$("input[id*='inputMwp']").each(function (index, element) {
				$(element).removeClass('input-print');
				$(element).attr('placeholder', '??');
				$(element).attr("disabled", "disabled");
			});
		},

		// 设定页面所有输入域为可用状态(算式应用题)
		ready = function () {
			$("input[id*='inputMwp']").each(function (index, element) {
				// 收集所有可輸入項目ID
				__allInputElementArray.push({ position: "mathSheetMathWordProblems", id: $(element).attr("id") });
				$(element).removeAttr("disabled");
			});
			$("img[id*='imgMwp']").each(function (index, element) {
				$(element).click(function () { _imgProblemsClick(element); });
			});
		},

		// 交卷
		theirPapers = function () {
			$("input[id*='hiddenMwp']").each(function (pIndex, pElement) {
				// 答题验证
				if (!_mathWordProblemsCorrecting(pIndex, pElement)) {
					// 答题错误时,错误件数加一
					__isFault++;
				} else {
					__isRight++;
				}
			});
		},

		// 订正(算式应用题)
		makeCorrections = function () {
			var fault = 0;
			$("input[id*='hiddenMwp']").each(function (pIndex, pElement) {
				// 答题验证
				if (!_mathWordProblemsCorrecting(pIndex, pElement)) {
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
