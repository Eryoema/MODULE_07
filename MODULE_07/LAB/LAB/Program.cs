using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB
{
    public interface IShippingStrategy
    {
        decimal CalculateShippingCost(decimal weight, decimal distance);
    }

    public class StandardShipping : IShippingStrategy
    {
        public decimal CalculateShippingCost(decimal weight, decimal distance)
        {
            return weight * 0.5m + distance * 0.2m;
        }
    }

    public class ExpressShipping : IShippingStrategy
    {
        public decimal CalculateShippingCost(decimal weight, decimal distance)
        {
            return weight * 1.0m + distance * 0.5m;
        }
    }

    public class InternationalShipping : IShippingStrategy
    {
        public decimal CalculateShippingCost(decimal weight, decimal distance)
        {
            return weight * 1.5m + distance * 1.0m; 
        }
    }

    public class NightShipping : IShippingStrategy
    {
        private const decimal UrgencyFee = 10.0m;

        public decimal CalculateShippingCost(decimal weight, decimal distance)
        {
            return (weight * 1.0m + distance * 0.5m) + UrgencyFee;
        }
    }

    public class DeliveryContext
    {
        private IShippingStrategy _shippingStrategy;

        public void SetShippingStrategy(IShippingStrategy shippingStrategy)
        {
            _shippingStrategy = shippingStrategy;
        }

        public decimal CalculateShippingCost(decimal weight, decimal distance)
        {
            if (weight < 0 || distance < 0)
            {
                throw new ArgumentException("Вес и расстояние должны быть положительными.");
            }

            if (_shippingStrategy == null)
            {
                throw new InvalidOperationException("Стратегия доставки не выбрана.");
            }

            return _shippingStrategy.CalculateShippingCost(weight, distance);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            DeliveryContext deliveryContext = new DeliveryContext();

            deliveryContext.SetShippingStrategy(new StandardShipping());
            Console.WriteLine("Стандартная доставка: " + deliveryContext.CalculateShippingCost(5, 10));

            deliveryContext.SetShippingStrategy(new ExpressShipping());
            Console.WriteLine("Экспресс-доставка: " + deliveryContext.CalculateShippingCost(5, 10));

            deliveryContext.SetShippingStrategy(new InternationalShipping());
            Console.WriteLine("Международная доставка: " + deliveryContext.CalculateShippingCost(5, 10));

            deliveryContext.SetShippingStrategy(new NightShipping());
            Console.WriteLine("Ночная доставка: " + deliveryContext.CalculateShippingCost(5, 10));

            try
            {
                Console.WriteLine("Проверка на отрицательные значения:");
                Console.WriteLine(deliveryContext.CalculateShippingCost(-1, 10));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
