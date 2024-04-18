using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Laba4._2
{
    public partial class Form1 : Form
    {
        Model model;
        public Form1()
        {
            InitializeComponent();

            model = new Model();
            model.form1 = this;
            model.ModelHandler += new System.EventHandler(this.UpdateValuesFromModel);
            model.LoadSettings();
            this.FormClosing += Form1Closing;
        }
        public void UpdateValuesFromModel(object sender, EventArgs e)
        {
            textBox1.Text = model.getA().ToString();
            textBox2.Text = model.getB().ToString();
            textBox3.Text = model.getC().ToString();

            numericUpDown1.Value = model.getA();
            numericUpDown2.Value = model.getB();
            numericUpDown3.Value = model.getC();

            trackBar1.Value = model.getA();
            trackBar2.Value = model.getB();
            trackBar3.Value = model.getC();
        }
        public void DisableValueChanged()
        {
            numericUpDown1.ValueChanged -= numericUpDown1_ValueChanged;
            numericUpDown2.ValueChanged -= numericUpDown2_ValueChanged;
            numericUpDown3.ValueChanged -= numericUpDown3_ValueChanged;
        }
        public void EnableValueChanged()
        {
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            numericUpDown3.ValueChanged += numericUpDown3_ValueChanged;
        }
        private void Form1Closing(object sender, FormClosingEventArgs e) // Сохранение данных в настройках приложения
        {
            Properties.Settings.Default.A = model.getA();
            Properties.Settings.Default.B = model.getB();
            Properties.Settings.Default.C = model.getC();
            Properties.Settings.Default.Save();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(textBox1.Text, out int result)) { model.changeA(result); }
                else { model.ModelHandler.Invoke(this, null); }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(textBox2.Text, out int result)) { model.changeB(result); }
                else { model.ModelHandler.Invoke(this, null); }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(textBox3.Text, out int result)) { model.changeC(result); }
                else { model.ModelHandler.Invoke(this, null); }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            model.changeA(Decimal.ToInt32(numericUpDown1.Value));
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            model.changeB(Decimal.ToInt32(numericUpDown2.Value));
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            model.changeC(Decimal.ToInt32(numericUpDown3.Value));
        }
        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            model.changeA(trackBar1.Value);
        }

        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            model.changeB(trackBar2.Value);
        }

        private void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            model.changeC(trackBar3.Value);
        }
    }
    class Model
    {
        private int A = 0;
        private int B = 50;
        private int C = 100;
        public System.EventHandler ModelHandler;
        public Form1 form1;

        public void LoadSettings() // Загрузка данных из настроек
        {
            A = Properties.Settings.Default.A;
            B = Properties.Settings.Default.B;
            C = Properties.Settings.Default.C;
            form1.DisableValueChanged();
            ModelHandler.Invoke(this, null);
            form1.EnableValueChanged();
        }
        public void changeA(int newA) // Разрешающее поведение
        {
            if (newA >= 0 && newA <= 100)
            {
                if (newA <= B) { A = newA; }
                else
                {
                    A = newA;
                    B = newA;
                    if (C < newA) {  C = newA; }
                }
            }
            else if (newA < 0) { A = 0; }
            else { A = 100; }
            ModelHandler.Invoke(this, null);
        }
        public void changeB(int newB) 
        {
            if (newB >= 0 && newB <= 100)
            {
                if (newB >= A && newB <= C)
                {
                    B = newB;
                }
                else if (newB < A) // Ограничивающее поведение
                {
                    B = A;
                }
                else // newB > C
                {
                    B = C;
                }
            }
            ModelHandler.Invoke(this, null);
        }
        public void changeC(int newC)
        {
            if (newC >= 0 && newC <= 100)
            {
                if (newC >= B) { C = newC; }
                else 
                {
                    C = newC;
                    B = newC;
                    if (A > newC) { A = newC; }
                }
            }
            else if (newC < 0) { C = 0; }
            else { C = 100; }
            ModelHandler.Invoke(this, null);
        }
        public int getA() { return A; }
        public int getB() { return B; }
        public int getC() { return C; }
    }
}
