namespace Lab1_IvanovAV_BPI_24_01
{
    public class Square : Figure
    {
        private readonly double sideValue;

        public double Side
        {
            get
            {
                return sideValue;
            }
        }

        public Square(double side)
            : base("Квадрат")
        {
            CheckSize(side, "Сторона квадрата");

            sideValue = side;

            constructionLogValue +=
                " → Конструктор Square";
        }

        public override double CalculateArea()
        {
            return CalculateArea(Side);
        }

        public double CalculateArea(double otherSide)
        {
            CheckSize(
                otherSide,
                "Другая сторона квадрата");

            return CheckResult(
                otherSide * otherSide);
        }

        public override double CalculatePerimeter()
        {
            return CheckResult(
                4 * sideValue);
        }

    }
}