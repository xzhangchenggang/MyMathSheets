﻿using MyMathSheets.CommonLib.Util;

namespace MyMathSheets.CommonLib.Main.Arithmetic
{
	/// <summary>
	/// 運算符自定導出的元數據特性（使用其導出的元數據以挑選需要的對象）
	/// </summary>
	public interface ICalculateMetaDataView
	{
		/// <summary>
		/// 運算符種類
		/// </summary>
		SignOfOperation Sign { get; }
	}
}