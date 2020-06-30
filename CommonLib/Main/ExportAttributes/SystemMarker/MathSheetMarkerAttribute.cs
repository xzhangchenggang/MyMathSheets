﻿using MyMathSheets.CommonLib.Util;
using System;

namespace MyMathSheets.CommonLib
{
	/// <summary>
	/// 自定義屬性(系統模塊識別號)
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public sealed class MathSheetMarkerAttribute : Attribute
	{
		/// <summary>
		/// 構造函數
		/// </summary>
		/// <param name="id">模塊識別號</param>
		/// <param name="classify">題型分類</param>
		/// <param name="preview">子模塊識別號（題型模塊化對應）</param>
		public MathSheetMarkerAttribute(SystemModel id, LayoutSetting.Classify classify = LayoutSetting.Classify.Default, LayoutSetting.Preview preview = LayoutSetting.Preview.Null)
		{
			this.SystemId = id;
			this.Preview = preview;
			this.Classify = classify;
			// 題型名稱
			this.Description = preview.ToComputationalStrategyName();
		}

		/// <summary>
		/// 題型名稱
		/// </summary>
		public string Description
		{
			get;
			set;
		}

		/// <summary>
		/// 識別號
		/// </summary>
		public SystemModel SystemId
		{
			get;
			set;
		}

		/// <summary>
		/// 子模塊識別號（題型模塊化對應）
		/// </summary>
		public LayoutSetting.Preview Preview
		{
			get;
			set;
		}

		/// <summary>
		/// 題型分類
		/// </summary>
		public LayoutSetting.Classify Classify
		{
			get;
			set;
		}
	}
}