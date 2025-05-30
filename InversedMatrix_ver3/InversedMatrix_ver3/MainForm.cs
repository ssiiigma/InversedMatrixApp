namespace InversedMatrix_ver3
{
    public class MainForm : Form
    {
        // Colour palette of Window
        private readonly Color _mainBackGColor = Color.FromArgb(240, 245, 249);
        private readonly Color _btnColor = Color.FromArgb(72, 126, 176);
        private readonly Color _btnHoverColor = Color.FromArgb(50, 100, 150);
        private readonly Color _darkBlueBtnColor = Color.FromArgb(40, 80, 120);
        private readonly Color _textBoxBackColor = Color.FromArgb(252, 252, 252);
        private readonly Color _lblColor = Color.FromArgb(40, 60, 80);

        // Elements of interface
        private Label _lblMenu;
        private readonly Button _btnImportMatrix = new();

        // Area for Matrix
        private Label _lblOriginal;
        private DataGridView _gridOriginalMatrix;
        private Label _lblInverse;
        private DataGridView _gridInverseMatrix;

        private ComboBox _cmbMethod;
        private Button _btnCalculate;
        private Button _btnSaveToFile;
        private Button _btnCompareMethods;
        private Button _btnResetMatrix;
        private Button _btnManualInput;

        private double[,] _currentMatrix;
        private double[,] _inverseMatrix;
        private double[,] _tmpMatrix;
        private double _lupTime;
        private double _borderTime;
        private int _spacing;

        public MainForm()
        {
            InitializeComponents();

            //Appearance of the Window
            Text = @"Matrix Inverse Calculator";
            Size = new Size(900, 460);
            BackColor = _mainBackGColor;
            Font = new Font("Segoe UI", 9);
        }

        private void InitializeComponents()
        {
            Text = @"Matrix Calculator";
            Size = new Size(900, 650);
            MinimumSize = new Size(700, 500);
            BackColor = _mainBackGColor;
            Font = new Font("Segoe UI", 9);

            // Menu label
            _lblMenu = new Label
            {
                Text = @"Меню",
                Location = new Point(20, 20),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                AutoSize = true,
                ForeColor = _lblColor
            };
            Controls.Add(_lblMenu);

            // Menu buttons
            int buttonWidth = 190;
            int buttonHeight = 43;
            int startY = 50;
            AddMenuButton("Що таке обернена матриця", 20, ref startY, buttonWidth, buttonHeight);
            AddMenuButton("Імпортувати матрицю", 20, ref startY, buttonWidth, buttonHeight);
            AddMenuButton("Випадкова матриця", 20, ref startY, buttonWidth, buttonHeight);
            AddMenuButton("Інструкція", 20, ref startY, buttonWidth, buttonHeight);
            AddMenuButton("Методи", 20, ref startY, buttonWidth, buttonHeight);
            AddMenuButton("Складність", 20, ref startY, buttonWidth, buttonHeight);
            AddMenuButton("Вихід", 20, ref startY, buttonWidth, buttonHeight);

            // Matrix labels
            _lblOriginal = new Label
            {
                Text = @"Оригінальна матриця:",
                Location = new Point(250, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = _lblColor,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            Controls.Add(_lblOriginal);

            _gridOriginalMatrix = new DataGridView
            {
                Location = new Point(250, 45),
                Size = new Size(285, 240), 
                BackgroundColor = _textBoxBackColor,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9),
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                ColumnHeadersVisible = false,
                ScrollBars = ScrollBars.Both,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            Controls.Add(_gridOriginalMatrix);

            _lblInverse = new Label
            {
                Text = @"Обернена матриця:",
                Location = new Point(555, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = _lblColor,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            Controls.Add(_lblInverse);

            _gridInverseMatrix = new DataGridView
            {
                Location = new Point(555, 45),
                Size = new Size(285, 240), 
                BackgroundColor = _textBoxBackColor,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9),
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                ColumnHeadersVisible = false,
                ScrollBars = ScrollBars.Both,
                ReadOnly = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            Controls.Add(_gridInverseMatrix);

            int bottomY = 305;

            _cmbMethod = new ComboBox
            {
                Location = new Point(250, bottomY),
                Size = new Size(150, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9),
                FlatStyle = FlatStyle.Flat,
                BackColor = _textBoxBackColor,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            _cmbMethod.Items.AddRange("Метод окаймлення", "LUP-розклад");
            _cmbMethod.SelectedIndex = 0;
            Controls.Add(_cmbMethod);

            _btnManualInput = new Button
            {
                Text = @"Ручний ввід",
                Location = new Point(410, bottomY),
                Size = new Size(100, 30),
                BackColor = _btnColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            _btnManualInput.FlatAppearance.BorderSize = 0;
            _btnManualInput.Click += BtnManualInput_Click;
            Controls.Add(_btnManualInput);

            _btnCalculate = new Button
            {
                Text = @"ОБЧИСЛИТИ",
                Location = new Point(520, bottomY),
                Size = new Size(100, 30),
                BackColor = _btnColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            _btnCalculate.FlatAppearance.BorderSize = 0;
            _btnCalculate.Click += BtnCalculate_Click;
            Controls.Add(_btnCalculate);

            _btnResetMatrix = new Button
            {
                Text = @"СКИНУТИ",
                Location = new Point(630, bottomY),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(160, 100, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            _btnResetMatrix.FlatAppearance.BorderSize = 0;
            _btnResetMatrix.Click += BtnResetMatrix_Click;
            Controls.Add(_btnResetMatrix);

            _btnSaveToFile = new Button
            {
                Text = @"Зберегти у файл",
                Location = new Point(555, bottomY + 50),
                Size = new Size(150, 35),
                BackColor = _darkBlueBtnColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            _btnSaveToFile.FlatAppearance.BorderSize = 0;
            _btnSaveToFile.Click += BtnSaveToFile_Click;
            Controls.Add(_btnSaveToFile);

            _btnCompareMethods = new Button
            {
                Text = @"Порівняти методи",
                Location = new Point(715, bottomY + 50),
                Size = new Size(150, 35),
                BackColor = _darkBlueBtnColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            _btnCompareMethods.FlatAppearance.BorderSize = 0;
            _btnCompareMethods.Click += BtnCompareMethods_Click;
            Controls.Add(_btnCompareMethods);
            
            Label lblMinRange = new Label
            {
                Text = "допустимі значення:\n         [-100; 100]",
                Location = new Point(403, 335),
                AutoSize = true,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };
            Controls.Add(lblMinRange);
            
            Resize += (s, e) => ResizeMatrices();

            // Change size of matrix area when changing Window size
            ResizeMatrices();
        }

        // Resizing labels and buttons to save size
        private void ResizeMatrices()
        {
            int leftMargin = 250;
            int topMargin = 45;
            int totalAvailableWidth = ClientSize.Width - leftMargin - 20; 
            int gapBetweenMatrices = 20;

            // Divide the width equally taking into account the margin
            int matrixWidth = (totalAvailableWidth - gapBetweenMatrices) / 2;

            int matrixHeight = 240;

            // Set positions for orig matrix
            _gridOriginalMatrix.Location = new Point(leftMargin, topMargin);
            _gridOriginalMatrix.Size = new Size(matrixWidth, matrixHeight);

            _lblOriginal.Location = new Point(leftMargin, topMargin - 25);

            // Set positions for inversed matrix
            _gridInverseMatrix.Location = new Point(leftMargin + matrixWidth + gapBetweenMatrices, topMargin);
            _gridInverseMatrix.Size = new Size(matrixWidth, matrixHeight);

            _lblInverse.Location = new Point(leftMargin + matrixWidth + gapBetweenMatrices, topMargin - 25);
        }

        // MENU BUTTONS Settings
        private void AddMenuButton(string text, int x, ref int y, int width, int height)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = _btnColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btn.FlatAppearance.BorderSize = 2;
            btn.MouseEnter += Btn_MouseEnter;
            btn.MouseLeave += Btn_MouseLeave;

            if (text.Contains("обернена")) btn.Click += BtnInverseInfo_Click;
            else if (text.Contains("Імпортувати")) btn.Click += BtnImportMatrix_Click;
            else if (text.Contains("Випадкова")) btn.Click += BtnRandomMatrix_Click;
            else if (text.Contains("Інструкція")) btn.Click += BtnUserGuide_Click;
            else if (text.Contains("Методи")) btn.Click += BtnMethodsInfo_Click;
            else if (text.Contains("Складність")) btn.Click += BtnTimeComplexity_Click;
            else if (text.Contains("Вихід")) btn.Click += BtnExit_Click;

            Controls.Add(btn);
            y += height + _spacing;
        }

        //Self-entered matrix
        private void BtnManualInput_Click(object? sender, EventArgs e)
        {
            using (var inputDialog = new Form())
            {
                inputDialog.Text = @"Введіть розмірність матриці";
                inputDialog.Size = new Size(300, 150);
                inputDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputDialog.StartPosition = FormStartPosition.CenterParent;
                inputDialog.MaximizeBox = false;
                inputDialog.MinimizeBox = false;

                var lbl = new Label
                {
                    Text = @"Розмірність (n x n):",
                    Location = new Point(20, 20),
                    AutoSize = true
                };

                // Changing size of matrix in window by NumericUpDown
                var numSize = new NumericUpDown
                {
                    Value = 3,
                    Location = new Point(150, 18),
                    Width = 100
                };

                var btnOk = new Button
                {
                    Text = @"OK",
                    DialogResult = DialogResult.OK,
                    Location = new Point(80, 60),
                    Size = new Size(80, 30),
                    BackColor = Color.FromArgb(72, 126, 176),
                    ForeColor = Color.White
                };

                var btnCancel = new Button
                {
                    Text = @"Скасувати",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(170, 60),
                    Size = new Size(80, 30),
                    BackColor = Color.FromArgb(150, 150, 150),
                    ForeColor = Color.White
                };
                
                // Available size-range for matrix
                Label lblSizeRange = new Label
                {
                    Text = @"Діапазон: [2; 10]",
                    Location = new Point(154, 40),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.Gray
                };

                inputDialog.Controls.Add(lbl);
                inputDialog.Controls.Add(numSize);
                inputDialog.Controls.Add(btnOk);
                inputDialog.Controls.Add(btnCancel);
                inputDialog.Controls.Add(lblSizeRange);
                inputDialog.AcceptButton = btnOk;

                inputDialog.CancelButton = btnCancel;
                
                if (inputDialog.ShowDialog(this) == DialogResult.OK)
                {
                    int size = (int)numSize.Value;
                    if (size < 2 || size > 10)
                    {
                        MessageBox.Show(@"Розмір матриці має бути в межах від 2 до 10.", 
                            @"Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DialogResult = DialogResult.None;
                        return;
                    }
                    InitializeMatrixGrid(_gridOriginalMatrix, size);

                    // Clear previous results
                    _gridInverseMatrix.Rows.Clear();
                    _gridInverseMatrix.Columns.Clear();
                    _inverseMatrix = null;
                }
            }
        }
        
        private void InitializeMatrixGrid(DataGridView grid, int size)
        {
            grid.Columns.Clear();
            grid.Rows.Clear();
            grid.AllowUserToAddRows = false;
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersVisible = false;
            grid.ScrollBars = ScrollBars.Both;
            grid.EditMode = DataGridViewEditMode.EditOnKeystroke;

            // Add columns
            for (int i = 0; i < size; i++)
            {
                grid.Columns.Add($"col{i}", "");
                grid.Columns[i].Width = 50;
            }

            // Add rows
            grid.Rows.Add(size);

            // Cell regulation
            foreach (DataGridViewRow row in grid.Rows)
            {
                row.Height = 25;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                    cell.Style.Font = new Font("Segoe UI", 9);
                }
            }
        }

        // Hover-effect on btns
        private void Btn_MouseEnter(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                ((Button)sender).BackColor = _btnHoverColor;
                ((Button)sender).Cursor = Cursors.Hand;
            }
        }

        private void BtnResetMatrix_Click(object? sender, EventArgs e)
        {
            _gridOriginalMatrix.Rows.Clear();
            _gridOriginalMatrix.Columns.Clear();
            _gridInverseMatrix.Rows.Clear();
            _gridInverseMatrix.Columns.Clear();
            _currentMatrix = null;
            _inverseMatrix = null;
            _tmpMatrix = null;
            _borderTime = 0.0;
            _lupTime = 0.0;
        }

        private void Btn_MouseLeave(object? sender, EventArgs e)
        {
            if (sender != null)
                ((Button)sender).BackColor = _btnColor;
        }

        private void BtnCalculate_Click(object? sender, EventArgs e)
        {
            try
            {
                // Check matrix availability
                if (_gridOriginalMatrix.Rows.Count == 0 || _gridOriginalMatrix.Columns.Count == 0)
                {
                    MessageBox.Show(@"Для початку обчислень потрібно ввести або згенерувати матрицю",
                        @"Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Solving the inversed matrix
                if (_cmbMethod.SelectedItem != null)
                {
                    string? method = _cmbMethod.SelectedItem.ToString();
                    if (method == "Метод окаймлення")
                    {
                        // Solve matrix by BORDERING from Grid
                        _currentMatrix = GetMatrixFromGrid(_gridOriginalMatrix, false);
                        if (!Validation.IsInputValid(_currentMatrix, true))
                            return;
                        
                        // Measuring time only for Bordering method if matrix is new
                        if (_tmpMatrix == null || !(_currentMatrix.Cast<double>().SequenceEqual(_tmpMatrix.Cast<double>())))
                        {
                            _tmpMatrix = _currentMatrix;
                            _lupTime = 0.0;
                        }
                        
                        // Measuring time for Bordering method
                        var watch1 = System.Diagnostics.Stopwatch.StartNew();
                        _inverseMatrix = InversionMethods.InverseByBordering(_currentMatrix);
                        watch1.Stop();
                        _borderTime = watch1.Elapsed.TotalMilliseconds;
             
                    }
                    else
                    {
                        // Solve matrix by LUP from Grid
                        _currentMatrix = GetMatrixFromGrid(_gridOriginalMatrix, true);
                        if (!Validation.IsInputValid(_currentMatrix, true))
                            return;
                        
                        // Measuring time only for LUP if matrix is new
                        if (_tmpMatrix == null || !(_currentMatrix.Cast<double>().SequenceEqual(_tmpMatrix.Cast<double>())))
                        {
                            _tmpMatrix = _currentMatrix;
                            _borderTime = 0.0;
                        }
                        
                        // Measuring time for LUP
                        var watch2 = System.Diagnostics.Stopwatch.StartNew();
                        _inverseMatrix = InversionMethods.InverseByLup(_currentMatrix);
                        watch2.Stop();
                        _lupTime = watch2.Elapsed.TotalMilliseconds;

                    }
                }

                // Output the result
                Matrix.DisplayMatrix(_gridInverseMatrix, _inverseMatrix);
                MessageBox.Show(@"Обчислення завершено успішно!", @"Результат",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Помилка обчислення: {ex.Message}", @"Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double[,] GetMatrixFromGrid(DataGridView grid, bool isLUP)
        {
            return Validation.CheckCellContent(grid, isLUP);
        }

        private void BtnCompareMethods_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_currentMatrix == null)
                {
                    MessageBox.Show(@"Для порівняння потрібна матриця", @"Помилка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string result =
                    $"Порівняння методів для матриці {_currentMatrix.GetLength(0)}x{_currentMatrix.GetLength(0)}:\n\n" +
                    $"Метод окаймлення: {_borderTime:F3} мс\n" +
                    $"LUP-розклад: {_lupTime:F3} мс\n\n" +
                    $"Різниця: {Math.Abs(_borderTime - _lupTime):F3} мс";
                
                MessageBox.Show(result, @"Результати порівняння",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Помилка при порівнянні: {ex.Message}", @"Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRandomMatrix_Click(object? sender, EventArgs e)
        {
            RandomMatrixDialog dialog = new RandomMatrixDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int size = dialog.MatrixSize;
                    double minValue = dialog.MinValue;
                    double maxValue = dialog.MaxValue;
                    
                    // Generate and Display
                    _currentMatrix = Matrix.GenerateRandomMatrix(size, minValue, maxValue);
                    Matrix.DisplayMatrix(_gridOriginalMatrix, _currentMatrix);

                    // Clear previous
                    _gridInverseMatrix.Rows.Clear();
                    _gridInverseMatrix.Columns.Clear();
                    _inverseMatrix = null;

                    MessageBox.Show($@"Згенеровано випадкову матрицю {size}x{size}", @"Успіх",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($@"Помилка генерації: {ex.Message}", @"Помилка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSaveToFile_Click(object? sender, EventArgs e)
        {
            if (_inverseMatrix == null)
            {
                MessageBox.Show(@"Немає даних для збереження", @"Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                // Only txt files!
                Filter = @"Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = @"Зберегти матрицю",
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _btnSaveToFile.Enabled = false;
                FileManager.SaveMatrixToFile(saveFileDialog.FileName, _inverseMatrix, () =>
                    {
                        _btnSaveToFile.Enabled = true;
                        MessageBox.Show(@"Матрицю збережено успішно!", @"Успіх",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    },
                    error =>
                    {
                        _btnSaveToFile.Enabled = true;
                        MessageBox.Show(error, @"Помилка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                );
            }
        }

        private void BtnImportMatrix_Click(object? sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = @"Text files (*.txt)|*.txt",
                Title = @"Імпортувати матрицю",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _btnImportMatrix.Enabled = false;
                FileManager.LoadMatrixFromFile(openFileDialog.FileName, matrix =>
                    {
                        // Checking content of the file
                        _currentMatrix = matrix;
                        if (!Validation.IsInputValid(_currentMatrix, true))
                            return;

                        Matrix.DisplayMatrix(_gridOriginalMatrix, _currentMatrix);
                        _gridInverseMatrix.Rows.Clear();
                        _gridInverseMatrix.Columns.Clear();
                        _inverseMatrix = null;
                        _btnImportMatrix.Enabled = true;
                        MessageBox.Show(@"Матрицю успішно імпортовано!", @"Успіх",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    },
                    error =>
                    {
                        _btnImportMatrix.Enabled = true;
                        MessageBox.Show(error, @"Помилка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                );
            }
        }

        private void BtnInverseInfo_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "📌 Обернена матриця A⁻¹ - матриця, добуток якої на початкову матрицю A рівний одиничній матриці:\n\n" +
                "\tA × A⁻¹ = A⁻¹ × A = I\n" +
                "де I - одинична матриця.\n\n" +
                "🔹 Властивості:\n\n" +
                "\t1. (A⁻¹)⁻¹ = A\n\n" +
                "\t2. (AB)⁻¹ = B⁻¹A⁻¹\n\n" +
                "\t3. (Aᵀ)⁻¹ = (A⁻¹)ᵀ\n\n" +
                @"❗ Обернена матриця існує тільки для квадратних невироджених матриць.", @"Довідник",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnUserGuide_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("📖 Як користуватись програмою?\n\n" +
                            "1. Введіть матрицю у поле 'Оригінальна матриця' (або згенеруйте випадкову)\n\n" +
                            "2. Виберіть метод обчислення\n\n" +
                            "3. Натисніть 'ОБЧИСЛИТИ'\n\n" +
                            "4. Результат з'явиться у полі 'Обернена матриця'\n\n" +
                            "5. Можете зберегти результат у файл або порівняти методи\n\n", @"Інструкція користувача",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnMethodsInfo_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("🔧 Доступні методи обчислення:\n\n" +
                            "1. Метод ОКАЙМЛЕННЯ - це чисельний метод для знаходження оберненої матриці квадратної матриці A.\n\n" +
                            "Суть методу полягає в тому, що обернена матриця (A⁻¹) будується поступово, починаючи з матриці 1×1, шляхом послідовного" +
                            "додавання рядків і стовпців (окаймлення).\n\n\n" +
                            "2. LUP-РОЗКЛАД - це спосіб розкласти квадратну матрицю A на добуток трьох матриць:\n\n" +
                            "\t\tPA=LU\n\n" +
                            "де P — матриця перестановок (запам’ятовує перестановки рядків для стабільності);\n" +
                            "L — нижня трикутна матриця з 1 на головній діагоналі\n" +
                            "U — верхня трикутна матриця\n\n", @"Довідка про методи",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnTimeComplexity_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("⏱ Часова складність методів:\n\n" +
                            "\t~ ОКАЙМЛЕННЯ ~\n\n" +
                            "На кожному кроці потрібно виконати операції:\n" +
                            "-->Обрахунок оновленого A⁻¹ за формулою окаймлення\n" +
                            "-->Основні операції — множення, віднімання, додавання матриць та векторів\n\n" +
                            "Загальна асимптотична складність: O(n³)\n\n\n" +
                            "\t~ LUP-РОЗКЛАД ~\n\n" +
                            "Складність кроків:\n" +
                            "-->LUP-розклад - O(n³);\n" +
                            "-->Розв'язання n систем Ly = b та Ux = y - O(n³).\n\n" +
                            "Підсумкова складність: O(n³)\n\n", @"Довідка про часову складність",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show(@"Ви впевнені, що хочете вийти?", @"Підтвердження",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _borderTime = 0.0;
                _lupTime = 0.0;
                Close();
            }
        }
    }
}