using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRAC
{
    public interface ICostCalculationStrategy
    {
        decimal CalculateCost(decimal basePrice, int passengerCount, bool isDiscounted, string serviceClass);
    }

    public class AirplaneCostCalculation : ICostCalculationStrategy
    {
        public decimal CalculateCost(decimal basePrice, int passengerCount, bool isDiscounted, string serviceClass)
        {
            decimal totalCost = basePrice * passengerCount;

            if (isDiscounted)
            {
                totalCost *= 0.9m;
            }

            if (serviceClass == "Business")
            {
                totalCost *= 1.5m;
            }

            return totalCost;
        }
    }

    public class TrainCostCalculation : ICostCalculationStrategy
    {
        public decimal CalculateCost(decimal basePrice, int passengerCount, bool isDiscounted, string serviceClass)
        {
            decimal totalCost = basePrice * passengerCount;

            if (isDiscounted)
            {
                totalCost *= 0.8m; 
            }

            if (serviceClass == "FirstClass")
            {
                totalCost *= 1.3m;
            }

            return totalCost;
        }
    }

    public class BusCostCalculation : ICostCalculationStrategy
    {
        public decimal CalculateCost(decimal basePrice, int passengerCount, bool isDiscounted, string serviceClass)
        {
            decimal totalCost = basePrice * passengerCount;

            if (isDiscounted)
            {
                totalCost *= 0.95m; 
            }

            return totalCost;
        }
    }

    public class TravelBookingContext
    {
        private ICostCalculationStrategy _costCalculationStrategy;

        public void SetCostCalculationStrategy(ICostCalculationStrategy costCalculationStrategy)
        {
            _costCalculationStrategy = costCalculationStrategy;
        }

        public decimal CalculateTripCost(decimal basePrice, int passengerCount, bool isDiscounted, string serviceClass)
        {
            if (_costCalculationStrategy == null)
            {
                throw new InvalidOperationException("Стратегия расчета стоимости не выбрана.");
            }
            return _costCalculationStrategy.CalculateCost(basePrice, passengerCount, isDiscounted, serviceClass);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            TravelBookingContext context = new TravelBookingContext();

            Console.WriteLine("Выберите тип транспорта (1 - Самолет, 2 - Поезд, 3 - Автобус): ");
            string choice = Console.ReadLine();
            ICostCalculationStrategy strategy = null;

            switch (choice)
            {
                case "1":
                    strategy = new AirplaneCostCalculation();
                    break;
                case "2":
                    strategy = new TrainCostCalculation();
                    break;
                case "3":
                    strategy = new BusCostCalculation();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    return;
            }

            context.SetCostCalculationStrategy(strategy);

            Console.Write("Введите базовую стоимость поездки: ");
            decimal basePrice = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Введите количество пассажиров: ");
            int passengerCount = Convert.ToInt32(Console.ReadLine());

            Console.Write("Есть ли скидка (да/нет): ");
            bool isDiscounted = Console.ReadLine()?.ToLower() == "да";

            string serviceClass = "Economy";
            if (strategy is AirplaneCostCalculation || strategy is TrainCostCalculation)
            {
                Console.Write("Введите класс обслуживания (Economy/Business/FirstClass): ");
                serviceClass = Console.ReadLine();
            }

            try
            {
                decimal totalCost = context.CalculateTripCost(basePrice, passengerCount, isDiscounted, serviceClass);
                Console.WriteLine($"Общая стоимость поездки: {totalCost}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
