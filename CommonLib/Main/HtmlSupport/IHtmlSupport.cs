﻿using MyMathSheets.CommonLib.Main.OperationStrategy;

namespace MyMathSheets.CommonLib.Main.HtmlSupport
{
	/// <summary>
	/// HTML支援類接口
	/// </summary>
	public interface IHtmlSupport
	{
		/// <summary>
		/// HTML模板信息作成并返回
		/// </summary>
		/// <param name="parameter">計算式參數</param>
		/// <returns>HTML模板信息</returns>
		string Make(ParameterBase parameter);
	}
}