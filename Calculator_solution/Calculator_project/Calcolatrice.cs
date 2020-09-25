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
            public ButtonStuct(char ch, bool bold)
            {
                this.ch = ch;
                this.bold = bold;
            }
        }

        /*{ '%', 'ɶ', 'C', '←' },
           { '¼', '²', '√', '÷' },
           { '7', '8', '9', 'X' },
           { '4', '5', '6', '-' },
           { '1', '2', '3', '+' },
           { '±', '0', ',', '=' }*/

        ButtonStuct[,] Buttons =
        {
            { new ButtonStuct('%', false), new ButtonStuct('ɶ', false), new ButtonStuct('C', false), new ButtonStuct('←', false) },
            { new ButtonStuct('¼', false), new ButtonStuct('²', false), new ButtonStuct('√', false), new ButtonStuct('÷', false) },
            { new ButtonStuct('7', true), new ButtonStuct('8', true), new ButtonStuct('9', true), new ButtonStuct('X', false) },
            { new ButtonStuct('4', true), new ButtonStuct('5', true), new ButtonStuct('6', true), new ButtonStuct('-', false) },
            { new ButtonStuct('1', true), new ButtonStuct('2', true), new ButtonStuct('3', true), new ButtonStuct('+', false) },
            { new ButtonStuct('±', false), new ButtonStuct('0', true), new ButtonStuct(',', false), new ButtonStuct('=', false) },  
        };

        private void Calcolatrice_Load(object sender, EventArgs e)
        {
            MakeCalculator(Buttons);
        }

        private void MakeCalculator(ButtonStuct[,] b)
        {
            const int bw = 109, bh = 75, ox = 11, oy = 135, s = 5;
            int x, y;

            //bottoni
            for (int i = 0; i < b.GetLength(0); i++)
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    Button newB = new Button();
                    x = bw * j + s * j + ox;
                    y = bh * i + s * i + oy;
                    newB.Name = "Btm-" + i + "-" + j;
                    newB.Location = new Point(x, y);
                    newB.Size = new Size(bw, bh);
                    newB.Text = b[i, j].ch.ToString();
                    newB.Font = new Font("Segoe UI", 14F, b[i,j].bold ? FontStyle.Bold : FontStyle.Regular);
                    this.Controls.Add(newB);
                }

            //schermo
            RichTextBox newTB = new RichTextBox();
            x = this.Width - (ox * 2 + s * 3);
            y = oy - (ox + s);
            newTB.Name = "Screen";
            newTB.SelectionAlignment = HorizontalAlignment.Right;
            newTB.Location = new Point(ox, ox);
            newTB.Size = new Size(x, y);
            newTB.Text = "12345";
            newTB.Font = new Font("Segoe UI", 18F);
            this.Controls.Add(newTB);
        }
    }
}
