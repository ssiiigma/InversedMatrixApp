using System.ComponentModel;

namespace InversedMatrix_ver3
{
    public class RandomMatrixDialog : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MatrixSize { get; private set; } = 3;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double MinValue { get; private set; } = -10;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double MaxValue { get; private set; } = 10;

        // Generate random matrix Window
        public RandomMatrixDialog()
        {
            Text = "Генерація випадкової матриці";
            Size = new Size(350, 300); 
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            Label lblSize = new Label
            {
                Text = "Розмір матриці (n x n):",
                Location = new Point(20, 20),
                AutoSize = true
            };

            NumericUpDown numSize = new NumericUpDown
            {
                Value = 3,
                Location = new Point(200, 18),
                Width = 100
            };

            Label lblSizeRange = new Label
            {
                Text = "мін. 2, макс. 10",
                Location = new Point(200, 40),
                AutoSize = true,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };

            Label lblMin = new Label
            {
                Text = "Мінімальне значення:",
                Location = new Point(20, 70),
                AutoSize = true
            };

            NumericUpDown numMin = new NumericUpDown
            {
                Minimum = -1000000000000,
                Value = 0,
                DecimalPlaces = 2,
                Location = new Point(200, 68),
                Width = 100
            };

            Label lblMinRange = new Label
            {
                Text = "мін. -100, макс. 0",
                Location = new Point(200, 90),
                AutoSize = true,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };

            Label lblMax = new Label
            {
                Text = "Максимальне значення:",
                Location = new Point(20, 120),
                AutoSize = true
            };

            NumericUpDown numMax = new NumericUpDown
            {
                Value = 10,
                DecimalPlaces = 2,
                Location = new Point(200, 118),
                Width = 100
            };

            Label lblMaxRange = new Label
            {
                Text = "мін. 1, макс. 100",
                Location = new Point(200, 140),
                AutoSize = true,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };

            Button btnOk = new Button
            {
                Text = "Згенерувати",
                DialogResult = DialogResult.OK,
                Location = new Point(80, 190),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(72, 126, 176),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.FlatAppearance.BorderSize = 0;

            Button btnCancel = new Button
            {
                Text = "Скасувати",
                DialogResult = DialogResult.Cancel,
                Location = new Point(200, 190),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(150, 150, 150),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnOk.Click += (s, e) =>
            {
                int size = (int)numSize.Value;
                double minValue = (double)numMin.Value;
                double maxValue = (double)numMax.Value;

                // Checking range of size, minValue, maxValue
                string validRes = Validation.GetErrorInputMsg(size, minValue, maxValue);
                if (validRes.Length > 0)
                {
                    MessageBox.Show(validRes, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.None;
                    return;
                }
                MatrixSize = size;
                MinValue = minValue;
                MaxValue = maxValue;
            };

            Controls.Add(lblSize);
            Controls.Add(numSize);
            Controls.Add(lblSizeRange);
            Controls.Add(lblMin);
            Controls.Add(numMin);
            Controls.Add(lblMinRange);
            Controls.Add(lblMax);
            Controls.Add(numMax);
            Controls.Add(lblMaxRange);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
        }
    }
}
