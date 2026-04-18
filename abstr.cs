namespace Lab5;

public class abstr
{
    public interface ITrackable
    {
        string GetTrackingCode();
    }
    
    public abstract class Delivery : ITrackable
    {
        public string Sender { get; set; }
        public double Weight { get; set; }
        public decimal BaseCost { get; set; }
        
        protected string TrackingCode { get; set; }

        public Delivery(string sender, double weight, decimal baseCost)
        {
            Sender = sender;
            Weight = weight;
            BaseCost = baseCost;
            TrackingCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
        
        public abstract decimal CalculateCost();
        
        public virtual string GetInfo()
        {
            return $"Відправник: {Sender} | Вага: {Weight} кг | Базова вартість: {BaseCost} грн";
        }
        
        public string GetTrackingCode()
        {
            return TrackingCode;
        }
    }
    
    public class DocumentDelivery : Delivery
    {
        public bool IsExpress { get; set; }

        public DocumentDelivery(string sender, double weight, decimal baseCost, bool isExpress) 
            : base(sender, weight, baseCost)
        {
            IsExpress = isExpress;
            TrackingCode = "DOC-" + TrackingCode;
        }

        public override decimal CalculateCost()
        {
            decimal finalCost = BaseCost;
            if (IsExpress) finalCost += 50m;
            return finalCost;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $" | Тип: Документи | Експрес: {(IsExpress ? "Так" : "Ні")}";
        }
    }

    // Клас: Доставка посилок
    public class PackageDelivery : Delivery
    {
        public bool IsFragile { get; set; } // Специфічна властивість (крихке)

        public PackageDelivery(string sender, double weight, decimal baseCost, bool isFragile) 
            : base(sender, weight, baseCost)
        {
            IsFragile = isFragile;
            TrackingCode = "PKG-" + TrackingCode;
        }

        public override decimal CalculateCost()
        {
            // Вартість залежить від ваги + націнка за крихкість
            decimal finalCost = BaseCost + (decimal)(Weight * 15);
            if (IsFragile) finalCost *= 1.2m; // +20% до ціни
            return finalCost;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $" | Тип: Посилка | Крихке: {(IsFragile ? "Так" : "Ні")}";
        }
    }
    
    public class CargoDelivery : Delivery
    {
        public bool RequiresLoader { get; set; }

        public CargoDelivery(string sender, double weight, decimal baseCost, bool requiresLoader) 
            : base(sender, weight, baseCost)
        {
            RequiresLoader = requiresLoader;
            TrackingCode = "CRG-" + TrackingCode;
        }

        public override decimal CalculateCost()
        {
            decimal finalCost = BaseCost + (decimal)(Weight * 40);
            if (RequiresLoader) finalCost += 800m;
            return finalCost;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $" | Тип: Вантаж | Потребує вантажника: {(RequiresLoader ? "Так" : "Ні")}";
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            List<Delivery> deliveries = new List<Delivery>
            {
                new DocumentDelivery("Іваненко І.І.", 0.2, 40m, isExpress: true),
                new DocumentDelivery("Петренко П.П.", 0.5, 40m, isExpress: false),
                new PackageDelivery("Сидоренко С.С.", 5.5, 70m, isFragile: true),
                new PackageDelivery("Коваленко К.К.", 12.0, 70m, isFragile: false),
                new CargoDelivery("ТОВ 'БудСервіс'", 250.0, 300m, requiresLoader: true)
            };

            Console.WriteLine("=== СПИСОК ВІДПРАВЛЕНЬ ===\n");
            
            foreach (var delivery in deliveries)
            {
                Console.WriteLine(delivery.GetInfo());
                Console.WriteLine($"Кінцева вартість: {delivery.CalculateCost():F2} грн");
                
                if (delivery is ITrackable trackable)
                {
                    Console.WriteLine($"Трекінг-код: {trackable.GetTrackingCode()}");
                }
                Console.WriteLine(new string('-', 60));
            }
            
            var mostExpensive = deliveries.OrderByDescending(d => d.CalculateCost()).First();
            Console.WriteLine($"\n[Найдорожче відправлення]");
            Console.WriteLine($"Відправник: {mostExpensive.Sender}, Вартість: {mostExpensive.CalculateCost():F2} грн");
            
            decimal averageCost = deliveries.Average(d => d.CalculateCost());
            Console.WriteLine($"\n[Середня вартість усіх відправлень]: {averageCost:F2} грн");
            
            Console.ReadLine();
        }
    }
}