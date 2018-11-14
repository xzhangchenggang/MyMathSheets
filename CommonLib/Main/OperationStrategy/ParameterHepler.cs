﻿using MyMathSheets.CommonLib.Composition;
using MyMathSheets.CommonLib.Main.Provider;
using MyMathSheets.CommonLib.Provider;
using MyMathSheets.CommonLib.Util;
using System;
using System.Configuration;
using System.Linq;

namespace MyMathSheets.CommonLib.OperationStrategy
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ParameterHepler
	{
		/// <summary>
		/// 參數Provider實例化
		/// </summary>
		/// <param name="preview">題型種類</param>
		/// <returns>Provider實例</returns>
		public OperationParameterProvider CreateParameterProvider(string preview)
		{
			var section = (OperationProviderConfigurationSection)ConfigurationManager.GetSection("Operation");

			OperationParameterProvider provider = ComposerFactory.GetComporser(SystemModel.Common).GetExports<OperationParameterProvider, IParameterProviderMetaDataView>().Where(d => d.Metadata.Name.Equals(section.ProviderType)).FirstOrDefault().Value;

			OperationParameterProvider instance = (OperationParameterProvider)Activator.CreateInstance(provider.GetType());
			instance.Argument = string.Format(section.Argument, preview);
			// 實例化
			return instance;
		}
	}
}