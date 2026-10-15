
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;
using System.Text;

namespace Lab1_IvanovAV_BPI_24_01
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private double ValidDimension(TextBox textBox, string title)
        {
            try
            {
                string text = textBox.Text.Trim().Replace(',', '.');

                if (text == string.Empty)
                    throw new ArgumentException(title + ": поле не должно быть пустым.");

                string[] parts = text.Split('.');

                if (parts.Length > 2)
                    throw new ArgumentException(title + ": введено некорректное число.");

                if (parts.Length == 2 && parts[1].Length > 4)
                    throw new ArgumentException(title + ": допускается не более 4 знаков после запятой.");

                double value = double.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

                if (double.IsNaN(value) || double.IsInfinity(value) || value <= 0)
                    throw new ArgumentException(title + ": введите конечное положительное число.");

                return value;
            }
            catch (FormatException)
            {
                throw new ArgumentException(title + ": введено некорректное число.");
            }
            catch (OverflowException)
            {
                throw new ArgumentException(title + ": введено слишком большое число.");
            }
        }

        private bool IsValidNumberText(string text)
        {
            int separatorCount = 0;

            foreach (char symbol in text)
            {
                if (char.IsDigit(symbol)) continue;

                if (symbol == ',' || symbol == '.')
                {
                    separatorCount++;
                    if (separatorCount <= 1) continue;
                }

                return false;
            }

            return true;
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox textBox) return;

            string newText = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
            newText = newText.Insert(textBox.SelectionStart, e.Text);

            if (!IsValidNumberText(newText)) e.Handled = true;
        }

        private void NumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is not TextBox textBox)
            {
                e.CancelCommand();
                return;
            }

            if (!e.DataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }

            if (e.DataObject.GetData(DataFormats.Text) is not string pastedText)
            {
                e.CancelCommand();
                return;
            }

            string newText = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
            newText = newText.Insert(textBox.SelectionStart, pastedText);

            if (!IsValidNumberText(newText)) e.CancelCommand();
        }

        private void ShowFigure(IFigure value, TextBlock result)
        {
            double areaValue = value.CalculateArea();
            double perimeterValue = value.CalculatePerimeter();

            result.Text = string.Format(
                "Площадь: {0:0.####}\nПериметр: {1:0.####}",
                areaValue, perimeterValue);
        }

        private void ShowError(string message, TextBlock result)
        {
            result.Text = "Ошибка: " + message;
        }

        private void ShowError(string message, TextBox result)
        {
            result.Text = "Ошибка: " + message;
        }

        private void CalculateEllipse_Click(object sender, RoutedEventArgs e)
        {
            EllipseResult.Text = string.Empty;

            try
            {
                Ellipse ellipseA = new Ellipse(
                    ValidDimension(EllipseMajorTextBox, "Полуось a"),
                    ValidDimension(EllipseMinorTextBox, "Полуось b"));

                ShowFigure(ellipseA, EllipseResult);
            }
            catch (ArgumentException exception)
            {
                ShowError(exception.Message, EllipseResult);
            }
            catch (ArithmeticException exception)
            {
                ShowError(exception.Message, EllipseResult);
            }
        }

        private void CalculateSquare_Click(object sender, RoutedEventArgs e)
        {
            SquareResult.Text = string.Empty;

            try
            {
                Square squareA = new Square(ValidDimension(SquareSideTextBox, "Сторона квадрата"));
                ShowFigure(squareA, SquareResult);
            }
            catch (ArgumentException exception)
            {
                ShowError(exception.Message, SquareResult);
            }
            catch (ArithmeticException exception)
            {
                ShowError(exception.Message, SquareResult);
            }
        }

        private void CalculateTrapezoid_Click(object sender, RoutedEventArgs e)
        {
            TrapezoidResult.Text = string.Empty;

            try
            {
                Trapezoid trapezoidA = new Trapezoid(
                    ValidDimension(TrapezoidBaseATextBox, "Первое основание"),
                    ValidDimension(TrapezoidBaseBTextBox, "Второе основание"),
                    ValidDimension(TrapezoidSideCTextBox, "Первая боковая сторона"),
                    ValidDimension(TrapezoidSideDTextBox, "Вторая боковая сторона"));

                ShowFigure(trapezoidA, TrapezoidResult);
                TrapezoidResult.Text += "\nВысота: " + trapezoidA.Height.ToString("0.####");
            }
            catch (ArgumentException exception)
            {
                ShowError(exception.Message, TrapezoidResult);
            }
            catch (ArithmeticException exception)
            {
                ShowError(exception.Message, TrapezoidResult);
            }
        }

        private void DemonstrateMethods_Click(object sender, RoutedEventArgs e)
        {
            DemonstrationResult.Text = string.Empty;

            StringBuilder text = new StringBuilder();
            text.AppendLine("Результаты:");
            text.AppendLine();

            try
            {
                Ellipse ellipseA = new Ellipse(
                    ValidDimension(EllipseMajorTextBox, "Полуось a"),
                    ValidDimension(EllipseMinorTextBox, "Полуось b"));

                text.AppendLine("Эллипс:");

                text.AppendFormat(
                    "Полуоси: {0:0.####} и {1:0.####}.\n",
                    ellipseA.SemiMajorAxis, ellipseA.SemiMinorAxis);

                text.AppendLine(ellipseA.ConstructionLog);
                text.AppendLine(ellipseA.GetDescription());

                IFigure ellipseInterface = ellipseA;

                text.AppendFormat(
                    "Площадь = {0:0.####}; периметр = {1:0.####}.\n\n",
                    ellipseInterface.CalculateArea(),
                    ellipseInterface.CalculatePerimeter());
            }
            catch (ArgumentException exception)
            {
                text.AppendLine("Эллипс:");
                text.AppendLine("Ошибка: " + exception.Message);
                text.AppendLine();
            }
            catch (ArithmeticException exception)
            {
                text.AppendLine("Эллипс:");
                text.AppendLine("Ошибка: " + exception.Message);
                text.AppendLine();
            }

            try
            {
                Square squareA = new Square(
                    ValidDimension(SquareSideTextBox, "Сторона квадрата"));

                text.AppendLine("Квадрат:");
                text.AppendFormat("Сторона: {0:0.####}.\n", squareA.Side);
                text.AppendLine(squareA.ConstructionLog);
                text.AppendLine(squareA.GetDescription());

                IFigure squareInterface = squareA;

                text.AppendFormat(
                    "Площадь = {0:0.####}; периметр = {1:0.####}.\n",
                    squareInterface.CalculateArea(),
                    squareInterface.CalculatePerimeter());

                text.AppendLine("Перегрузка метода CalculateArea:");
                text.AppendFormat("squareA.CalculateArea() = {0:0.####}.\n", squareA.CalculateArea());

                text.AppendFormat(
                    "squareA.CalculateArea(squareA.Side / 2) = {0:0.####}.\n",
                    squareA.CalculateArea(squareA.Side / 2));

                text.AppendFormat(
                    "После перегрузки сторона исходного квадрата: {0:0.####}.\n\n",
                    squareA.Side);
            }
            catch (ArgumentException exception)
            {
                text.AppendLine("Квадрат:");
                text.AppendLine("Ошибка: " + exception.Message);
                text.AppendLine();
            }
            catch (ArithmeticException exception)
            {
                text.AppendLine("Квадрат:");
                text.AppendLine("Ошибка: " + exception.Message);
                text.AppendLine();
            }

            try
            {
                Trapezoid trapezoidA = new Trapezoid(
                    ValidDimension(TrapezoidBaseATextBox, "Первое основание"),
                    ValidDimension(TrapezoidBaseBTextBox, "Второе основание"),
                    ValidDimension(TrapezoidSideCTextBox, "Первая боковая сторона"),
                    ValidDimension(TrapezoidSideDTextBox, "Вторая боковая сторона"));

                text.AppendLine("Трапеция:");

                text.AppendFormat(
                    "Основания: {0:0.####} и {1:0.####}, стороны: {2:0.####} и {3:0.####}.\n",
                    trapezoidA.BaseA, trapezoidA.BaseB,
                    trapezoidA.SideC, trapezoidA.SideD);

                text.AppendLine(trapezoidA.ConstructionLog);
                text.AppendLine(trapezoidA.GetDescription());

                IFigure trapezoidInterface = trapezoidA;

                text.AppendFormat(
                    "Площадь = {0:0.####}; периметр = {1:0.####}.\n",
                    trapezoidInterface.CalculateArea(),
                    trapezoidInterface.CalculatePerimeter());

                text.AppendFormat(
                    "Высота трапеции: {0:0.####}.\n\n",
                    trapezoidA.Height);
            }
            catch (ArgumentException exception)
            {
                text.AppendLine("Трапеция:");
                text.AppendLine("Ошибка: " + exception.Message);
                text.AppendLine();
            }
            catch (ArithmeticException exception)
            {
                text.AppendLine("Трапеция:");
                text.AppendLine("Ошибка: " + exception.Message);
                text.AppendLine();
            }

            DemonstrationResult.Text = text.ToString();
        }

        private void TrapezoidConditionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void EllipseMajorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void EllipseMinorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void StoronaKvadrata_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void RazmeryTrapecii_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
        private void MaximizeButton_Click(object sender, RoutedEventArgs e) { WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized; }
        private void CloseButton_Click(object sender, RoutedEventArgs e) { Close(); }
    }
}
