using System.Globalization;

namespace TargetDesafio
{
    public class Questao3Juros
    {
        private const decimal TaxaDiaria = 0.025m;

        private static readonly CultureInfo CulturaBr = new("pt-BR");

        public void ExecutarQuestao()
        {
            Console.WriteLine("CÁLCULO DE JUROS POR ATRASO");
            Console.WriteLine();

            decimal? valor = LerValor();
            if (valor == null)
                return;

            DateTime? vencimento = LerVencimento();
            if (vencimento == null)
                return;

            DateTime hoje = DateTime.Today;
            int diasAtraso = CalcularDiasAtraso(vencimento.Value, hoje);
            decimal juros = CalcularJuros(valor.Value, diasAtraso);
            decimal total = valor.Value + juros;

            Console.WriteLine();
            Console.WriteLine($"Valor original: {valor.Value.ToString("C", CulturaBr)}");
            Console.WriteLine($"Vencimento: {vencimento.Value.ToString("dd/MM/yyyy", CulturaBr)}");
            Console.WriteLine($"Data de hoje: {hoje.ToString("dd/MM/yyyy", CulturaBr)}");

            if (diasAtraso == 0)
            {
                Console.WriteLine("O título não está vencido, então não há juros.");
            }
            else
            {
                Console.WriteLine($"Dias em atraso: {diasAtraso}");
                Console.WriteLine($"Taxa: {(TaxaDiaria * 100).ToString("0.0", CulturaBr)}% ao dia");
            }

            Console.WriteLine($"Juros: {juros.ToString("C", CulturaBr)}");
            Console.WriteLine($"Total a pagar: {total.ToString("C", CulturaBr)}");
        }

        public static int CalcularDiasAtraso(DateTime vencimento, DateTime hoje)
        {
            int dias = (hoje.Date - vencimento.Date).Days;

            return Math.Max(0, dias);
        }

        public static decimal CalcularJuros(decimal valor, int diasAtraso)
        {
            decimal juros = valor * TaxaDiaria * diasAtraso;

            return Math.Round(juros, 2, MidpointRounding.AwayFromZero);
        }

        private static decimal? LerValor()
        {
            Console.Write("Digite o valor original (use vírgula, ex.: 1500,75): ");

            string? entrada = Console.ReadLine();

            if (!decimal.TryParse(entrada, NumberStyles.AllowDecimalPoint, CulturaBr, out decimal valor) || valor <= 0)
            {
                Console.WriteLine("Valor inválido. Digite um número maior que zero, com vírgula nos centavos.");
                return null;
            }

            return valor;
        }

        private static DateTime? LerVencimento()
        {
            Console.Write("Digite a data de vencimento (dd/MM/yyyy): ");

            string? entrada = Console.ReadLine();

            if (!DateTime.TryParseExact(entrada, "dd/MM/yyyy", CulturaBr, DateTimeStyles.None, out DateTime vencimento))
            {
                Console.WriteLine("Data inválida. Use o formato dd/MM/yyyy, por exemplo 15/09/2026.");
                return null;
            }

            return vencimento;
        }
    }
}