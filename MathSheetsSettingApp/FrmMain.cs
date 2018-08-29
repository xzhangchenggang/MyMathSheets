﻿using ComputationalStrategy.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheFormulaShows;

namespace MathSheetsSettingApp
{
	/// <summary>
	/// 
	/// </summary>
	public partial class FrmMain : Form
	{
		/// <summary>
		/// 
		/// </summary>
		public FrmMain()
		{
			InitializeComponent();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
		{
			Dictionary<int, string> kvTopic = new Dictionary<int, string>
			{
				{ 1, "标准" },
				{ 2, "填空" }
			};
			BindingSource bsTopic = new BindingSource
			{
				DataSource = kvTopic
			};

			this.cmbTopic.DataSource = bsTopic;
			this.cmbTopic.ValueMember = "Key";
			this.cmbTopic.DisplayMember = "Value";

			Dictionary<int, string> kvComplexity = new Dictionary<int, string>
			{
				{ 20, "简单" },
				{ 50, "中等" },
				{ 100, "困难" }
			};
			BindingSource bsComplexity = new BindingSource
			{
				DataSource = kvComplexity
			};

			this.cmbComplexity.DataSource = bsComplexity;
			this.cmbTopic.ValueMember = "Key";
			this.cmbTopic.DisplayMember = "Value";
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StandardCheckedChanged(object sender, EventArgs e)
		{
			this.chkAdition.Checked = true;
			this.chkSubtraction.Checked = false;
			this.chkMultiplication.Checked = false;
			this.chkDivision.Checked = false;

			this.chkAdition.Enabled = true;
			this.chkSubtraction.Enabled = false;
			this.chkMultiplication.Enabled = false;
			this.chkDivision.Enabled = false;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MixtureCheckedChanged(object sender, EventArgs e)
		{
			this.chkAdition.Checked = true;
			this.chkSubtraction.Checked = true;
			this.chkMultiplication.Checked = false;
			this.chkDivision.Checked = false;

			this.chkAdition.Enabled = false;
			this.chkSubtraction.Enabled = false;
			this.chkMultiplication.Enabled = false;
			this.chkDivision.Enabled = false;
		}
		/// <summary>
		/// 
		/// </summary>
		private SignOfOperation _sign
		{
			get
			{
				return chkAdition.Checked ? SignOfOperation.Plus :
							chkDivision.Checked ? SignOfOperation.Division :
							chkMultiplication.Checked ? SignOfOperation.Multiple : SignOfOperation.Subtraction;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private QuestionType _questionType
		{
			get
			{
				return (QuestionType)(Convert.ToInt32(this.cmbTopic.SelectedValue));
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private int _maximumLimit
		{
			get
			{
				return Convert.ToInt32(((KeyValuePair<int, string>)this.cmbComplexity.SelectedValue).Key);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private int _numberOfQuestions
		{
			get
			{
				return Convert.ToInt32(this.tbxNumberOf.Text);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private FourOperationsType _fourOperationsType
		{
			get
			{
				return radStandard.Checked ? FourOperationsType.Standard : FourOperationsType.Random;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private IList<SignOfOperation> _signs
		{
			get
			{
				var tmp = new List<SignOfOperation>();
				if (chkAdition.Checked)
				{
					tmp.Add(SignOfOperation.Plus);
				}
				if (chkDivision.Checked)
				{
					tmp.Add(SignOfOperation.Division);
				}
				if (chkMultiplication.Checked)
				{
					tmp.Add(SignOfOperation.Multiple);
				}
				if (chkSubtraction.Checked)
				{
					tmp.Add(SignOfOperation.Subtraction);
				}

				return tmp;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SureClick(object sender, EventArgs e)
		{
			if (!chkAdition.Checked && !chkDivision.Checked && !chkMultiplication.Checked && !chkSubtraction.Checked)
			{
				MessageBox.Show(this, "加减乘除未指定！");
				return;
			}

			MakeHtmlBase work = new Arithmetic(_fourOperationsType, _signs, _questionType, _maximumLimit, _numberOfQuestions);
			work.Structure();
			string html = work.MakeHtml();

			string sourceFileName = Path.GetFullPath(System.Configuration.ConfigurationManager.AppSettings.Get("Template"));
			string destFileName = Path.GetFullPath(System.Configuration.ConfigurationManager.AppSettings.Get("HtmlWork") + string.Format("HTMLPage_{0}.html", DateTime.Now.ToString("HHmmssfff")));
			File.Copy(sourceFileName, destFileName);

			int index = 0;
			string[] allTextLines = File.ReadAllLines(destFileName, Encoding.UTF8);
			allTextLines.ToList().ForEach(d =>
			{
				index++;
				if (d.IndexOf("<!--ARITHMETIC-->") >= 0)
				{
					allTextLines[index] = html;
				}
			});
			File.WriteAllLines(destFileName, allTextLines, Encoding.Unicode);

			System.Diagnostics.Process.Start(@"IExplore.exe", Path.GetFullPath(destFileName));

			Environment.Exit(0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="parameter"></param>
		/// <param name="gap"></param>
		/// <returns></returns>
		private string GetValue(GapFilling item, int parameter, GapFilling gap)
		{
			if (item == gap)
			{
				return string.Empty.PadLeft(2);
			}
			return parameter.ToString();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AditionCheckedChanged(object sender, EventArgs e)
		{
			if (this.radStandard.Checked && this.chkAdition.Checked)
			{
				this.chkSubtraction.Checked = false;
				this.chkDivision.Checked = false;
				this.chkMultiplication.Checked = false;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SubtractionCheckedChanged(object sender, EventArgs e)
		{
			if (this.radStandard.Checked && this.chkSubtraction.Checked)
			{
				this.chkAdition.Checked = false;
				this.chkDivision.Checked = false;
				this.chkMultiplication.Checked = false;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MultiplicationCheckedChanged(object sender, EventArgs e)
		{
			if (this.radStandard.Checked && this.chkMultiplication.Checked)
			{
				this.chkAdition.Checked = false;
				this.chkDivision.Checked = false;
				this.chkSubtraction.Checked = false;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DivisionCheckedChanged(object sender, EventArgs e)
		{
			if (this.radStandard.Checked && this.chkDivision.Checked)
			{
				this.chkAdition.Checked = false;
				this.chkMultiplication.Checked = false;
				this.chkSubtraction.Checked = false;
			}
		}
	}
}
