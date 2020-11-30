using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator_project
{
    public partial class Calcolatrice : Form
    {
        public Calcolatrice()
        {
            InitializeComponent();
        }

        public struct ButtonStuct
        {
            public char ch;
            public bool bold;
            public bool isNum;
            public bool isDec;
            public bool isPM;
            public bool isOp;
            public bool isEq;
            public ButtonStuct(char ch, bool bold, bool isNum = false, bool isDec = false, bool isPM = false, bool isOp = false, bool isEq = false)
            {
                this.ch = ch;
                this.bold = bold;
                this.isNum = isNum;
                this.isDec = isDec;
                this.isPM = isPM;
                this.isOp = isOp;
                this.isEq = isEq;
            }
        }

        /* { '%', 'ɶ', 'C', '←' },
           { '⅟', '²', '√', '÷' },
           { '7', '8', '9', 'X' },
           { '4', '5', '6', '-' },
           { '1', '2', '3', '+' },
           { '±', '0', ',', '=' }  */

        ButtonStuct[,] Buttons =
        {
            { new ButtonStuct('%', false), new ButtonStuct('ɶ', false), new ButtonStuct('C', false), new ButtonStuct('←', false) },
            { new ButtonStuct('⅟',false), new ButtonStuct('²', false), new ButtonStuct('√', false), new ButtonStuct('÷', false, false, false, false, true) },
            { new ButtonStuct('7', true, true), new ButtonStuct('8', true, true), new ButtonStuct('9', true, true), new ButtonStuct('X', false, false, false, false, true) },
            { new ButtonStuct('4', true, true), new ButtonStuct('5', true, true), new ButtonStuct('6', true, true), new ButtonStuct('-', false, false, false, false, true) },
            { new ButtonStuct('1', true, true), new ButtonStuct('2', true, true), new ButtonStuct('3', true, true), new ButtonStuct('+', false, false, false, false, true) },
            { new ButtonStuct('±', false, false, false, true), new ButtonStuct('0', true, true), new ButtonStuct(',', false, false, true), new ButtonStuct('=', true, false, false, false, true, true) },  
        };

        private RichTextBox Screen; //schermo
        private RichTextBox History; //schermo dela crnonlogia

        private const char AZ = '\x0000';
        private const string BACKCOLOR = "#cccccc";
        private double o1, o2, res; //o1=operatore 1, o2=operatore 2, res=risultato
        private char op = AZ; //op=operando
        ButtonStuct lbc; //last button clicked

        private void Calcolatrice_Load(object sender, EventArgs e)
        {
            MakeCalculator(Buttons);
        }

        private void MakeCalculator(ButtonStuct[,] b)
        {
            //BW=button width, BH=button height, OX=offset x(margine laterale), OY=offset y(margine superiore), S=space(distanza tra le parti);
            const int BW = 109, BH = 75, OX = 11, OY = 135, S = 5;
            int x, y;

            //bottoni
            for (int i = 0; i < b.GetLength(0); i++)
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    Button newB = new Button();
                    if(i == 5 && j == 3)
                        newB.BackColor = ColorTranslator.FromHtml("#b0b0b0");
                    else if (i <= 1 || j >= 3)
                        newB.BackColor = ColorTranslator.FromHtml("#dddddd");
                    else
                        newB.BackColor = Color.White;
                    newB.FlatStyle = FlatStyle.Flat;
                    newB.FlatAppearance.BorderSize = 0;
                    x = BW * j + S * j + OX;
                    y = BH * i + S * i + OY;
                    newB.Name = "Btm-" + i + "-" + j;
                    newB.Tag = b[i, j];
                    newB.Location = new Point(x, y);
                    newB.Size = new Size(BW, BH);
                    newB.Text = b[i, j].ch.ToString();
                    newB.Font = new Font("Segoe UI", 14F, b[i, j].bold ? FontStyle.Bold : FontStyle.Regular);
                    newB.Click += Button_Click;
                    this.Controls.Add(newB);
                }

            //schermo
            Screen = new RichTextBox();
            x = this.Width - (OX * 2 + S * 3);
            y = OY - (OX + S);
            Screen.Name = "Schermo";
            Screen.BackColor = ColorTranslator.FromHtml(BACKCOLOR);
            Screen.BorderStyle = BorderStyle.None;
            Screen.SelectionAlignment = HorizontalAlignment.Right;
            Screen.Location = new Point(OX, Convert.ToInt32(OX + (y - (y / 1.5f))));
            Screen.Size = new Size(x, Convert.ToInt32(y / 1.5f));
            Screen.Text = "0";
            Screen.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            Screen.ReadOnly = true;
            Screen.TabStop = false;
            Screen.TextChanged += Screen_TextCanged;
            this.Controls.Add(Screen);

            //history
            History = new RichTextBox();
            y = OY - (OX + S) - (int)(y - (y / 1.5f));
            History.Name = "Cronologia";
            History.BackColor = ColorTranslator.FromHtml(BACKCOLOR);
            History.BorderStyle = BorderStyle.None;
            History.SelectionAlignment = HorizontalAlignment.Right;
            History.Location = new Point(OX, OX);
            History.Size = new Size(x, y);
            History.Text = "";
            History.Font = new Font("Segoe UI", 18F, FontStyle.Regular);
            History.ReadOnly = true;
            History.TabStop = false;
            this.Controls.Add(History);

            //modifica form
            this.BackColor = ColorTranslator.FromHtml(BACKCOLOR);
        }

        private void Screen_TextCanged(object sender, EventArgs e)
        {
            int ext = Screen.Text.Contains('-') ? 1 : 0;
            if (Screen.Text.Length <= 15 + ext)
                Screen.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            else if (Screen.Text.Length <= 18 + ext)
                Screen.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            else if(Screen.Text.Length <= 24 + ext)
                Screen.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            else if(Screen.Text.Length <= 31 + ext)
                Screen.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            else
                Screen.Text = Screen.Text.Remove(Screen.Text.Length - 1);
            MettiPunti();
        }

        private void MettiPunti()
        {
            if (!Screen.Text.Contains(','))
            {
                string num = Screen.Text;
                if (num.Contains('-'))
                    num = num.Substring(1);
                string[] app = num.Split('.');
                num = "";
                for (int i = 0; i < app.Length; i++)
                    num += app[i];
                for (int i = num.Length - 1, cont = 1; i > 0; i--, cont++)
                {
                    if (cont % 3 == 0)
                    {
                        num = num.Insert(i, ".");
                        i--;
                        cont = 1;
                    }
                }
                string sign = Screen.Text.Contains('-') ? "-" : "";
                Screen.Text = sign + num;
            }
            else if (Screen.Text.Split(',')[1].Length > 16)
                Screen.Text = Screen.Text.Remove(Screen.Text.Length - 1);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btm = (Button)sender; //il sender è di sicuro un bottone
            ButtonStuct btmTag = (ButtonStuct)btm.Tag;
            if(btmTag.isNum) //numeri
            {
                if (lbc.isEq || lbc.ch == '√' || lbc.ch == '⅟' || lbc.ch == '²')
                {
                    Screen.Text = "0";  
                    History.Text = "";
                    op = AZ;
                    o1 = 0;
                    o2 = 0;
                    res = 0;
                }
                if (Screen.Text == "0" || lbc.isOp)
                {
                    Screen.Text = "";
                }
                Screen.Text += btm.Text;
            }
            else if(btmTag.isDec) //virgola
            {
                if (!Screen.Text.Contains(btmTag.ch))
                    Screen.Text += btm.Text;
            }
            else if (btmTag.isPM) //più o meno
            {
                if (Screen.Text != "0")
                {
                    if (!Screen.Text.Contains('-'))
                        Screen.Text = "-" + Screen.Text;
                    else
                        Screen.Text = Screen.Text.Substring(1);
                }
            }
            else
            {
                double a, b;
                switch (btmTag.ch)
                {
                    case 'C':
                        Screen.Text = "0";
                        History.Text = "";
                        op = AZ;
                        o1 = 0;
                        o2 = 0;
                        res = 0;
                        break;
                    case 'ɶ':
                        Screen.Text = "0";
                        break;
                    case '←':
                        Screen.Text = Screen.Text.Remove(Screen.Text.Length - 1);
                        if ((Screen.Text.Length == 0) || (Screen.Text == "-0") || (Screen.Text == "-"))
                            Screen.Text = "0";
                        break;
                    case '+':
                    case '-':
                    case 'X':
                    case '÷':
                    case '=':
                        if (op == AZ && !btmTag.isEq)//valore di default
                        {
                            o1 = double.Parse(Screen.Text);
                            op = btmTag.ch;
                            Screen.Text = "0";
                            if (lbc.ch != '²' && lbc.ch != '√' && lbc.ch != '⅟')
                                History.Text += o1;
                        }
                        else if(op != AZ)
                        {
                            if (lbc.isOp && !btmTag.isEq)
                                op = btmTag.ch;
                            else
                            {
                                if (!lbc.isEq)
                                    o2 = double.Parse(Screen.Text);
                                switch (op)
                                {
                                    case '+':
                                        res = o1 + o2;
                                        break;
                                    case '-':
                                        res = o1 - o2;
                                        break;
                                    case 'X':
                                        res = o1 * o2;
                                        break;
                                    case '÷':
                                        res = o1 / o2;
                                        break;
                                    default:
                                        break;
                                }
                                o1 = res;
                                if (lbc.ch != '²' && lbc.ch != '√' && lbc.ch != '⅟')
                                    History.Text += op.ToString() + o2;
                                if (!btmTag.isEq)
                                {
                                    op = btmTag.ch;
                                    if(lbc.ch != '²' && lbc.ch != '√' && lbc.ch != '⅟')
                                        History.Text += op.ToString();                          
                                    o2 = 0;
                                }
                                Screen.Text = res.ToString();
                            }
                        }
                        break;
                    case '²':
                        a = double.Parse(Screen.Text);
                        b = Math.Pow(a, 2);
                        Screen.Text = b.ToString();
                        if (lbc.isEq || lbc.ch == '²' || lbc.ch == '√' || lbc.ch == '⅟')
                        {
                            History.Text = a + "^2";
                        }
                        else
                        {
                            if (op != AZ)
                                History.Text += op.ToString();
                            History.Text += a + "^2";
                        }
                        break;
                    case '√':
                        a = double.Parse(Screen.Text);
                        b = Math.Sqrt(a);
                        Screen.Text = b.ToString();
                        if (lbc.isEq || lbc.ch == '²' || lbc.ch == '√' || lbc.ch == '⅟')
                        {
                            History.Text = "√" + a;
                        }
                        else if (!lbc.isEq)
                        {
                            if (op != AZ)
                                History.Text += op.ToString();
                            History.Text += "√" + a;
                        }
                        break;
                    case '⅟':
                        a = double.Parse(Screen.Text);
                        b = 1 / a;
                        Screen.Text = b.ToString();
                        if (lbc.isEq || lbc.ch == '²' || lbc.ch == '√' || lbc.ch == '⅟')
                        {
                            History.Text = "1/" + a;
                        }
                        else if (!lbc.isEq)
                        {
                            if (op != AZ)
                                History.Text += op.ToString();
                            History.Text += "1/" + a;
                        }
                        break;
                    default:
                        MessageBox.Show("Spiacente ma l'operazione \"" + btmTag.ch + "\" non implementata");
                        break;
                }
            }
            lbc = btmTag;
        }
    }
}
