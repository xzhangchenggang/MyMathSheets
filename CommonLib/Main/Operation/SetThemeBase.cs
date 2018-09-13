﻿using MyMathSheets.CommonLib.Composition;
using MyMathSheets.CommonLib.Main.ArithmeticStrategy;
using MyMathSheets.CommonLib.Util;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MyMathSheets.CommonLib.Main.Operation
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class SetThemeBase<T> where T : new()
	{
		/// <summary>
		/// 
		/// </summary>
		protected T _formulas;
		/// <summary>
		/// 
		/// </summary>
		//private IObjectFactory _operatorObjectFactory;
		/// <summary>
		/// 
		/// </summary>
		private readonly Dictionary<string, ICalculatePattern> _cacheStrategy;
		/// <summary>
		/// 题型（标准、随机填空）
		/// </summary>
		protected QuestionType _questionType;
		/// <summary>
		/// 在四则运算标准题下指定运算法（加减乘除）
		/// </summary>
		protected IList<SignOfOperation> _signs;
		/// <summary>
		/// 四则运算类型（标准、随机出题）
		/// </summary>
		protected FourOperationsType _fourOperationsType;
		/// <summary>
		/// 运算结果最大限度值
		/// </summary>
		protected int _maximumLimit;
		/// <summary>
		/// 出题数量
		/// </summary>
		protected int _numberOfQuestions;
		/// <summary>
		/// 
		/// </summary>
		public T Formulas { get => _formulas; private set => _formulas = value; }

		/// <summary>
		/// 
		/// </summary>
		public SetThemeBase()
		{
			_formulas = new T();

			_cacheStrategy = new Dictionary<string, ICalculatePattern>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="maximumLimit">运算结果最大限度值</param>
		/// <param name="numberOfQuestions">出题数量</param>
		public SetThemeBase(int maximumLimit, int numberOfQuestions)
			: this()
		{
			_maximumLimit = maximumLimit;
			_numberOfQuestions = numberOfQuestions;
		}

		/// <summary>
		/// 
		/// </summary>
		public abstract void MarkFormulaList();

		/// <summary>
		/// 指定运算符获得相应的运算处理对象（实例）
		/// </summary>
		/// <param name="sign">运算符</param>
		/// <returns>运算处理对象（实例）</returns>
		protected ICalculatePattern GetPatternInstance(SignOfOperation sign)
		{
			if (!_cacheStrategy.ContainsKey(sign.ToString()))
			{
				var operations = ComposerFactory.GetComporser("ComputationalStrategy").GetExports<CalculatePatternBase, ICalculateMetadata>().Where(d => d.Metadata.Sign == sign);
				if (operations.Count() == 0)
				{
					throw new NullReferenceException();
				}
				_cacheStrategy.Add(sign.ToString(), (ICalculatePattern)Activator.CreateInstance(operations.First().Value.GetType()));
			}
			return _cacheStrategy[sign.ToString()];
		}
	}
}
