using System;
using System.Globalization;

namespace Questao1
{
    public class ContaBancaria {
        public long NumeroDaConta { get; private set; }
        public string Nome { get; set; }
        private double Saldo { get; set; }

        public ContaBancaria(long numeroDaConta, string nome, double saldo = 0)
        {
            NumeroDaConta = numeroDaConta;
            Nome = nome;
            Saldo = saldo;
        }

        public void Deposito(double valorDeposito)
        {
            Saldo += Math.Abs(valorDeposito);
        }

        public void Saque(double valorSaque)
        {
            Saldo -= Math.Abs(valorSaque);
            double taxaSaque = 3.50;
            Saldo -= taxaSaque;
        }

        public override string ToString()
        {
            return $"Conta {NumeroDaConta}, Titular: {Nome}, Saldo: $ {Saldo.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"))}";
        }
    }
}
