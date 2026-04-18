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
    
}