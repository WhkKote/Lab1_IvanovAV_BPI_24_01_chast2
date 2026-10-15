using System;

namespace Lab1_IvanovAV_BPI_24_01
{
    public class Ellipse : Figure
    {
        private readonly double semiMajorAxis;
        private readonly double semiMinorAxis;

        public double SemiMajorAxis
        {
            get
            {
                return semiMajorAxis;
            }
        }

        public double SemiMinorAxis
        {
            get
            {
                return semiMinorAxis;
            }
        }

        public Ellipse(double semiMajorAxis, double semiMinorAxis)
            : base("Эллипс")
        {
            CheckSize(semiMajorAxis, "Первая полуось");
            CheckSize(semiMinorAxis, "Вторая полуось");

            this.semiMajorAxis =
                Math.Max(semiMajorAxis, semiMinorAxis);

            this.semiMinorAxis =
                Math.Min(semiMajorAxis, semiMinorAxis);

            constructionLogValue +=
                " → Конструктор Ellipse";
        }

        // S = Pi * a * b.
        public override double CalculateArea()
        {
            return CheckResult(
                (semiMajorAxis * semiMinorAxis) * Math.PI);
        }

        // P = 2 * Pi * sqrt((a² + b²) / 2), приближённо.
        public override double CalculatePerimeter()
        {
            double largest =
                Math.Max(semiMajorAxis, semiMinorAxis);

            double smallest =
                Math.Min(semiMajorAxis, semiMinorAxis);

            double ratio = smallest / largest;

            return CheckResult(
                largest *
                (2 * Math.PI *
                 Math.Sqrt((1 + ratio * ratio) / 2)));
        }

        public override string GetDescription()
        {
            return base.GetDescription() +
                   ". Периметр вычисляется приближённо.";
        }
    }
}