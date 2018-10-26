﻿using MyMathSheets.CommonLib.Main.HtmlSupport;
using MyMathSheets.CommonLib.Main.HtmlSupport.Attributes;
using MyMathSheets.CommonLib.Main.OperationStrategy;
using MyMathSheets.CommonLib.Util;
using MyMathSheets.ComputationalStrategy.MathWordProblems.Item;
using MyMathSheets.ComputationalStrategy.MathWordProblems.Main.Parameters;
using System.Text;

namespace MyMathSheets.TheFormulaShows.MathWordProblems.Support
{
	/// <summary>
	/// 
	/// </summary>
	[HtmlSupport(LayoutSetting.Preview.MathWordProblems)]
	[Substitute("<!--MATHWORDPROBLEMSSCRIPT-->", "<script src=\"../Scripts/Ext/MathSheets.MathWordProblems.js\" charset=\"utf-8\"></script>")]
	[Substitute("//<!--MATHWORDPROBLEMSREADY-->", "MathSheets.MathWordProblems.ready();")]
	[Substitute("//<!--MATHWORDPROBLEMSMAKECORRECTIONS-->", "fault += MathSheets.MathWordProblems.makeCorrections();")]
	[Substitute("//<!--MATHWORDPROBLEMSTHEIRPAPERS-->", "MathSheets.MathWordProblems.theirPapers();")]
	[Substitute("//<!--MATHWORDPROBLEMSPRINTSETTING-->", "MathSheets.MathWordProblems.printSetting();")]
	[Substitute("//<!--MATHWORDPROBLEMSPRINTAFTERSETTING-->", "MathSheets.MathWordProblems.printAfterSetting();")]
	public class MathWordProblemsHtmlSupport : HtmlSupportBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		protected override string MakeHtmlStatement(ParameterBase parameter)
		{
			MathWordProblemsParameter p = parameter as MathWordProblemsParameter;

			if (p.Formulas.Count == 0)
			{
				return string.Empty;
			}

			int parentControlIndex = 0;
			StringBuilder html = new StringBuilder();
			StringBuilder rowHtml = new StringBuilder();
			foreach (MathWordProblemsFormula item in p.Formulas)
			{
				rowHtml.AppendLine("<div class=\"col-md-12 form-inline\">");
				rowHtml.AppendLine("<h5>");
				rowHtml.AppendLine("<p class=\"text-info\">");
				rowHtml.AppendLine(string.Format("<span class=\"label\">{0}</span>", item.MathWordProblem));
				rowHtml.AppendLine("</p>");
				rowHtml.AppendLine("</h5>");
				rowHtml.AppendLine("</div>");
				rowHtml.AppendLine("<div class=\"col-md-12 form-inline\">");
				rowHtml.AppendLine("<h5>");
				rowHtml.AppendLine(string.Format("<input id=\"inputMwp{0}{1}\" type = \"text\" placeholder=\" ?? \" class=\"form-control input-addBorder\" style=\"width: 50px; text-align:center;\" disabled=\"disabled\" onkeyup=\"if(!/^\\d+$/.test(this.value)) this.value='';\" />", parentControlIndex, 0));
				rowHtml.AppendLine(string.Format("<img src=\"../Content/image/help.png\" id=\"imgMwp{0}\" style=\"width: 30px; height: 30px;\" title=\"help\" />", parentControlIndex));
				rowHtml.AppendLine(string.Format("<input id=\"inputMwp{0}{1}\" type = \"text\" placeholder=\" ?? \" class=\"form-control input-addBorder\" style=\"width: 50px; text-align:center;\" disabled=\"disabled\" onkeyup=\"if(!/^\\d+$/.test(this.value)) this.value='';\" />", parentControlIndex, 1));
				rowHtml.AppendLine("<img src=\"../Content/image/calculator.png\" style=\"width: 30px; height: 30px;\" />");
				rowHtml.AppendLine(string.Format("<input id=\"inputMwp{0}{1}\" type = \"text\" placeholder=\" ?? \" class=\"form-control input-addBorder\" style=\"width: 50px; text-align:center;\" disabled=\"disabled\" onkeyup=\"if(!/^\\d+$/.test(this.value)) this.value='';\" />", parentControlIndex, 2));
				rowHtml.AppendLine(string.Format("<input id=\"hiddenMwp{0}\" type=\"hidden\" value=\"{1}\" />", parentControlIndex, item.Verify));
				rowHtml.AppendLine(string.Format("<img id=\"imgOKProblems{0}\" src=\"../Content/image/correct.png\" style=\"width: 40px; height: 40px; display: none; \" />", parentControlIndex));
				rowHtml.AppendLine(string.Format("<img id=\"imgNoProblems{0}\" src=\"../Content/image/fault.png\" style=\"width: 40px; height: 40px; display: none; \" />", parentControlIndex));
				rowHtml.AppendLine("</h5>");
				rowHtml.AppendLine("</div>");

				parentControlIndex++;
			}

			if (p.Formulas.Count > 0)
			{
				html.AppendLine("<div class=\"row text-center row-margin-top\">");
				html.Append(rowHtml.ToString());
				html.AppendLine("</div>");
			}

			if (html.Length != 0)
			{
				html.Insert(0, "<br/><div class=\"page-header\"><h4><img src=\"../Content/image/homework.png\" width=\"30\" height=\"30\" /><span style=\"padding: 8px\">算數應用題</span></h4></div><hr />");
			}

			return html.ToString();
		}
	}
}