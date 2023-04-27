using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class ConsultarSaldosCommand : IRequest<ResultadoOperacao>
    {
        public ConsultarSaldoRequest Request { get; }

        public ConsultarSaldosCommand(ConsultarSaldoRequest request)
        {
            Request = request;
        }
    }

    public class ConsultarSaldosCommandHandler : IRequestHandler<ConsultarSaldosCommand, ResultadoOperacao>
    {
        private readonly DatabaseConfig databaseConfig;

        public ConsultarSaldosCommandHandler(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<ResultadoOperacao> Handle(ConsultarSaldosCommand command, CancellationToken cancellationToken)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            ContaCorrente contaCorrente = await connection.QuerySingleOrDefaultAsync<ContaCorrente>("SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente", new { IdContaCorrente = command.Request.IdContaCorrente });
            if (contaCorrente == null)
            {
                return ResultadoOperacao.SendFalha("INVALID_ACCOUNT", "Conta não encontrada");
            }
            else
            {
                if (!contaCorrente.Ativo)
                    return ResultadoOperacao.SendFalha("INACTIVE_ACCOUNT", "Conta Desativada");
            }

            // Verifica as movimentações da conta corrente
            var movimentacoes = await connection.QueryAsync<Movimentacao>("SELECT * FROM movimento WHERE idcontacorrente = @IdContaCorrente", new { IdContaCorrente = command.Request.IdContaCorrente });

            var saldo = 0.0;
            if (movimentacoes == null)
            {
                return ResultadoOperacao.SendSucesso("0");
            }

            saldo = movimentacoes.Sum(m => m.TipoMovimento == 'C' ? m.Valor : -m.Valor);

            var response = new
            {
                NumeroContaCorrente = contaCorrente.Numero,
                NomeTitulaDaConta = contaCorrente.Nome,
                DataResponse = DateTime.Now,
                SaldoAtual = saldo,
            };
            return ResultadoOperacao.SendSucesso(response);
        }
    }
}
