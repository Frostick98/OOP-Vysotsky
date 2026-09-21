using System;

namespace Lab1V2
{
    // Клас Car згідно з варіантом №2
    class Car
    {
        // Приватні поля
        private string brand;
        private string model;
        private int year;

        // Публічні властивості
        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1886 || value > DateTime.Now.Year)
                    throw new ArgumentException("Некоректний рік випуску автомобіля.");
                year = value;
            }
        }

        // Конструктор
        public Car(string brand, string model, int year)
        {
            this.brand = brand;
            this.model = model;
            Year = year;
        }

        // Метод, що виконує дію, пов'язану з класом
        public void Drive()
        {
            Console.WriteLine($"{brand} {model} ({year} р.) вирушає в поїздку. Дорога вільна, двигун заведено!");
        }

        // Деструктор (фіналізатор) - демонструє життєвий цикл об'єкта
        ~Car()
        {
            Console.WriteLine($"Об'єкт Car ({brand} {model}) знищено збирачем сміття.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Створення 3 об'єктів класу Car
            Car car1 = new Car("Toyota", "Corolla", 2019);
            Car car2 = new Car("BMW", "X5", 2021);
            Car car3 = new Car("Skoda", "Octavia", 2017);

            // Виклик методів для виведення результату
            car1.Drive();
            car2.Drive();
            car3.Drive();

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}
