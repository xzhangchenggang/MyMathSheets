﻿using MyMathSheets.CommonLib.Main.HtmlSupport;
using MyMathSheets.CommonLib.Main.HtmlSupport.Attributes;
using MyMathSheets.CommonLib.Main.OperationStrategy;
using MyMathSheets.CommonLib.Util;
using MyMathSheets.ComputationalStrategy.CurrencyOperation.Item;
using MyMathSheets.ComputationalStrategy.LearnCurrency.Main.Parameters;
using System.Text;

namespace MyMathSheets.TheFormulaShows.CurrencyOperation.Support
{
	/// <summary>
	/// 
	/// </summary>
	[HtmlSupport(LayoutSetting.Preview.CurrencyOperation)]
	[Substitute("<!--CURRENCYOPERATIONSCRIPT-->", "<script src=\"../Scripts/Ext/MathSheets.CurrencyOperation.js\" charset=\"utf-8\"></script>")]
	[Substitute("//<!--CURRENCYOPERATIONREADY-->", "MathSheets.CurrencyOperation.ready();")]
	[Substitute("//<!--CURRENCYOPERATIONMAKECORRECTIONS-->", "fault += MathSheets.CurrencyOperation.makeCorrections();")]
	[Substitute("//<!--CURRENCYOPERATIONTHEIRPAPERS-->", "MathSheets.CurrencyOperation.theirPapers();")]
	[Substitute("//<!--CURRENCYOPERATIONPRINTSETTING-->", "MathSheets.CurrencyOperation.printSetting();")]
	[Substitute("//<!--CURRENCYOPERATIONPRINTAFTERSETTING-->", "MathSheets.CurrencyOperation.printAfterSetting();")]
	public class CurrencyOperationHtmlSupport : HtmlSupportBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		protected override string MakeHtmlStatement(ParameterBase parameter)
		{
			CurrencyOperationParameter p = parameter as CurrencyOperationParameter;

			if (p.Formulas.Count == 0)
			{
				return string.Empty;
			}

			int numberOfColumns = 0;
			bool isRowHtmlClosed = false;

			int controlIndex = 0;
			StringBuilder html = new StringBuilder();
			StringBuilder rowHtml = new StringBuilder();
			StringBuilder colHtml = new StringBuilder();
			foreach (CurrencyOperationFormula item in p.Formulas)
			{
				isRowHtmlClosed = false;
				colHtml.AppendLine("<div class=\"col-md-3 form-inline\">");
				colHtml.AppendLine("<h5>");

				if (item.AnswerIsRight)
				{
					colHtml.AppendLine(GetHtml(item.CurrencyArithmetic.Gap, item.CurrencyArithmetic.LeftParameter, GapFilling.Left, controlIndex));
					colHtml.AppendLine(string.Format("<span class=\"label\">{0}</span>", item.CurrencyArithmetic.Sign.ToOperationString()));
					colHtml.AppendLine(GetHtml(item.CurrencyArithmetic.Gap, item.CurrencyArithmetic.RightParameter, GapFilling.Right, controlIndex));
					colHtml.AppendLine("<span class=\"label\">=</span>");
					colHtml.AppendLine(GetHtml(item.CurrencyArithmetic.Gap, item.CurrencyArithmetic.Answer, GapFilling.Answer, controlIndex));
				}
				else
				{
					colHtml.AppendLine(GetHtml(item.CurrencyArithmetic.Gap, item.CurrencyArithmetic.Answer, GapFilling.Answer, controlIndex));
					colHtml.AppendLine("<span class=\"label\">=</span>");
					colHtml.AppendLine(GetHtml(item.CurrencyArithmetic.Gap, item.CurrencyArithmetic.LeftParameter, GapFilling.Left, controlIndex));
					colHtml.AppendLine(string.Format("<span class=\"label\">{0}</span>", item.CurrencyArithmetic.Sign.ToOperationString()));
					colHtml.AppendLine(GetHtml(item.CurrencyArithmetic.Gap, item.CurrencyArithmetic.RightParameter, GapFilling.Right, controlIndex));
				}

				colHtml.AppendLine(string.Format("<img id=\"imgOKCurrencyOperation{0}\" src=\"../Content/image/correct.png\" style=\"width: 40px; height: 40px; display: none; \" />", controlIndex));
				colHtml.AppendLine(string.Format("<img id=\"imgNoCurrencyOperation{0}\" src=\"../Content/image/fault.png\" style=\"width: 40px; height: 40px; display: none; \" />", controlIndex));
				colHtml.AppendLine("</h5>");
				colHtml.AppendLine("</div>");

				controlIndex++;
				numberOfColumns++;
				if (numberOfColumns == 4)
				{
					rowHtml.AppendLine("<div class=\"row text-center row-margin-top\">");
					rowHtml.Append(colHtml.ToString());
					rowHtml.AppendLine("</div>");

					html.Append(rowHtml);

					isRowHtmlClosed = true;
					numberOfColumns = 0;
					rowHtml.Length = 0;
					colHtml.Length = 0;
				}
			}

			if (!isRowHtmlClosed)
			{
				rowHtml.AppendLine("<div class=\"row text-center row-margin-top\">");
				rowHtml.Append(colHtml.ToString());
				rowHtml.AppendLine("</div>");

				html.Append(rowHtml);
			}

			if (html.Length != 0)
			{
				html.Insert(0, "<br/><div class=\"page-header\"><h4><img src=\"../Content/image/homework.png\" width=\"30\" height=\"30\" /><span style=\"padding: 8px\">四則運算</span></h4></div><hr />");
			}

			return html.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="parameter"></param>
		/// <param name="gap"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		private string GetHtml(GapFilling item, int parameter, GapFilling gap, int index)
		{
			var html = string.Empty;
			if (item == gap)
			{
				html += string.Format("<input id=\"inputAc{0}\" type = \"text\" placeholder=\" ?? \" class=\"form-control input-addBorder\" style=\"width: 50px; text-align:center;\" disabled=\"disabled\" onkeyup=\"if(!/^\\d+$/.test(this.value)) this.value='';\" />", index);
				html += string.Format("<input id=\"hiddenAc{0}\" type=\"hidden\" value=\"{1}\"/>", index, parameter);
			}
			else
			{
				html = string.Format("<span class=\"label\">{0}</span>", parameter);
			}
			return html;
		}
	}
}
