﻿using MyMathSheets.CommonLib.Util;

namespace MyMathSheets.CommonLib.Main.OperationStrategy
{
	/// <summary>
	/// 計算式策略對象生產工廠接口類
	/// </summary>
	public interface IOperationFactory
	{
		/// <summary>
		/// 對指定計算式策略實例化
		/// </summary>
		/// <param name="preview">策略種類</param>
		/// <returns>策略實例</returns>
		IOperation CreateOperationInstance(LayoutSetting.Preview preview);

		/// <summary>
		/// 對指定計算式策略所需參數的對象實例化
		/// </summary>
		/// <param name="preview"></param>
		/// <param name="identifier">參數識別ID（如果沒有指定參數標識，則默認返回當前參數序列的第一個參數項目）</param>
		/// <returns>對象實例</returns>
		ParameterBase CreateOperationParameterInstance(LayoutSetting.Preview preview, string identifier = "");
	}
}