﻿using MyMathSheets.CommonLib.Main.OperationStrategy;
using MyMathSheets.CommonLib.Util;
using MyMathSheets.ComputationalStrategy.FindTheLaw.Item;
using System;
using System.Collections.Generic;

namespace MyMathSheets.ComputationalStrategy.NumericSorting.Main.Parameters
{
	/// <summary>
	/// 數字排序參數類
	/// </summary>
	[OperationParameter(LayoutSetting.Preview.NumericSorting, "NS001")]
	public class NumericSortingParameter : ParameterBase
	{
		/// <summary>
		/// 數字排序作成并輸出
		/// </summary>
		public IList<NumericSortingFormula> Formulas { get; set; }

		/// <summary>
		/// 數字排序個數設置
		/// </summary>
		public int Amount { get; set; }

		/// <summary>
		/// 初期化參數
		/// </summary>
		public override void InitParameter()
		{
			base.InitParameter();

			Amount = Convert.ToInt32(JsonExtension.GetPropertyByJson(Reserve, "Amount"));

			// 數字排序集合實例化
			Formulas = new List<NumericSortingFormula>();
		}
	}
}
