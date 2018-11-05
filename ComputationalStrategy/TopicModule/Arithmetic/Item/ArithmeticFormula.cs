﻿using MyMathSheets.CommonLib.Util;

namespace MyMathSheets.CommonLib.Main.Item
{
	/// <summary>
	/// 计算式对象构成
	/// </summary>
	public class ArithmeticFormula
	{
		/// <summary>
		/// 默認情況計算式有解
		/// </summary>
		public Formula Arithmetic { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool AnswerIsRight { get; set; }
	}
}