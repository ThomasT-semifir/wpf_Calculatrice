using Calculatrice.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculatrice
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		int currentState = 1;
		string mathOperator;
		double firstNumber, secondNumber;
		List<string> historyList = new List<string> { "" };
		string historyString;

		public MainWindow()
		{
			InitializeComponent();
			OnClear(this, null);
		}

		void OnSelectNumber(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			string pressed = button.Content.ToString();

			if (this.resultText.Text == "0" || currentState < 0)
			{
				this.resultText.Text = "";
				if (currentState < 0)
					currentState *= -1;
			}

			this.resultText.Text += pressed;

			double number;
			if (double.TryParse(this.resultText.Text, out number))
			{
				this.resultText.Text = number.ToString("N0");
				if (currentState == 1)
				{
					firstNumber = number;
				}
				else
				{
					secondNumber = number;
				}
			}
		}

		void OnSelectOperator(object sender, EventArgs e)
		{
			currentState = -2;
			Button button = (Button)sender;
			string pressed = button.Content.ToString();
			mathOperator = pressed;
		}

		void OnClear(object sender, EventArgs e)
		{
			firstNumber = 0;
			secondNumber = 0;
			currentState = 1;
			this.resultText.Text = "0";
		}

		void OnCalculate(object sender, EventArgs e)
		{
			if (currentState == 2)
			{
				var result = SimpleCalculator.Calculate(firstNumber, secondNumber, mathOperator);
				this.resultText.Text = result.ToString();
				HandleHistory(firstNumber, secondNumber, mathOperator, result);
				firstNumber = result;
				currentState = -1;
			}
		}

		private void OnClearEverything(object sender, EventArgs e)
		{
			OnClear(this, null);
			ClearHistory();
		}

		private void ClearHistory()
		{
			this.historyList = new List<string>();
			this.historyString = null;
			SetHistory(this.historyString);
		}

		private void SetHistory(string newHistory)
		{
			this.history1.Text = newHistory;
		}

		private void HandleHistory(double firstNumber, double secondNumber, string mathOperator, double result)
		{
			this.historyList.Add($"{firstNumber} {mathOperator} {secondNumber} = {result}");
			this.historyString = "";
			for (int i = this.historyList.Count - 3; i < this.historyList.Count; i++)
			{
				this.historyString += i >= 0 ? historyList[i] + "\n" : "";
			};
			SetHistory(this.historyString);
		}
	}
}
