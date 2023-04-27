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
    public class CriarMovimentacaoCommand : IRequest<ResultadoOperacao>
    {
        public CriarMovimentacaoRequest Request { get; set; }

        public CriarMovimentacaoCommand(CriarMovimentacaoRequest request)
        {
            Request = request;
        }
    }

    public class CriarMovimentacaoCommandHandler : IRequestHandler<CriarMovimentacaoCommand, ResultadoOperacao>
    {
        private readonly DatabaseConfig databaseConfig;

        public CriarMovimentacaoCommandHandler(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<ResultadoOperacao> Handle(CriarMovimentacaoCommand command, CancellationToken cancellationToken)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var contaCorrente = await connection.QuerySingleOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
                new { command.Request.IdContaCorrente });

            if (contaCorrente == null)
            {
                return ResultadoOperacao.SendErro("Conta corrente não encontrada");
            }

            var idMovimento = Guid.NewGuid().ToString();
            var movimentacao = new Movimentacao
            {
                IdMovimento = idMovimento,
                IdContaCorrente = command.Request.IdContaCorrente,
                DataMovimento = command.Request.DataMovimento,
                TipoMovimento = command.Request.TipoMovimento[0],
                Valor = command.Request.Valor
            };

            var resultado = await connection.ExecuteAsync(
                "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
                new
                {
                    IdMovimento = idMovimento,
                    IdContaCorrente = movimentacao.IdContaCorrente,
                    DataMovimento = movimentacao.DataMovimento,
                    TipoMovimento = movimentacao.TipoMovimento.ToString(),
                    Valor = movimentacao.Valor
                });

            if (resultado == 1)
            {
                return ResultadoOperacao.SendSucesso(movimentacao.IdMovimento);
            }
            else
            {
                return ResultadoOperacao.SendErro("Erro ao inserir movimentação");
            }
        }
    }
}
