using InvestmentDecisionDss.Data;
using InvestmentDecisionDss.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentDecisionDss.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DecisionController : ControllerBase
    {
        private readonly DssContext _context;
        private readonly IDssEngine _engine;

        public DecisionController(DssContext context, IDssEngine engine)
        {
            _context = context;
            _engine = engine;
        }

        [HttpGet("evaluate")]
        public IActionResult Evaluate([FromQuery] string strategy = "SAW")
        {
            var result = _engine.Evaluate(_context.Alternatives, _context.Criteria, strategy);
            return Ok(result);
        }
    }
}

