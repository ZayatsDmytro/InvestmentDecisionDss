using InvestmentDecisionDss.Data;
using System.Collections.Generic;

namespace InvestmentDecisionDss.Services
{
	public interface IDssEngine
	{
		DecisionResult Evaluate(IEnumerable<Alternative> alternatives, IEnumerable<Criterion> criteria, string strategy = "SAW");
	}
}


