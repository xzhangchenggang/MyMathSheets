﻿using ComputationalStrategy.Item;
using ComputationalStrategy.Main.Operation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheFormulaShows;

namespace TestConsoleApp
{
	/// <summary>
	/// 
	/// </summary>
	public class Program
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			MakeHtml<List<Formula>, Arithmetic> work = null;
			MakeHtml<List<EqualityFormula>, EqualityComparison> work1 = null;
			MakeHtml<List<ConnectionFormula>, ComputingConnection> work2 = null;
			bool isShowMenu = true;

			while (1 == 1)
			{
				if (isShowMenu)
				{
					Console.WriteLine("參數選擇：");
					Console.WriteLine("************************* 四則運算 *************************");
					Console.WriteLine("    a-隨機四則運算填空");
					Console.WriteLine("    b-標準加法填空");
					Console.WriteLine("    c-標準減法填空");
					Console.WriteLine("    d-標準乘法填空");
					Console.WriteLine("    e-標準除法填空");
					Console.WriteLine("************************* 計算比大小 ***********************");
					Console.WriteLine("    f-隨機四則運算比較");
					Console.WriteLine("    g-標準加法比較");
					Console.WriteLine("    h-標準減法比較");
					Console.WriteLine("************************* 計算接龙 ***********************");
					Console.WriteLine("    i-隨機加减運算接龙");
					Console.WriteLine("    j-標準加法運算接龙");
					Console.WriteLine("    k-標準減法運算接龙");
					Console.WriteLine("*************************");
					Console.Write("    9-菜单    0-退出");
					Console.WriteLine("");
					Console.Write("");

					isShowMenu = false;
				}

				ConsoleKey key = Console.ReadKey().Key;
				switch (key)
				{
					case ConsoleKey.A:
						IList<SignOfOperation> signs = new List<SignOfOperation>
						{
							SignOfOperation.Plus,
							SignOfOperation.Subtraction,
							SignOfOperation.Multiple,
							SignOfOperation.Division
						};

						Console.WriteLine();
						Console.WriteLine("隨機四則運算填空");
						// TestCase0001
						work = new MakeHtml<List<Formula>, Arithmetic>(FourOperationsType.Random, signs, QuestionType.GapFilling, 100, 35);
						work.Structure();
						Util.CreateOperatorObjectFactory<List<Formula>>("FormulaWrite", work.Formulas);
						break;

					case ConsoleKey.B:
						Console.WriteLine();
						Console.WriteLine("標準加法填空");
						// TestCase0002
						work = new MakeHtml<List<Formula>, Arithmetic>(FourOperationsType.Standard, SignOfOperation.Plus, QuestionType.GapFilling, 50, 20);
						work.Structure();
						Util.CreateOperatorObjectFactory<List<Formula>>("FormulaWrite", work.Formulas);
						break;

					case ConsoleKey.C:
						Console.WriteLine();
						Console.WriteLine("標準減法填空");
						// TestCase0003
						work = new MakeHtml<List<Formula>, Arithmetic>(FourOperationsType.Standard, SignOfOperation.Subtraction, QuestionType.GapFilling, 200, 15);
						work.Structure();
						Util.CreateOperatorObjectFactory<List<Formula>>("FormulaWrite", work.Formulas);
						break;

					case ConsoleKey.D:
						Console.WriteLine();
						Console.WriteLine("標準乘法填空");
						// TestCase0004
						work = new MakeHtml<List<Formula>, Arithmetic>(FourOperationsType.Standard, SignOfOperation.Multiple, QuestionType.GapFilling, 81, 20);
						work.Structure();
						Util.CreateOperatorObjectFactory<List<Formula>>("FormulaWrite", work.Formulas);
						break;

					case ConsoleKey.E:
						Console.WriteLine();
						Console.WriteLine("標準除法填空");
						// TestCase0005
						work = new MakeHtml<List<Formula>, Arithmetic>(FourOperationsType.Standard, SignOfOperation.Division, QuestionType.GapFilling, 81, 20);
						work.Structure();
						Util.CreateOperatorObjectFactory<List<Formula>>("FormulaWrite", work.Formulas);
						break;

					case ConsoleKey.F:
						Console.WriteLine();
						Console.WriteLine("隨機四則運算比較");
						// TestCase0006
						work1 = new MakeHtml<List<EqualityFormula>, EqualityComparison>(FourOperationsType.Random, new List<SignOfOperation> { SignOfOperation.Plus, SignOfOperation.Subtraction }, QuestionType.GapFilling, 81, 20);
						work1.Structure();
						Util.CreateOperatorObjectFactory<List<EqualityFormula>>("EqualityFormulaWrite", work1.Formulas);
						break;

					case ConsoleKey.G:
						Console.WriteLine();
						Console.WriteLine("標準加法比較");
						// TestCase0005
						work1 = new MakeHtml<List<EqualityFormula>, EqualityComparison>(FourOperationsType.Standard, SignOfOperation.Plus, QuestionType.GapFilling, 81, 20);
						work1.Structure();
						Util.CreateOperatorObjectFactory<List<EqualityFormula>>("EqualityFormulaWrite", work1.Formulas);
						break;

					case ConsoleKey.H:
						Console.WriteLine();
						Console.WriteLine("標準減法比較");
						// TestCase0005
						work1 = new MakeHtml<List<EqualityFormula>, EqualityComparison>(FourOperationsType.Standard, SignOfOperation.Subtraction, QuestionType.GapFilling, 81, 20);
						work1.Structure();
						Util.CreateOperatorObjectFactory<List<EqualityFormula>>("EqualityFormulaWrite", work1.Formulas);
						break;

					case ConsoleKey.I:
						Console.WriteLine();
						Console.WriteLine("標準加法運算接龙");
						// TestCase0005
						work2 = new MakeHtml<List<ConnectionFormula>, ComputingConnection>(FourOperationsType.Random, new List<SignOfOperation> {  SignOfOperation.Plus, SignOfOperation.Subtraction}, QuestionType.GapFilling, 100, 4);
						work2.Structure();
						Util.CreateOperatorObjectFactory<List<ConnectionFormula>>("ComputingConnectionWrite", work2.Formulas);
						break;

					case ConsoleKey.J:
						Console.WriteLine();
						Console.WriteLine("標準加法運算接龙");
						// TestCase0005
						work2 = new MakeHtml<List<ConnectionFormula>, ComputingConnection>(FourOperationsType.Standard, SignOfOperation.Plus, QuestionType.GapFilling, 100, 4);
						work2.Structure();
						Util.CreateOperatorObjectFactory<List<ConnectionFormula>>("ComputingConnectionWrite", work2.Formulas);
						break;

					case ConsoleKey.K:
						Console.WriteLine();
						Console.WriteLine("標準減法運算接龙");
						// TestCase00055
						work2 = new MakeHtml<List<ConnectionFormula>, ComputingConnection>(FourOperationsType.Standard, SignOfOperation.Subtraction, QuestionType.GapFilling, 100, 4);
						work2.Structure();
						Util.CreateOperatorObjectFactory<List<ConnectionFormula>>("ComputingConnectionWrite", work2.Formulas);
						break;


					case ConsoleKey.D9:
						isShowMenu = true;
						break;

					default:
						Console.WriteLine();
						Console.WriteLine("Close");
						Console.ReadKey();
						Environment.Exit(0);
						break;
				}
			}
		}


	}
}
