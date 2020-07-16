﻿using MyMathSheets.CommonLib.Composition;
using MyMathSheets.CommonLib.Logging;
using MyMathSheets.CommonLib.Message;
using MyMathSheets.CommonLib.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MyMathSheets.CommonLib.Main.Policy
{
	/// <summary>
	/// 運算符對象生產工廠
	/// </summary>
	[Export(typeof(ITopicFactory)), PartCreationPolicy(CreationPolicy.Shared)]
	public class TopicFactory : ITopicFactory
	{
		/// <summary>
		/// 運算符檢索用的composer
		/// </summary>
		private Composer _composer;

		/// <summary>
		/// 類型對象緩存
		/// </summary>
		/// <remarks>
		/// <see cref="Composer"/>是以題型為管理單位
		/// </remarks>
		private static readonly ConcurrentDictionary<Composer, Lazy<TopicBase, IOperationMetaDataView>> OperationCache
										= new ConcurrentDictionary<Composer, Lazy<TopicBase, IOperationMetaDataView>>();

		/// <summary>
		/// 類型參數對象緩存
		/// </summary>
		private static readonly ConcurrentDictionary<string, Lazy<TopicParameterBase, IOperationMetaDataView>> ParameterCache
										= new ConcurrentDictionary<string, Lazy<TopicParameterBase, IOperationMetaDataView>>();

		/// <summary>
		/// 構造函數
		/// </summary>
		[ImportingConstructor]
		public TopicFactory()
		{
		}

		/// <summary>
		/// 運算符屬性注入點
		/// </summary>
		[ImportMany(RequiredCreationPolicy = CreationPolicy.NonShared)]
		public IEnumerable<Lazy<TopicBase, IOperationMetaDataView>> Operations { get; set; }

		/// <summary>
		/// 對指定計算式策略實例化
		/// </summary>
		/// <param name="topicIdentifier">題型識別ID</param>
		/// <returns>策略實例</returns>
		public virtual ITopic CreateOperationInstance(string topicIdentifier)
		{
			// 以題型為單位取得Composer
			_composer = ComposerFactory.GetComporser(topicIdentifier);

			// 返回緩衝區中的運算符對象
			Lazy<TopicBase, IOperationMetaDataView> lazyOperation = OperationCache.GetOrAdd(_composer, (o) =>
			{
				// 內部部件組合
				_composer.Compose(this);

				// 指定運算符并獲取處理類型
				IEnumerable<Lazy<TopicBase, IOperationMetaDataView>> operations = Operations.Where(
					d =>
					{
						return d.Metadata.Layout.Equals(topicIdentifier, StringComparison.CurrentCultureIgnoreCase);
					});
				// 部件是否存在
				if (!operations.Any())
				{
					// 指定的題型策略對象未找到
					throw new TopicNotFoundException(MessageUtil.GetMessage(() => MsgResources.E0018L, topicIdentifier));
				}
				LogUtil.LogDebug(MessageUtil.GetMessage(() => MsgResources.I0003L));

				return operations.First();
			});

			// 返回該運算符處理類型的實例（實例化）
			var operation = lazyOperation.Value;
			// 內部部件組合（策略抽象類中的計算式工廠對象注入）
			_composer.Compose(operation);
			return operation;
		}

		/// <summary>
		/// 對指定計算式策略所需參數的對象實例化
		/// </summary>
		/// <param name="topicIdentifier">題型種類</param>
		/// <param name="topicNumber">參數識別ID</param>
		/// <returns>對象實例</returns>
		public virtual TopicParameterBase CreateOperationParameterInstance(string topicIdentifier, string topicNumber)
		{
			// 參數對象緩存區管理
			string key = $"{topicIdentifier}::{topicNumber}";

			Lazy<TopicParameterBase, IOperationMetaDataView> lazyParameter = ParameterCache.GetOrAdd(key, (o) =>
			{
				// 注入運算符參數對象（題型與參數是一對一關係）
				IEnumerable<Lazy<TopicParameterBase, IOperationMetaDataView>> parameters = _composer.GetExports<TopicParameterBase, IOperationMetaDataView>();
				if (!parameters.Any())
				{
					// 指定的題型參數對象未找到
					throw new TopicNotFoundException(MessageUtil.GetMessage(() => MsgResources.E0019L, topicIdentifier));
				}
				LogUtil.LogDebug(MessageUtil.GetMessage(() => MsgResources.I0004L));

				return parameters.First();
			});

			// 參數類實例化
			TopicParameterBase paramater = lazyParameter.Value;
			// 通用參數初期化處理（依據Provider配置）
			paramater.InitParameterBase(key);
			// 派生類參數初期化（各子類實現）
			paramater.InitParameter();

			LogUtil.LogDebug(MessageUtil.GetMessage(() => MsgResources.I0005L, key));

			return paramater;
		}
	}
}