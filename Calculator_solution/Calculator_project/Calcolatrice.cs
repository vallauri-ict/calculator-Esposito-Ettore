﻿using System;
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
            public ButtonStuct(char ch, bool bold, bool isNum = false, bool isDec = false, bool isPM = false)
            {
                this.ch = ch;
                this.bold = bold;
                this.isNum = isNum;
                this.isDec = isDec;
                this.isPM = isPM;
            }
        }

        /* { '%', 'ɶ', 'C', '←' },
           { '¼', '²', '√', '÷' },
           { '7', '8', '9', 'X' },
           { '4', '5', '6', '-' },
           { '1', '2', '3', '+' },
           { '±', '0', ',', '=' }  */

        ButtonStuct[,] Buttons =
        {
            { new ButtonStuct('%', false), new ButtonStuct('ɶ', false), new ButtonStuct('C', false), new ButtonStuct('←', false) },
            { new ButtonStuct('¼', false), new ButtonStuct('²', false), new ButtonStuct('√', false), new ButtonStuct('÷', false) },
            { new ButtonStuct('7', true, true), new ButtonStuct('8', true, true), new ButtonStuct('9', true, true), new ButtonStuct('X', false) },
            { new ButtonStuct('4', true, true), new ButtonStuct('5', true, true), new ButtonStuct('6', true, true), new ButtonStuct('-', false) },
            { new ButtonStuct('1', true, true), new ButtonStuct('2', true, true), new ButtonStuct('3', true, true), new ButtonStuct('+', false) },
            { new ButtonStuct('±', false, false, false, true), new ButtonStuct('0', true, true), new ButtonStuct(',', false, false, true), new ButtonStuct('=', true) },  
        };

        private RichTextBox Screen;

        private void Calcolatrice_Load(object sender, EventArgs e)
        {
            MakeCalculator(Buttons);
        }

        private void MakeCalculator(ButtonStuct[,] b)
        {
            //bw=button width, bh=button height, ox=offset x, oy=offset y, s=space;
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
                    newB.Tag = b[i, j];
                    newB.Location = new Point(x, y);
                    newB.Size = new Size(bw, bh);
                    newB.Text = b[i, j].ch.ToString();
                    newB.Font = new Font("Segoe UI", 14F, b[i, j].bold ? FontStyle.Bold : FontStyle.Regular);
                    newB.Click += Button_Click;
                    this.Controls.Add(newB);
                }

            //schermo
            Screen = new RichTextBox();
            x = this.Width - (ox * 2 + s * 3);
            y = oy - (ox + s);
            Screen.Name = "Schermo";
            Screen.SelectionAlignment = HorizontalAlignment.Right;
            Screen.Location = new Point(ox, ox);
            Screen.Size = new Size(x, y);
            Screen.Text = "0";
            Screen.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            Screen.ReadOnly = true;
            Screen.TabStop = false;
            this.Controls.Add(Screen);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btm = (Button)sender; //il sender è di sicuro un bottone
            ButtonStuct btmTag = (ButtonStuct)btm.Tag;
            if(btmTag.isNum)
            {
                if (Screen.Text == "0")
                    Screen.Text = "";
                Screen.Text += btm.Text;
            }
            else if(btmTag.isDec)
            {
                if (!Screen.Text.Contains(btmTag.ch))
                    Screen.Text += btm.Text;
            }
            else if (btmTag.isPM)
            {
                if (!Screen.Text.Contains('-'))
                    Screen.Text = "-" + Screen.Text;
                else
                    Screen.Text = Screen.Text.Substring(1);
            }
        }
    }
}
