﻿using MyMathSheets.CommonLib.Logging;
using MyMathSheets.CommonLib.Message;
using MyMathSheets.CommonLib.Util;
using MyMathSheets.ComputationalStrategy.Item;
using MyMathSheets.TestConsoleApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMathSheets.TestConsoleApp.Write
{
	/// <summary>
	/// 等式比大小题型计算式结果显示输出
	/// </summary>
	public class FindNearestNumberWrite : IConsoleWrite<List<EqualityFormula>>
	{
		private static Log log = Log.LogReady(typeof(FindNearestNumberWrite));

		/// <summary>
		/// 计算式结果显示输出
		/// </summary>
		/// <param name="formulas">计算式</param>
		public void ConsoleFormulas(List<EqualityFormula> formulas)
		{
			log.Debug(MessageUtil.GetException(() => MsgResources.I0004T, "等式比大小"));

			formulas.ToList().ForEach(d =>
			{
				Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6}",
					Util.GetValue(GapFilling.Left, d.LeftFormula.LeftParameter, d.LeftFormula.Gap),
					d.LeftFormula.Sign.ToOperationString(),
					Util.GetValue(GapFilling.Right, d.LeftFormula.RightParameter, d.LeftFormula.Gap),
					d.Answer.ToSignOfCompareString(),
					Util.GetValue(GapFilling.Left, d.RightFormula.LeftParameter, d.RightFormula.Gap),
					d.RightFormula.Sign.ToOperationString(),
					Util.GetValue(GapFilling.Right, d.RightFormula.RightParameter, d.RightFormula.Gap)));
			});
		}
	}
}
