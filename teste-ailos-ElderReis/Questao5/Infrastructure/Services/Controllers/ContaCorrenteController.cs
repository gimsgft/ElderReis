using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Database.CommandStore;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarSaldo([FromQuery] ConsultarSaldoRequest request)
        {
            var response = await _mediator.Send(new ConsultarSaldosCommand(request));
            return Ok(response);
        }
    }
}
