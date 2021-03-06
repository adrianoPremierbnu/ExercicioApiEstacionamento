using ExercicioApiEstacionamento.Enumerados;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace ExercicioApiEstacionamento.Entidades
{
    public class Diaria : EntidadeBase
    {
        
        public Diaria(DateTime dataHoraInicio, Veiculo veiculo, bool diariaAdquirida,
                      bool duchaAdquirida) : base(Guid.NewGuid())
        {
            DataHoraInicio = dataHoraInicio;
            Veiculo = veiculo;
            DiariaAdquirida = diariaAdquirida;
            DuchaAdquirida = duchaAdquirida;

        }

        public DateTime DataHoraInicio { get; private set; }
        public DateTime DataHoraFim { get; private set; }
        public DateTime DataPagamento { get; private set; }
        public Veiculo Veiculo { get; private set; }
        public bool DiariaAdquirida { get; private set; }
        public bool DuchaAdquirida { get; private set; }
        public decimal ValorDiaria { get; private set; }
        public bool DiariaFinalizada { get; private set; }
        public Pagamento? Pagamento { get; private set; }

        public void AtualizarDiaria(DateTime horaFim, Startup.MinhasConfiguracoes options)
        {
            DataHoraFim = horaFim;
            DiariaFinalizada = true;

            TimeSpan ts = DataHoraFim - DataHoraInicio;

            if (Veiculo.TipoVeiculo == ETipoVeiculo.Carro)
            {
                if (DiariaAdquirida)
                    ValorDiaria = options.DiariaCarro;
                else if (DuchaAdquirida)
                    ValorDiaria = options.DuchaCarro;
                else if (ts.TotalMinutes < options.TempoLimite)
                    ValorDiaria = options.AbaixoQuinzeMinutosCarro;
                else
                    ValorDiaria = options.AcimaQuinzeMinutosCarro;
            }
            else
            {
                if (DiariaAdquirida)
                    ValorDiaria = options.DiariaMoto;
                else if (ts.TotalMinutes < options.TempoLimite)
                    ValorDiaria = options.AbaixoQuinzeMinutosMoto;
                else ValorDiaria = options.AcimaQuinzeMinutosMoto;

            }

        }

        public void AdicionarVeiculo(Veiculo veiculo)
        {
            Veiculo = veiculo;
                           
        }

        public void FinalizarPagamento(Pagamento pagamento)
        {
            if (pagamento.Valido)
            {
                Pagamento = pagamento;

                this.DataPagamento = DateTime.Now;
                
                //efetiva pagamento
            }
            else
            {
                throw new Exception("Forma de pagamento inválida!!");
            }

        }

    }
}
