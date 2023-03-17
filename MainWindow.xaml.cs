using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Operations 
        {
            None,
            Plus,
            Minus,
            Multi,
            Devide,
            Equal
        }
            public bool IsHistoryOpened=false;
            public bool IsWriteHistory = true;
            public Queue<string> History = new Queue<string>();
        public struct Calculator
        {
            public TextBlock main;
            public TextBlock result;
            public ListBox listBox;
            public CheckBox checkBox;
            public string[] Nums;
            public Operations Operation;
            public string? Answer;
            public int ind;
            string[] znaki = { "+", "-", "*", "x" ,"="};
            public Calculator(TextBlock m, TextBlock r,ListBox l, CheckBox ch)
            {
                listBox = l;
                checkBox = ch;
                main = m;
                result = r;
                Nums = new string[2] { null, null };
                Operation = Operations.None;
                Answer = null;
                ind = 0;
            }
            // Отображение всех данных
            public void ShowData()
            {
                if (Answer!=null)
                {
                    result.Text = Answer;
                    SetDefault();
                }
                string firstNum = Nums[0];
                string? Oper=null;
                switch (Operation)
                {
                    case Operations.None:
                        Oper = null;
                        break;
                    case Operations.Plus:
                        Oper = "+";
                        break;
                    case Operations.Minus:
                        Oper = "-";
                        break;
                    case Operations.Multi:
                        Oper = "x";
                        break;
                    case Operations.Devide:
                        Oper = "÷";
                        break;
                    case Operations.Equal:
                        break;
                    default:
                        break;
                }
                string secondNum = Nums[1];
                main.Text=(firstNum+Oper+secondNum);
            }
            // Возврат в начальное состояние программы
            public void SetDefault()
            {
                Nums = new string[2] { null, null };
                Operation = Operations.None;
                Answer = null;
                ind = 0;
            }
            public void DeleteALL()
            {
                SetDefault();
                result.Text = null;
            }
            // Формирование числа, добавление цифр
            public void CinNUM(string x)
            {
                if (Nums[ind] == "0")
                {
                    Nums[ind] = x;
                    return;
                }
                Nums[ind] += x;
            }
            // Формирование числа, добавление точки (натуральное число)
            public void CinDOT(string x) 
            {
                if (Nums[ind] == null)
                    return;
                foreach (char h in Nums[ind])
                {
                    if (h ==',')
                        return;
                }
                Nums[ind] += ",";
            }
            // Подсчет
            public void Calculate()
            {
                Decimal first = Decimal.Parse(Nums[0]);
                Decimal second = Decimal.Parse(Nums[1]);
                Decimal ans = 0;
                switch (Operation)
                {
                    case Operations.None:
                        break;
                    case Operations.Plus:
                        ans = first + second;
                        break;
                    case Operations.Minus:
                        ans = first - second;
                        break;
                    case Operations.Multi:
                        ans= first*second;
                        break;
                    case Operations.Devide:
                        if (second==0)
                        {
                            Answer = "Ошибка";
                            return;
                        }
                        ans= first/second;
                        break;
                    case Operations.Equal:
                        break;
                    default:
                        break;
                }
                Answer = ans.ToString();
                if (true==checkBox.IsChecked)
                {
                    listBox.Items.Add(first.ToString() + znaki[(int)(Operation)+1] + second.ToString() +"="+Answer);
                }
                //result.Text= Answer;
            }
            // Помещение операции в переменную
            public void SetOperation( Operations op)
            {
                Operation = op;
                ind = 1;
            }
            // Выбор действие в зависимости операции и условия (помещение в переменную операции либо подсчет)
            public void CallOperation(Operations op)
            {


                if (Nums[1] != null)
                { 
                    if (op == Operations.Equal)
                        Calculate();
                }else if (Nums[0]!=null)
                {
                    if (op == Operations.Equal)
                        return;
                     SetOperation(op);
                }
                    

            }
            // Преобразование текста с кнопки в операцию
            public void CinOperation(string x)
            {
                switch (x)
                {
                    case"=":
                        CallOperation(Operations.Equal);
                        break;
                    case "+":
                        CallOperation(Operations.Plus);
                        break;
                    case "-":
                        CallOperation(Operations.Minus);
                        break;
                    case "x":
                        CallOperation(Operations.Multi);
                        break;
                    case "÷":
                        CallOperation(Operations.Devide);
                        break;
                    default:
                        break;
                }
            }
        }
        public Calculator Data;

        public MainWindow()
        {
            InitializeComponent();

            foreach (UIElement E in Father.Children)
            {
                if (E is Button)
                {
                    ((Button)E).Click += OnClick;
                }
            }
            Data= new Calculator(MainTextBlock,ResultTextBlock,listHist,check);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            string x = (string)((Button)e.OriginalSource).Content ;
            if (int.TryParse(x,out int l))
            {
                Data.CinNUM(x);
            }
            else if (x==",")
            {
                Data.CinDOT(x);
            }
            else if (x=="✘")
            {
                Data.DeleteALL();
            }else
            {
                Data.CinOperation(x);
            }
            Data.ShowData();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void HistoryPanelOff(object sender, RoutedEventArgs e)
        {
            if (!IsHistoryOpened)
                return;
            Width = 480;
            Father.ColumnDefinitions.RemoveAt(5);
            Father.ColumnDefinitions.RemoveAt(4);
            HistoryPanel.Visibility = Visibility.Hidden;
            IsHistoryOpened = false;
        }
        private void HistoryPanelOn(object sender, RoutedEventArgs e)
        {
            if (IsHistoryOpened)
                return;
            Width = 620;
            Father.ColumnDefinitions.Add(new ColumnDefinition());
            Father.ColumnDefinitions.Add(new ColumnDefinition());
            HistoryPanel.Visibility = Visibility.Visible;
            IsHistoryOpened = true;

        }
        private void DeleteAll(object sender, RoutedEventArgs e)
        {
            listHist.Items.Clear();

        }
        private void DeleteLast(object sender, RoutedEventArgs e)
        {
            if (listHist.Items.Count < 1)
                return;
            listHist.Items.RemoveAt(listHist.Items.Count - 1);

        }
    }
}
