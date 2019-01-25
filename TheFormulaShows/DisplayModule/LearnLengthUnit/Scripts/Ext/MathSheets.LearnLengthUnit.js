﻿var MathSheets = MathSheets || {};

MathSheets.LearnLengthUnit = MathSheets.LearnLengthUnit || (function () {

	// 打印設置
	printSetting = function () {
		$("input[id*='inputLlu']").each(function (index, element) {
			$(element).addClass('input-print');
			$(element).removeAttr('placeholder');
			$(element).removeAttr("disabled");
		});
	},

		// 打印后頁面設定
		printAfterSetting = function () {
			$("input[id*='inputLlu']").each(function (index, element) {
				$(element).removeClass('input-print');
				$(element).attr('placeholder', '??');
				$(element).attr("disabled", "disabled");
			});
		},

		// 答题验证(正确:true  错误:false)
		_learnLengthUnitCorrecting = function (pindex, pelement) {
			var inputAry = new Array();
			// 答案數組
			var answerAry = ($(pelement).val() || '').split(';');
			var isOK = true;
			$("input[id*='inputLlu" + pindex + "S']").each(function (index, element) {
				inputAry.push($(element));
				if (parseInt($(element).val()) != parseInt($.base64.atob(answerAry[index], true))) {
					isOK = false;
				}
			});

			// 验证输入值是否与答案一致
			if (isOK) {
				// 动错题集中移除当前项目
				$.each(inputAry, function (index, inputObj) {
					inputObj.attr("disabled", "disabled");
					removeInputElementArray({ position: "mathSheetLearnLengthUnit", id: inputObj.attr("id") });
				});
				// 对错图片显示和隐藏
				$('#imgOKLearnLengthUnit' + pindex).show();
				$('#imgNoLearnLengthUnit' + pindex).hide();
				// 移除圖片抖動特效
				$('#imgNoLearnLengthUnit' + pindex).removeClass("shake shake-slow");
				// 正确:true
				return true;
			} else {
				// 对错图片显示和隐藏
				$('#imgOKLearnLengthUnit' + pindex).hide();
				$('#imgNoLearnLengthUnit' + pindex).show();
				$('#imgNoLearnLengthUnit' + pindex).animate({
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

		// 设定页面所有输入域为可用状态(認識長度單位)
		ready = function () {
			$("input[id*='inputLlu']").each(function (index, element) {
				// 收集所有可輸入項目ID
				__allInputElementArray.push({ position: "mathSheetLearnLengthUnit", id: $(element).attr("id") });
				$(element).removeAttr("disabled");
			});
		},

		// 订正(認識長度單位題)
		makeCorrections = function () {
			var fault = 0;
			$("input[id*='hidLluAnswer']").each(function (pindex, pelement) {
				// 答题验证
				if (!_learnLengthUnitCorrecting(pindex, pelement)) {
					// 答题错误时,错误件数加一
					fault++;
				}
			});
			return fault;
		},

		// 認識長度單位交卷
		theirPapers = function () {
			$("input[id*='hidLluAnswer']").each(function (pindex, pelement) {
				// 答题验证
				if (!_learnLengthUnitCorrecting(pindex, pelement)) {
					// 答题错误时,错误件数加一
					__isFault++;
				} else {
					__isRight++;
				}
			});
		};

	return {
		printSetting: printSetting,
		printAfterSetting: printAfterSetting,
		ready: ready,
		makeCorrections: makeCorrections,
		theirPapers: theirPapers
	};
}());
