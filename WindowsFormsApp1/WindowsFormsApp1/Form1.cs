using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetInputsVisibility(false, false);
        }

        private void SetInputsVisibility(bool singleMode, bool intervalMode)
        {
           
            textBox1.Visible = singleMode;
    
            labelStartInterval.Visible = intervalMode;
            textBox2.Visible = intervalMode;
            labelEndInterval.Visible = intervalMode;
            textBox3.Visible = intervalMode;
            labelStep.Visible = intervalMode;
            textBox4.Visible = intervalMode;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                SetInputsVisibility(true, false);
                listBox1.Items.Clear();
                listBox2.ClearSelected();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                SetInputsVisibility(false, true);
                listBox1.Items.Clear();
                listBox2.ClearSelected();
            }
        }

        
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                return;

        
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Ошибка: сначала выберите режим расчета!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                listBox2.ClearSelected();
                return;
            }

        
            string selectedUnit = listBox2.SelectedItem.ToString();
            double divisor = GetUnitDivisor(selectedUnit);

            if (divisor <= 0)
            {
                MessageBox.Show("Ошибка: выбрана неизвестная единица измерения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            listBox1.Items.Clear();

            
            if (radioButton1.Checked)
            {
                if (!TryParsePositiveDouble(textBox1.Text, "веса в граммах", out double grams, allowZero: true))
                {
                    listBox2.ClearSelected();
                    return;
                }

                double result = grams / divisor;
                listBox1.Items.Add($"{grams} граммов = {result} {selectedUnit}");
            }
            
            else if (radioButton2.Checked)
            {
                if (!TryParsePositiveDouble(textBox2.Text, "начала интервала", out double start, allowZero: true) ||
                    !TryParsePositiveDouble(textBox3.Text, "конца интервала", out double end, allowZero: true) ||
                    !TryParsePositiveDouble(textBox4.Text, "шага", out double step, allowZero: false))
                {
                    listBox2.ClearSelected();
                    return;
                }

                if (start > end)
                {
                    MessageBox.Show("Ошибка: начало интервала не может быть больше конца интервала!", "Некорректный интервал", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    listBox2.ClearSelected();
                    return;
                }

                if ((end - start) / step > 10000)
                {
                    MessageBox.Show("Ошибка: слишком маленький шаг для заданного интервала (более 10 000 строк).", "Переполнение вывода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    listBox2.ClearSelected();
                    return;
                }

                for (double current = start; current <= end + 1e-9; current += step)
                {
                    double roundedCurrent = Math.Round(current, 6);
                    double result = roundedCurrent / divisor;
                    listBox1.Items.Add($"{roundedCurrent} граммов = {result} {selectedUnit}");
                }
            }
        }

        private double GetUnitDivisor(string unit)
        {
            switch (unit)
            {
                case "Фунт": return 453.6;
                case "Пуд": return 16380.0;
                case "Унция": return 28.35;
                case "Драхм": return 3.3;
                case "Гран": return 15.43;
                default: return -1;
            }
        }

      
        private bool TryParsePositiveDouble(string rawInput, string fieldName, out double value, bool allowZero)
        {
            value = 0;

            if (string.IsNullOrWhiteSpace(rawInput))
            {
                MessageBox.Show($"Ошибка: поле «{fieldName}» не должно быть пустым!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

         
            string normalized = rawInput.Trim().Replace('.', ',');

            if (!double.TryParse(normalized, out value) || double.IsNaN(value) || double.IsInfinity(value))
            {
                MessageBox.Show($"Ошибка: в поле «{fieldName}» введено некорректное число!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (allowZero && value < 0)
            {
                MessageBox.Show($"Ошибка: значение {fieldName} не может быть отрицательным!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!allowZero && value <= 0)
            {
                MessageBox.Show($"Ошибка: значение {fieldName} должно быть строго больше нуля!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void UpdateListBoxStyle()
        {
            FontStyle style = checkBox1.Checked ? FontStyle.Italic : FontStyle.Regular;
            listBox1.Font = new Font(FontFamily.GenericSansSerif, 8, style);
            listBox1.ForeColor = checkBox2.Checked ? Color.Red : Color.Black;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateListBoxStyle();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateListBoxStyle();
        }
    }
}
