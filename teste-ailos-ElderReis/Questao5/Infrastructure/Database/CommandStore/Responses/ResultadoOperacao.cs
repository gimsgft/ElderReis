using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Database.CommandStore.Responses
{
    public class ResultadoOperacao
    {
        public bool Sucesso { get; private set; }
        public string Mensagem { get; private set; }
        public string TipoFalha { get; private set; }
        public Object Dados { get; private set; }


        private ResultadoOperacao(bool sucesso, string mensagem, Object dados = null, string tipoFalha = null)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Dados = dados;
            TipoFalha = tipoFalha;
        }

        public static ResultadoOperacao SendSucesso(Object dados)
        {
            return new ResultadoOperacao(true, null, dados);
        }

        public static ResultadoOperacao SendErro(string mensagem)
        {
            return new ResultadoOperacao(false, mensagem, null);
        }

        public static ResultadoOperacao SendFalha(string tipoFalha, string mensagem)
        {
            return new ResultadoOperacao(false, mensagem, null, tipoFalha);
        }

    }
}
