using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2
{
    public interface IObserver
    {
        void Update(float temperature);
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    public class WeatherStation : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        private float _temperature;

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            if (!_observers.Remove(observer))
            {
                throw new InvalidOperationException("Наблюдатель не найден.");
            }
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_temperature);
            }
        }

        public float Temperature
        {
            get => _temperature;
            set
            {
                if (value < -50 || value > 50) 
                {
                    throw new ArgumentOutOfRangeException("Температура должна быть в диапазоне от -50 до 50 градусов.");
                }
                _temperature = value;
                Notify();
            }
        }
    }

    public class WeatherDisplay : IObserver
    {
        private string _displayName;

        public WeatherDisplay(string displayName)
        {
            _displayName = displayName;
        }

        public void Update(float temperature)
        {
            Console.WriteLine($"{_displayName} обновлен: Температура = {temperature}°C");
        }
    }

    public class AlertSystem : IObserver
    {
        public void Update(float temperature)
        {
            Console.WriteLine($"Система оповещения: Температура изменена на {temperature}°C");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            WeatherStation weatherStation = new WeatherStation();

            WeatherDisplay display1 = new WeatherDisplay("Дисплей 1");
            WeatherDisplay display2 = new WeatherDisplay("Дисплей 2");
            AlertSystem alertSystem = new AlertSystem();

            weatherStation.Attach(display1);
            weatherStation.Attach(display2);
            weatherStation.Attach(alertSystem);

            try
            {
                weatherStation.Temperature = 25.5f; 
                weatherStation.Temperature = 30.0f; 
                weatherStation.Temperature = 55.0f; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            weatherStation.Detach(display1);
            Console.WriteLine("Дисплей 1 удален.");

            weatherStation.Temperature = 20.0f;
        }
    }
}