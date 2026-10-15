using System;

namespace Lab1_IvanovAV_BPI_24_01
{
    public class Trapezoid : Figure
    {
        private readonly double firstBaseValue;
        private readonly double secondBaseValue;
        private readonly double firstSideValue;
        private readonly double secondSideValue;
        private readonly double heightValue;

        private const double Epsilon = 0.0001;

        public double BaseA
        {
            get
            {
                return firstBaseValue;
            }
        }

        public double BaseB
        {
            get
            {
                return secondBaseValue;
            }
        }

        public double SideC
        {
            get
            {
                return firstSideValue;
            }
        }

        public double SideD
        {
            get
            {
                return secondSideValue;
            }
        }

        public double Height
        {
            get
            {
                return heightValue;
            }
        }

        public Trapezoid(
            double baseA,
            double baseB,
            double sideC,
            double sideD)
            : base("Трапеция")
        {
            CheckSize(baseA, "Первое основание");
            CheckSize(baseB, "Второе основание");
            CheckSize(sideC, "Первая боковая сторона");
            CheckSize(sideD, "Вторая боковая сторона");

            double difference =
                Math.Abs(baseA - baseB);

            if (difference < Epsilon)
            {
                throw new ArgumentException(
                    "При равных основаниях четырёх длин недостаточно для определения площади. Введите разные основания.");
            }

            double sideDifference =
                Math.Abs(sideC - sideD);

            double sideSum =
                sideC + sideD;

            if (difference <= sideDifference + Epsilon ||
                difference >= sideSum - Epsilon)
            {
                throw new ArgumentException(
                    "Трапеция с такими сторонами не существует или имеет нулевую высоту.");
            }

            double scale =
                Math.Max(
                    difference,
                    Math.Max(sideC, sideD));

            double d = difference / scale;
            double c = sideC / scale;
            double e = sideD / scale;

            double projection =
                (d * d + (c - e) * (c + e)) /
                (2 * d);

            double heightSquared =
                (c - projection) *
                (c + projection);

            if (heightSquared <= 0 ||
                double.IsNaN(heightSquared))
            {
                throw new ArithmeticException(
                    "Не удалось вычислить высоту.");
            }

            // Высота трапеции.
            double height =
                scale *
                Math.Sqrt(heightSquared);

            if (height < Epsilon)
            {
                throw new ArgumentException(
                    "Трапеция имеет нулевую или слишком малую высоту.");
            }

            firstBaseValue = baseA;
            secondBaseValue = baseB;
            firstSideValue = sideC;
            secondSideValue = sideD;

            heightValue =
                CheckResult(height);

            constructionLogValue +=
                " → Конструктор Trapezoid";
        }

        // Площадь трапеции: S = ((a + b) / 2) * h.
        public override double CalculateArea()
        {
            double averageBase =
                firstBaseValue / 2 +
                secondBaseValue / 2;

            return CheckResult(
                averageBase * heightValue);
        }

        // Периметр трапеции: P = a + b + c + d.
        public override double CalculatePerimeter()
        {
            return CheckResult(
                firstBaseValue +
                secondBaseValue +
                firstSideValue +
                secondSideValue);
        }

        // GetDescription() наследуется без переопределения.
    }
}