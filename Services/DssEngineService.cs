using InvestmentDecisionDss.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestmentDecisionDss.Services
{
    public class DssEngineService : IDssEngine
    {
        public DecisionResult Evaluate(IEnumerable<Alternative> alternatives, IEnumerable<Criterion> criteria, string strategy = "SAW")
        {
            var altList = alternatives.ToList();
            var critList = criteria.ToList();

            // Build raw matrix values: [alternative][criterionId]
            var matrix = new Dictionary<string, Dictionary<string, double>>();
            foreach (var a in altList)
            {
                matrix[a.Id] = new Dictionary<string, double>();
                foreach (var c in critList)
                {
                    if (a.Values.TryGetValue(c.Id, out var v)) matrix[a.Id][c.Id] = v;
                    else matrix[a.Id][c.Id] = 0.0;
                }
            }

            // Normalize each criterion to 0..1
            var normalized = new Dictionary<string, Dictionary<string, double>>();
            foreach (var c in critList)
            {
                var values = altList.Select(a => matrix[a.Id][c.Id]).ToList();
                double min = values.Min();
                double max = values.Max();

                foreach (var a in altList)
                {
                    if (!normalized.ContainsKey(a.Id)) normalized[a.Id] = new Dictionary<string, double>();

                    double raw = matrix[a.Id][c.Id];
                    double norm;
                    if (Math.Abs(max - min) < 1e-12)
                    {
                        norm = 0.0; // no variation
                    }
                    else if (c.Direction == CriterionDirection.Maximize)
                    {
                        norm = (raw - min) / (max - min);
                    }
                    else // Minimize
                    {
                        // for minimize, lower raw is better -> invert
                        norm = (max - raw) / (max - min);
                    }

                    normalized[a.Id][c.Id] = norm;
                }
            }

            var results = new Dictionary<string, double>();

            if (string.Equals(strategy, "MEW", StringComparison.OrdinalIgnoreCase))
            {
                // Multiplicative Exponential Weights: product(norm_i ^ weight_i)
                foreach (var a in altList)
                {
                    double prod = 1.0;
                    foreach (var c in critList)
                    {
                        // To avoid zeroing out, use (epsilon + norm)
                        double norm = Math.Max(1e-12, normalized[a.Id][c.Id]);
                        prod *= Math.Pow(norm, c.Weight);
                    }
                    results[a.Name] = prod;
                }
            }
            else
            {
                // Default SAW: sum(weight * norm)
                foreach (var a in altList)
                {
                    double sum = 0.0;
                    foreach (var c in critList)
                    {
                        sum += c.Weight * normalized[a.Id][c.Id];
                    }
                    results[a.Name] = sum;
                }
            }

            // Sort descending
            var sorted = results.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);

            // Choose best
            var best = sorted.FirstOrDefault();
            string bestAlt = best.Key ?? string.Empty;

            // Explanation: find the criterion where the best alternative has the highest normalized weighted contribution
            string explanation = "";
            if (!string.IsNullOrEmpty(bestAlt))
            {
                var bestAltObj = altList.First(a => a.Name == bestAlt);
                // compute contributions
                var contributions = new List<(string CriterionName, double Contribution)>();
                foreach (var c in critList)
                {
                    double norm = normalized[bestAltObj.Id][c.Id];
                    double contrib = c.Weight * norm;
                    contributions.Add((c.Name, contrib));
                }
                var top = contributions.OrderByDescending(t => t.Contribution).First();
                explanation = $"Перемога альтернативи '{bestAlt}' пояснюється її високою оцінкою по критерію '{top.CriterionName}' (внесок: {top.Contribution:F3}).";
            }

            return new DecisionResult
            {
                Results = sorted,
                BestAlternative = bestAlt,
                Explanation = explanation
            };
        }
    }
}

