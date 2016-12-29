using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCal.Models
{
    public class FarmProduct : BaseDictionaryItem
    {
        public FarmProduct(string id, string name, CoverageType coverageType) : base(id, name)
        {
            CoverageType = coverageType;
        }

        public FarmProduct()
        {
        }

        public FarmProduct(string name, CoverageType coverageType) : this(null, name, coverageType)
        {
        }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public CoverageType CoverageType { get; private set; }

        public double Price { get; set; }

        private string Suffix
        {
            get
            {
                switch (CoverageType)
                {
                    case CoverageType.KgPerHectare:
                        return "kg/ha";
                    case CoverageType.GramsPerPlant:
                        return "gm/plant";
                    case CoverageType.LitresPerHectare:
                        return "lt/ha";
                    default:
                        return "kg/ha";
                }
            }
        }

        public List<FarmProductCoverage> Coverages { get; } = new List<FarmProductCoverage>();

        public string CoverageText => GetCoverageText();

        private string GetCoverageText()
        {
            var sb = new StringBuilder();
            foreach (var coverage in Coverages)
            {
                var message = coverage != Coverages.Last()
                    ? $"Year {coverage.Year}: {coverage.Quantity} {Suffix}"
                    : $"Year {coverage.Year} and onwards: {coverage.Quantity} {Suffix}";
                sb.Append(message);
                sb.Append(", ");
            }
            if (sb.Length > 2)
                sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }


        public void AddCoverage(int year, double quantity)
        {
            Coverages.Add(new FarmProductCoverage(year, quantity));
        }
    }

    public enum CoverageType
    {
        KgPerHectare,
        LitresPerHectare,
        GramsPerPlant
    }
}