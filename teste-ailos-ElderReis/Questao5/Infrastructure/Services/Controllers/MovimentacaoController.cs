using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.CommandStore;
using System.Security.Cryptography;
using System.Text;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CriarMovimentacao([FromBody] CriarMovimentacaoRequest request)
        {
            var chaveIdempotencia = request.IdRequesicao;

            var idempotencia = await _mediator.Send(new ObterIdempotenciaQuery(chaveIdempotencia));
            if (idempotencia != null)
            {
                return Ok(idempotencia.Resultado);
            }

            var response = await _mediator.Send(new CriarMovimentacaoCommand(request));
            await _mediator.Send(new CriarIdempotenciaCommand(chaveIdempotencia, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response)));

            return Ok(response);
        }
    }
}
