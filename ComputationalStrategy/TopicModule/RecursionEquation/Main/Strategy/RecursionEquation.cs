﻿using MyMathSheets.CommonLib.Logging;
using MyMathSheets.CommonLib.Main.Calculate;
using MyMathSheets.CommonLib.Main.Item;
using MyMathSheets.CommonLib.Main.Policy;
using MyMathSheets.CommonLib.Main.Policy.Attributes;
using MyMathSheets.CommonLib.Main.VirtualHelper;
using MyMathSheets.CommonLib.Util;
using MyMathSheets.ComputationalStrategy.Arithmetic.Main.Helper;
using MyMathSheets.ComputationalStrategy.RecursionEquation.Item;
using MyMathSheets.ComputationalStrategy.RecursionEquation.Main.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Text;

namespace MyMathSheets.ComputationalStrategy.RecursionEquation.Main.Strategy
{
	/// <summary>
	/// 遞等式計算
	/// </summary>
	[Topic("RecursionEquation")]
	public class RecursionEquation : TopicBase<RecursionEquationParameter>
	{
		/// <summary>
		/// 反推判定次數（如果大於五次則認為此題無法作成繼續下一題）
		/// </summary>
		private const int INVERSE_NUMBER = 3;

		/// <summary>
		/// 遞等式計算的實現方法集合
		/// </summary>
		private readonly Dictionary<TopicType, Action<IList<RecursionEquationFormula>>> _calculations = new Dictionary<TopicType, Action<IList<RecursionEquationFormula>>>();

		/// <summary>
		/// <see cref="RecursionEquation"/>構造函數
		/// </summary>
		[ImportingConstructor]
		public RecursionEquation()
		{
			// 遞等式計算[A+B+C]
			_calculations[TopicType.CleverA] = CleverA;
			// 遞等式計算[A-(B-C)]
			_calculations[TopicType.CleverB] = CleverB;
			// 遞等式計算[A-(B+C)]
			_calculations[TopicType.CleverC] = CleverC;
			// 遞等式計算[A+(B-C)]
			_calculations[TopicType.CleverD] = CleverD;
			// 遞等式計算[A+(B+C)]
			_calculations[TopicType.CleverE] = CleverE;
			// 遞等式計算[A+B-C]
			_calculations[TopicType.CleverF] = CleverF;
			// 遞等式計算[A-B+C]
			_calculations[TopicType.CleverG] = CleverG;
			// 遞等式計算[A-B-C]
			_calculations[TopicType.CleverH] = CleverH;
		}

		/// <summary>
		/// 算式作成
		/// </summary>
		/// <param name="p">題型參數</param>
		public override void MarkFormulaList(RecursionEquationParameter p)
		{
			// 算式作成
			MarkFormulaList(p, () =>
			{
				// 隨機子題型
				return CommonUtil.GetRandomNumber(p.TopicTypes);
			});

			// 智能提示作成
			VirtualHelperBase<RecursionEquationFormula> helper = new RecursionEquationDialogue();
			p.BrainpowerHint = helper.CreateHelperDialogue(p.Formulas);
		}

		/// <summary>
		/// 算式作成
		/// </summary>
		/// <param name="p">題型參數</param>
		/// <param name="topicTypeFunc">運算符取得用的表達式</param>
		private void MarkFormulaList(RecursionEquationParameter p, Func<TopicType> topicTypeFunc)
		{
			// 按照指定數量作成相應的數學計算式
			for (var i = 0; i < p.NumberOfQuestions; i++)
			{
				if (_calculations.TryGetValue(topicTypeFunc(), out Action<IList<RecursionEquationFormula>> calculation))
				{
					calculation(p.Formulas);
				}
			}
		}

		/// <summary>
		/// 遞等式計算通用處理
		/// </summary>
		/// <param name="type">題型</param>
		/// <param name="expressFormat">計算表達式</param>
		/// <param name="getArguments">參數處理邏輯</param>
		/// <param name="formulas">計算式作成</param>
		private void CleverStartegy(TopicType type, string expressFormat, Func<int[]> getArguments, IList<RecursionEquationFormula> formulas)
		{
			// 當前反推判定次數（一次推算內次數累加）
			int defeated = 0;

			RecursionEquationFormula RecursionEquation = new RecursionEquationFormula
			{
				Type = type,
				ConfixFormulas = new List<Formula>(),
				Answer = new List<int>()
			};

			while (1 == 1)
			{
				// 如果大於三次則認為此題無法作成繼續下一題
				if (defeated == INVERSE_NUMBER)
				{
					RecursionEquation = null;
					break;
				}

				int[] factors = getArguments();
				if (factors == null)
				{
					defeated++;
					continue;
				}

				StringBuilder express = new StringBuilder();
				var answer = factors[3];
				express.AppendFormat(CultureInfo.CurrentCulture, expressFormat, factors[0], factors[1], factors[2]);

				// 計算式推導
				var calc = new ExpressArithmeticUtil();
				if (!calc.IsResult(express.ToString(), out int result))
				{
					defeated++;
					continue;
				}
				if (result != answer)
				{
					defeated++;
					continue;
				}
				// 加入推導出計算式集合
				calc.Formulas.ToList().ForEach(f => RecursionEquation.ConfixFormulas.Add(f));
				RecursionEquation.Answer.Add(answer);
				defeated = 0;

				break;
			}

			if (RecursionEquation != null)
			{
				formulas.Add(RecursionEquation);
			}
		}

		#region 遞等式計算

		/// <summary>
		/// 遞等式計算[A-B-C]
		/// </summary>
		/// <param name="formulas">計算式作成</param>
		/// <remarks>
		/// 68-44-18 -> 68-18-44
		/// </remarks>
		protected virtual void CleverH(IList<RecursionEquationFormula> formulas)
		{
			CleverStartegy(TopicType.CleverH, "{0}-{1}-{2}",
				() =>
				{
					// (A-C)參數差值（獲取三位數(整十位或者整百位)）
					var diff = CommonUtil.GetRandomNumber(160, 350, condition: _ => _ % 10 == 0, getDefault: () => 160);
					// 參數C (*增加參數B的取值範圍) eg: 162-(44+38)
					var argumentC = CommonUtil.GetRandomNumber(50, 150);
					// 另一個隨機取值方案（整十數湊百）eg: 160+(44-40)
					if (1 == CommonUtil.GetRandomNumber(1, 2))
					{
						diff = CommonUtil.GetRandomNumber(160, 350, condition: _ => _ % 100 == 0, () => 200);
						argumentC = CommonUtil.GetRandomNumber(50, 150, condition: _ => _ % 10 == 0 && _ % 100 != 0, () => 140);
					}
					// 參數A
					var argumentA = diff + argumentC;
					// 參數B
					var argumentB = CommonUtil.GetRandomNumber(argumentC, diff,
																	condition: b => b != argumentC
																				&& (argumentA - b) % 10 != 0
																				&& (argumentC + b) % 10 != 0,
																	getDefault: () => -1);
					if (argumentB == -1)
					{
						return null;
					}

					// 結果值
					var answer = diff - argumentB;

					// 隨機排列計算式各參數（打亂參數B,C 位置）
					SetNewGuidFactors(out List<int> factors1, argumentB, argumentC);
					return new int[] { argumentA, factors1[0], factors1[1], answer };
				}, formulas);
		}

		/// <summary>
		/// 遞等式計算[A-B+C]
		/// </summary>
		/// <param name="formulas">計算式作成</param>
		/// <remarks>
		/// 68-44+12 -> 68+12-44
		/// </remarks>
		protected virtual void CleverG(IList<RecursionEquationFormula> formulas)
		{
			CleverStartegy(TopicType.CleverG, "{0}-{1}+{2}",
				() =>
				{
					// (A+C)參數合計（獲取三位數(整十位或者整百位)）
					var argumentSum = CommonUtil.GetRandomNumber(300, 550, condition: _ => _ % 10 == 0, getDefault: () => 400);
					// 參數C (*增加參數B的取值範圍) eg: 162-44+38
					var argumentC = CommonUtil.GetRandomNumber(100, 250, condition: _ => _ % 100 != 0, getDefault: () => 128);
					// 另一個隨機取值方案（整十數湊百）eg: 160-44+40)
					if (1 == CommonUtil.GetRandomNumber(1, 2))
					{
						argumentSum = CommonUtil.GetRandomNumber(300, 550, condition: _ => _ % 100 == 0, getDefault: () => 400);
						argumentC = CommonUtil.GetRandomNumber(100, 250, condition: _ => _ % 10 == 0 && _ % 100 != 0, getDefault: () => 180);
					}
					// 參數A
					var argumentA = argumentSum - argumentC;
					if (argumentA % 100 == 0)
					{
						var diff = CommonUtil.GetRandomNumber(1, 100, condition: _ => _ % 10 == 0, getDefault: () => 1);
						argumentA += diff;
						argumentSum += diff;
					}
					// 參數B
					var argumentB = CommonUtil.GetRandomNumber(50, argumentA, condition: b => b != argumentC && (b + argumentA) % 10 != 0, getDefault: () => -1);
					if (argumentB == -1)
					{
						return null;
					}
					// 結果值
					var answer = argumentSum - argumentB;

					// 參數不能隨便交換位置
					return new int[] { argumentA, argumentB, argumentC, answer };
				}, formulas);
		}

		/// <summary>
		/// 遞等式計算[A+B-C]
		/// </summary>
		/// <param name="formulas">計算式作成</param>
		/// <remarks>
		/// 68+44-18 -> 68+44-18 -> 68-18+44
		/// </remarks>
		protected virtual void CleverF(IList<RecursionEquationFormula> formulas)
		{
			CleverStartegy(TopicType.CleverF, "{0}+{1}-{2}",
				() =>
				{
					// (A-C)參數差值（獲取三位數(整十位或者整百位)）
					var diff = CommonUtil.GetRandomNumber(160, 350, condition: _ => _ % 10 == 0, getDefault: () => 160);
					// 參數C (*增加參數B的取值範圍) eg: 162+44-62
					var argumentC = CommonUtil.GetRandomNumber(50, 150);
					// 另一個隨機取值方案（整十數湊百）eg: 160+44-60
					if (1 == CommonUtil.GetRandomNumber(1, 2))
					{
						diff = CommonUtil.GetRandomNumber(160, 350, condition: _ => _ % 100 == 0, getDefault: () => 200);
						argumentC = CommonUtil.GetRandomNumber(50, 150, condition: _ => _ % 10 == 0 && _ % 100 != 0, getDefault: () => 50);
					}
					// 參數A
					var argumentA = diff + argumentC;
					// 參數B
					var argumentB = CommonUtil.GetRandomNumber(argumentC, diff, condition: b => b != argumentC && (argumentA - b) % 10 != 0, getDefault: () => -1);
					if (argumentB == -1)
					{
						return null;
					}
					// 結果值
					var answer = diff + argumentB;

					// 參數不能隨便交換位置
					return new int[] { argumentA, argumentB, argumentC, answer };
				}, formulas);
		}

		/// <summary>
		/// 遞等式計算[A+(B+C)]
		/// </summary>
		/// <param name="formulas">計算式作成</param>
		/// <remarks>
		/// 68+(44+12) -> 68+44+12 -> 68+12+44
		/// </remarks>
		protected virtual void CleverE(IList<RecursionEquationFormula> formulas)
		{
			CleverStartegy(TopicType.CleverE, "{0}+({1}+{2})",
				() =>
				{
					// (A+C)參數合計（獲取三位數(整十位或者整百位)）
					var argumentSum = CommonUtil.GetRandomNumber(300, 550, condition: _ => _ % 10 == 0, getDefault: () => 400);
					// 參數C (*增加參數B的取值範圍)
					var argumentC = CommonUtil.GetRandomNumber(100, 250, condition: _ => _ % 10 != 0, getDefault: () => 128);
					// 另一個隨機取值方案（整十數湊百）
					if (1 == CommonUtil.GetRandomNumber(1, 2))
					{
						argumentSum = CommonUtil.GetRandomNumber(300, 550, condition: _ => _ % 100 == 0, getDefault: () => 400);
						argumentC = CommonUtil.GetRandomNumber(100, 250, condition: _ => _ % 10 == 0 && _ % 100 != 0, getDefault: () => 180);
					}

					// 參數A
					var argumentA = argumentSum - argumentC;
					if (argumentA % 100 == 0)
					{
						var diff = CommonUtil.GetRandomNumber(1, 100, condition: _ => _ % 10 == 0, getDefault: () => 50);
						argumentA += diff;
						argumentSum += diff;
					}
					// 參數B
					var argumentB = CommonUtil.GetRandomNumber(50, 250,
																	condition: b => b != argumentC && (b + argumentA) % 10 != 0 && (b + argumentC) % 10 != 0,
																	getDefault: () => -1);
					if (argumentB == -1)
					{
						return null;
					}

					// 結果值
					var answer = argumentSum + argumentB;

					// 隨機排列計算式各參數（打亂參數A,C 位置）
					SetNewGuidFactors(out List<int> factors1, argumentA, argumentC);
					SetNewGuidFactors(out List<int> factors2, factors1[1], argumentB);
					return new int[] { factors1[0], factors2[0], factors2[1], answer };
				}, formulas);
		}

		/// <summary>
		/// 遞等式計算[A+(B-C)]
		/// </summary>
		/// <param name="formulas">計算式作成</param>
		/// <remarks>
		/// 62+(44-12) -> 62+44-12 -> 62-12+44
		/// </remarks>
		protected virtual void CleverD(IList<RecursionEquationFormula> formulas)
		{
			CleverStartegy(TopicType.CleverD, "{0}+({1}-{2})",
				() =>
				{
					// (A-C)參數差值（獲取三位數(整十位或者整百位)）
					var diff = CommonUtil.GetRandomNumber(160, 350, condition: _ => _ % 10 == 0, getDefault: () => 160);
					// 參數C (*增加參數B的取值範圍) eg: 162+(44-62)
					var argumentC = CommonUtil.GetRandomNumber(50, 150);
					// 另一個隨機取值方案（整十數湊百）eg: 160+(44-60)
					if (1 == CommonUtil.GetRandomNumber(1, 2))
					{
						diff = CommonUtil.GetRandomNumber(160, 350, condition: _ => _ % 100 == 0, getDefault: () => 200);
						argumentC = CommonUtil.GetRandomNumber(50, 150, condition: _ => _ % 10 == 0 && _ % 100 != 0, getDefault: () => 50);
					}
					// 參數A
					var argumentA = diff + argumentC;
					// 參數B
					var argumentB = CommonUtil.GetRandomNumber(argumentC, diff, condition: b => b != argumentC && (argumentA - b) % 10 != 0, getDefault: () => -1);
					if (argumentB == -1)
					{
						return null;
					}
					// 結果值
					var answer = diff + argumentB;

					// 參數不能隨便交換位置
					return new int[] { argumentA, argumentB, argumentC, answer };
				}, formulas);
		}

		/// <summary>
		/// 遞等式計算[A-(B+C)]
		/// </summary>
		/// <param name="formulas">計算式作成</param>
		/// <remarks>
		/// 62-(44+12) -> 62-44-12
		/// </remarks>
		protected virtual void CleverC(IList<RecursionEquationFormula> formulas)
		{
			CleverStartegy(TopicType.CleverC, "{0}-({1}+{2})",
				() =>
				{
					// (A-C)參數差值（獲取三位數(整十位或者整百位)）
					var diff = CommonUtil.GetRandomNumber(160, 350, condition: _ => _ % 10 == 0, getDefault: () => 160);
					// 參數C (*增加參數B的取值範圍) eg: 162-(44+38)
					var argumentC = CommonUtil.GetRandomNumber(50, 150);
					// 另一個隨機取值方案（整十數湊百）eg: 160+(44-40)
					if (1 == CommonUtil.GetRandomNumber(1, 2))
					{
						diff = CommonUtil.GetRandomNumber(160, 350, condition: _ => _ % 100 == 0, getDefault: () => 200);
						argumentC = CommonUtil.GetRandomNumber(50, 150, condition: _ => _ % 10 == 0 && _ % 100 != 0, getDefault: () => 50);
					}
					// 參數A
					var argumentA = diff + argumentC;
					// 參數B
					var argumentB = CommonUtil.GetRandomNumber(argumentC, diff, condition: b => b != argumentC && (argumentA - b) % 10 != 0, getDefault: () => -1);
					if (argumentB == -1)
					{
						return null;
					}
					// 結果值
					var answer = diff - argumentB;

					// 參數不能隨便交換位置
					return new int[] { argumentA, argumentB, argumentC, answer };
				}, formulas);
		}

		/// <summary>
		/// 遞等式計算[A-(B-C)]
		/// </summary>
		/// <param name="formulas">計算式作成</param>
		/// <remarks>
		/// 68-(44-12) -> 68-44+12 -> 68+12-44
		/// </remarks>
		protected virtual void CleverB(IList<RecursionEquationFormula> formulas)
		{
			CleverStartegy(TopicType.CleverB, "{0}-({1}-{2})",
				() =>
				{
					CreateArgumentsForSum(out int argumentA, out int argumentB);
					// 確保被減數為最大值
					if (argumentA < argumentB)
					{
						var tmp = argumentB;
						argumentB = argumentA;
						argumentA = argumentB;
					}

					// 參數C
					var argumentC = 0;
					while (1 == 1)
					{
						argumentC = CommonUtil.GetRandomNumber(argumentB - 1, argumentA - 1);
						// 避免AB或者AC之間差值為整十數
						if ((argumentC - argumentB) % 10 == 0 || (argumentA - argumentC) % 10 == 0)
						{
							continue;
						}
						break;
					}

					// 結果值
					var answer = argumentA + argumentB - argumentC;

					// 參數不能隨便交換位置
					return new int[] { argumentA, argumentC, argumentB, answer };
				}, formulas);
		}

		/// <summary>
		/// 基於兩個參數之和策略的隨機取值方案
		/// </summary>
		/// <param name="argumentA">參數</param>
		/// <param name="argumentB">參數</param>
		private void CreateArgumentsForSum(out int argumentA, out int argumentB)
		{
			// 參數合計（獲取三位數(整數十位或者整百數 100 ~ 500)）
			var argumentSum = CommonUtil.GetRandomNumber(1, 4) * 100 + CommonUtil.GetRandomNumber(0, 10) * 10;
			// 如果合計值是200以內, 隨機取值兩位數
			if (argumentSum < 200)
			{
				// （個位湊十）eg: 45 + 13 + 137
				argumentA = CommonUtil.GetRandomNumber(1, 9) * 10 + CommonUtil.GetRandomNumber(1, 9);
			}
			// 除上述情況以外(整百數)
			else if (argumentSum % 100 == 0)
			{
				// （十位湊百）eg: 45 + 30 + 370
				argumentA = CommonUtil.GetRandomNumber(1, (argumentSum / 100) - 1) * 100 + CommonUtil.GetRandomNumber(1, 9) * 10;
			}
			// 上述情況以外,隨機取值三位數
			else
			{
				// 隨機取值方案（個位湊十）eg: 45 + 132 + 238
				argumentA = CommonUtil.GetRandomNumber(1, (argumentSum / 100) - 1) * 100 + CommonUtil.GetRandomNumber(1, 9) * 10 + CommonUtil.GetRandomNumber(1, 9);
			}

			// 參數C
			argumentB = argumentSum - argumentA;
		}

		/// <summary>
		/// 遞等式計算[A+B+C]
		/// </summary>
		/// <param name="formulas">計算式作成</param>
		/// <remarks>
		/// 22+44+56 -> 22+(44+56)
		/// </remarks>
		protected virtual void CleverA(IList<RecursionEquationFormula> formulas)
		{
			CleverStartegy(TopicType.CleverA, "{0}+{1}+{2}",
				() =>
				{
					CreateArgumentsForSum(out int argumentA, out int argumentB);

					// 參數A
					var argumentC = CommonUtil.GetRandomNumber(100, 300);
					// 避免AB或者AC相加和為整十數
					if ((argumentC + argumentA) % 10 == 0 || (argumentC + argumentB) % 10 == 0)
					{
						argumentC -= 1;
					}
					// 結果值
					var answer = argumentC + argumentA + argumentB;

					// 隨機排列計算式各參數（打亂參數位置）
					SetNewGuidFactors(out List<int> factors, argumentA, argumentB, argumentC);
					factors.Add(answer);

					return factors.ToArray();
				}, formulas);
		}

		#endregion 遞等式計算

		/// <summary>
		/// 隨機排列計算式各參數
		/// </summary>
		/// <param name="factors">參數列</param>
		/// <param name="arguments">參數集合</param>
		/// <remarks>
		/// 5 * 10 + 2 * 5
		/// 計算式中的參數位置可以有以下情況：
		/// 10 * 5 + 2 * 5
		/// 5 * 10 + 5 * 2
		/// 等等，顧需要隨機打亂其中的擺放順序
		/// 注：共有因數必須兩邊都有
		/// </remarks>
		private void SetNewGuidFactors(out List<int> factors, params int[] arguments)
		{
			// 隨機排序
			factors = arguments.OrderBy(c => Guid.NewGuid()).ToList();
		}
	}
}