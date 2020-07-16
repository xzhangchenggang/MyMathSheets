﻿using MyMathSheets.CommonLib.Logging;
using MyMathSheets.CommonLib.Main.Policy;
using MyMathSheets.CommonLib.Message;
using MyMathSheets.ComputationalStrategy.CombinatorialEquation.Main.Parameters;
using MyMathSheets.TestConsoleApp.Properties;
using MyMathSheets.TestConsoleApp.Util;
using System;
using System.Linq;

namespace MyMathSheets.TestConsoleApp.Write
{
	/// <summary>
	/// 算式組合题型计算式结果显示输出
	/// </summary>
	public class CombinatorialFormulaWrite : IConsoleWrite
	{
		/// <summary>
		/// 計算結果顯示輸出
		/// </summary>
		/// <param name="parameter">參數</param>
		public void ConsoleFormulas(TopicParameterBase parameter)
		{
			LogUtil.LogDebug(MessageUtil.GetMessage(() => MsgResources.I0004T, "算式組合"));

			CombinatorialEquationParameter param = (CombinatorialEquationParameter)parameter;

			param.Formulas.ToList().ForEach(d =>
			{
				Console.WriteLine(string.Format("{0} {1} {2} {3}", d.ParameterA, d.ParameterB, d.ParameterC, d.ParameterD));
				d.CombinatorialFormulas.ToList().ForEach(m =>
				{
					Console.WriteLine(string.Format("{0} {1} {2} = {3}",
										m.LeftParameter,
										m.Sign.ToOperationString(),
										m.RightParameter,
										m.Answer));
				});
			});
		}
	}
}