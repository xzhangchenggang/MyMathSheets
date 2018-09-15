﻿using MyMathSheets.CommonLib.Composition;
using MyMathSheets.CommonLib.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MyMathSheets.CommonLib.Main.Arithmetic
{
	/// <summary>
	/// 運算符對象生產工廠
	/// </summary>
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[Export(typeof(ICalculateFactory))]
	public class CalculateFactory : ICalculateFactory
	{
		/// <summary>
		/// 以防止重複注入（減少損耗）
		/// </summary>
		private bool _composed = false;

		/// <summary>
		/// 運算符檢索用的composer
		/// </summary>
		private readonly Composer _composer;

		/// <summary>
		/// 運算符處理類型緩存區
		/// </summary>
		private static readonly ConcurrentDictionary<Composer, ConcurrentDictionary<SignOfOperation, ICalculate>> ComposerCache = new ConcurrentDictionary<Composer, ConcurrentDictionary<SignOfOperation, ICalculate>>();

		/// <summary>
		/// 構造函數
		/// </summary>
		[ImportingConstructor]
		public CalculateFactory()
		{
			// 獲取計算式策略模塊Composer
			_composer = ComposerFactory.GetComporser(SystemModel.ComputationalStrategy);
		}

		/// <summary>
		/// 在MEF容器中收集本類的屬性信息
		/// </summary>
		private void ComposeThis()
		{
			// 工廠實例后只需要收集一次
			if (this._composed)
			{
				return;
			}
			// 從MEF容器中注入本類的屬性信息（注入運算符屬性）
			this._composer.Compose(this);
			// 以防止重複注入（減少損耗）
			this._composed = true;
		}

		/// <summary>
		/// 運算符屬性注入點
		/// </summary>
		[ImportMany(RequiredCreationPolicy = CreationPolicy.NonShared)]
		public IEnumerable<Lazy<CalculateBase, ICalculateMetaDataView>> _operations { get; set; }

		/// <summary>
		/// 對指定運算符實例化
		/// </summary>
		/// <param name="sign">運算符</param>
		/// <returns>運算符實例</returns>
		public ICalculate CreateCalculateInstance(SignOfOperation sign)
		{
			// 運算符對象緩存區管理
			ConcurrentDictionary<SignOfOperation, ICalculate> cacheStrategy = ComposerCache.GetOrAdd(_composer, _ => new ConcurrentDictionary<SignOfOperation, ICalculate>());
			// 返回緩衝區中的運算符對象
			return cacheStrategy.GetOrAdd(sign, (calculate) =>
			{
				// 在MEF容器中收集本類的屬性信息（實際情況屬性只注入一次）
				ComposeThis();
				// 指定運算符并獲取處理類型
				CalculateBase operation = _operations.Where(d => d.Metadata.Sign == sign).First().Value;
				// 返回該運算符處理類型的實例
				return (ICalculate)Activator.CreateInstance(operation.GetType());
			});
		}
	}
}