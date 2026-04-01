using System.Collections.Generic;

namespace InvestmentDecisionDss.Data
{
    public class DssContext
    {
        public List<Alternative> Alternatives { get; } = new List<Alternative>();
        public List<Criterion> Criteria { get; } = new List<Criterion>();

        public DssContext()
        {
            // Define criteria
            Criteria.Add(new Criterion
            {
                Id = "expected_return",
                Name = "Очікувана дохідність",
                Weight = 0.4,
                Direction = CriterionDirection.Maximize
            });

            Criteria.Add(new Criterion
            {
                Id = "volatility",
                Name = "Волатильність",
                Weight = 0.3,
                Direction = CriterionDirection.Minimize
            });

            Criteria.Add(new Criterion
            {
                Id = "dividend_yield",
                Name = "Дивідендна дохідність",
                Weight = 0.15,
                Direction = CriterionDirection.Maximize
            });

            Criteria.Add(new Criterion
            {
                Id = "pe_ratio",
                Name = "Коефіцієнт P/E",
                Weight = 0.15,
                Direction = CriterionDirection.Minimize
            });

            // Add alternatives with realistic example values
            Alternatives.Add(new Alternative
            {
                Id = "tech_company",
                Name = "Технологічна компанія",
                Values = new Dictionary<string, double>
                {
                    { "expected_return", 0.18 }, // 18%
                    { "volatility", 0.35 }, // 35% std dev
                    { "dividend_yield", 0.01 }, // 1%
                    { "pe_ratio", 30.0 }
                }
            });

            Alternatives.Add(new Alternative
            {
                Id = "startup",
                Name = "Стартап",
                Values = new Dictionary<string, double>
                {
                    { "expected_return", 0.40 }, // 40%
                    { "volatility", 0.6 }, // 60%
                    { "dividend_yield", 0.0 },
                    { "pe_ratio", 100.0 }
                }
            });

            Alternatives.Add(new Alternative
            {
                Id = "government_bonds",
                Name = "Державні облігації",
                Values = new Dictionary<string, double>
                {
                    { "expected_return", 0.05 }, // 5%
                    { "volatility", 0.05 }, // 5%
                    { "dividend_yield", 0.03 }, // 3% coupon
                    { "pe_ratio", 12.0 }
                }
            });

            Alternatives.Add(new Alternative
            {
                Id = "dividend_aristocrat",
                Name = "Дивідендний аристократ",
                Values = new Dictionary<string, double>
                {
                    { "expected_return", 0.09 }, // 9%
                    { "volatility", 0.12 }, // 12%
                    { "dividend_yield", 0.04 }, // 4%
                    { "pe_ratio", 18.0 }
                }
            });
        }
    }
}

