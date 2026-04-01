using System.Collections.Generic;

namespace InvestmentDecisionDss.Data
{
    public class Criterion
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Weight { get; set; }
        public CriterionDirection Direction { get; set; }
    }

    public class Alternative
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        // Key is criterion Id, value is raw score
        public Dictionary<string, double> Values { get; set; } = new Dictionary<string, double>();
    }

    public class DecisionResult
    {
        // Sorted results: Alternative name -> score
        public Dictionary<string, double> Results { get; set; } = new Dictionary<string, double>();
        public string BestAlternative { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }
}

