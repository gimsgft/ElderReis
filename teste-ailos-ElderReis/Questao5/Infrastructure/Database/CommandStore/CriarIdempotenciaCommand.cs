using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class CriarIdempotenciaCommand : IRequest<Unit>
    {
        public CriarIdempotenciaCommand(string chaveIdempotencia, string requisicao, string resultado)
        {
            ChaveIdempotencia = chaveIdempotencia;
            Requisicao = requisicao;
            Resultado = resultado;
        }

        public string ChaveIdempotencia { get; }
        public string Requisicao { get; }
        public string Resultado { get; }
    }

    public class CriarIdempotenciaCommandHandler : IRequestHandler<CriarIdempotenciaCommand, Unit>
    {
        private readonly DatabaseConfig databaseConfig;

        public CriarIdempotenciaCommandHandler(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<Unit> Handle(CriarIdempotenciaCommand request, CancellationToken cancellationToken)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var sql = @"INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) 
                    VALUES (@chave_idempotencia, @requisicao, @resultado)";

            var parametros = new
            {
                chave_idempotencia = request.ChaveIdempotencia,
                requisicao = request.Requisicao,
                resultado = request.Resultado
            };

            await connection.ExecuteAsync(sql, parametros);

            return Unit.Value;
        }
    }
}
