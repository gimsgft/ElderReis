namespace Questao5.Application.Commands.Requests
{
    public class CriarMovimentacaoRequest
    {
        public string IdRequesicao { get; set; }
        public string IdContaCorrente { get; set; }
        public string TipoMovimento { get; set; }
        public string DataMovimento { get; set; }
        public double Valor { get; set; }
    }
}
