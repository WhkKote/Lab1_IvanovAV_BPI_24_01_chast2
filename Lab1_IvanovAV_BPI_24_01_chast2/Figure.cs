using System;

namespace Lab1_IvanovAV_BPI_24_01
{
    public abstract class Figure : IFigure
    {
        private readonly string name;
        protected string constructionLogValue;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string ConstructionLog
        {
            get
            {
                return constructionLogValue;
            }
        }

        protected Figure(string name)
        {
            this.name = name;
            constructionLogValue = "Конструктор Figure";
        }

        public abstract double CalculateArea();

        public abstract double CalculatePerimeter();

        public virtual string GetDescription()
        {
            return "Фигура: " + name;
        }

        protected static void CheckSize(double value, string title)
        {
            if (double.IsNaN(value) ||
                double.IsInfinity(value) ||
                value <= 0)
            {
                throw new ArgumentException(
                    title + ": требуется конечное положительное число.");
            }
        }
        //валидация 
        protected static double CheckResult(double value)
        {
            if (double.IsNaN(value) ||
                double.IsInfinity(value) ||
                value <= 0)
            {
                throw new ArithmeticException(
                    "Не удалось вычислить положительный конечный результат. Проверьте размеры фигуры.");
            }

            return value;
        }
    }
}