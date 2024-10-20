using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOM
{
    public interface IPaymentStrategy
    {
        void Pay(decimal amount);
    }

    public class CreditCardPayment : IPaymentStrategy
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Оплата {amount} через банковскую карту.");
        }
    }

    public class PayPalPayment : IPaymentStrategy
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Оплата {amount} через PayPal.");
        }
    }

    public class CryptocurrencyPayment : IPaymentStrategy
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Оплата {amount} криптовалютой.");
        }
    }

    public class PaymentContext
    {
        private IPaymentStrategy _paymentStrategy;

        public void SetPaymentStrategy(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public void ExecutePayment(decimal amount)
        {
            _paymentStrategy.Pay(amount);
        }
    }

    public interface IObserver
    {
        void Update(decimal exchangeRate);
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    public class CurrencyExchange : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        private decimal _exchangeRate;

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_exchangeRate);
            }
        }

        public void SetExchangeRate(decimal exchangeRate)
        {
            _exchangeRate = exchangeRate;
            Notify();
        }
    }

    public class PrintObserver : IObserver
    {
        public void Update(decimal exchangeRate)
        {
            Console.WriteLine($"Курс валют обновлен: {exchangeRate}");
        }
    }

    public class LogObserver : IObserver
    {
        public void Update(decimal exchangeRate)
        {
            Console.WriteLine($"Лог: Курс валют изменен на {exchangeRate}");
        }
    }

    public class NotifyObserver : IObserver
    {
        public void Update(decimal exchangeRate)
        {
            Console.WriteLine($"Уведомление: новый курс валют - {exchangeRate}");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            PaymentContext paymentContext = new PaymentContext();

            paymentContext.SetPaymentStrategy(new CreditCardPayment());
            paymentContext.ExecutePayment(100);

            paymentContext.SetPaymentStrategy(new PayPalPayment());
            paymentContext.ExecutePayment(200);

            paymentContext.SetPaymentStrategy(new CryptocurrencyPayment());
            paymentContext.ExecutePayment(300);

            CurrencyExchange currencyExchange = new CurrencyExchange();

            PrintObserver printObserver = new PrintObserver();
            LogObserver logObserver = new LogObserver();
            NotifyObserver notifyObserver = new NotifyObserver();

            currencyExchange.Attach(printObserver);
            currencyExchange.Attach(logObserver);
            currencyExchange.Attach(notifyObserver);

            currencyExchange.SetExchangeRate(74.50m);
            currencyExchange.SetExchangeRate(75.00m);
        }
    }
}