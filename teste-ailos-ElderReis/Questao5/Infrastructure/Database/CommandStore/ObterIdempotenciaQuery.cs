using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class ObterIdempotenciaQuery : IRequest<Idempotencia>
    {
        public ObterIdempotenciaQuery(string chaveIdempotencia)
        {
            ChaveIdempotencia = chaveIdempotencia;
        }

        public string ChaveIdempotencia { get; }
    }

    public class ObterIdempotenciaQueryHandler : IRequestHandler<ObterIdempotenciaQuery, Idempotencia>
    {
        private readonly DatabaseConfig databaseConfig;

        public ObterIdempotenciaQueryHandler(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<Idempotencia> Handle(ObterIdempotenciaQuery request, CancellationToken cancellationToken)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var sql = "SELECT * FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia";
            var idempotencia = await connection.QuerySingleOrDefaultAsync<Idempotencia>(sql, new { request.ChaveIdempotencia });
            return idempotencia;
        }
    }
}
