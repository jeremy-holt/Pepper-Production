namespace PCal.Models
{
    public class FarmProductCoverage
    {
        public FarmProductCoverage(int year, double quantity)
        {
            Year = year;
            Quantity = quantity;
        }

        public int Year { get; }
        public double Quantity { get; }
    }
}
