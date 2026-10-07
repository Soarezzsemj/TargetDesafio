using System.Globalization;

namespace TargetDesafio
{
    public class Questao3Juros
    {
        private const decimal TaxaDiaria = 0.025m;

        private static readonly CultureInfo CulturaBr = new("pt-BR");

        public void ExecutarQuestao()
        {
            bool calcularOutro;

            do
            {
                Console.WriteLine("CÁLCULO DE JUROS POR ATRASO");
                Console.WriteLine();

                decimal? valor = LerValor();
                if (valor == null)
                    return;

                DateTime? vencimento = LerVencimento();
                if (vencimento == null)
                    return;

                ExibirResultado(valor.Value, vencimento.Value);

                calcularOutro = DesejaCalcularOutro();

                if (calcularOutro)
                    Console.Clear();
            } while (calcularOutro);
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

        private static void ExibirResultado(decimal valor, DateTime vencimento)
        {
            DateTime hoje = DateTime.Today;
            int diasAtraso = CalcularDiasAtraso(vencimento, hoje);
            decimal juros = CalcularJuros(valor, diasAtraso);
            decimal total = valor + juros;

            Console.WriteLine();
            Console.WriteLine($"Valor original: {valor.ToString("C", CulturaBr)}");
            Console.WriteLine($"Vencimento: {vencimento.ToString("dd/MM/yyyy", CulturaBr)}");
            Console.WriteLine($"Data de hoje: {hoje.ToString("dd/MM/yyyy", CulturaBr)}");

            if (diasAtraso == 0)
            {
                Console.WriteLine("O título não está vencido, então não há juros.");
            }
            else
            {
                string taxa = (TaxaDiaria * 100).ToString("0.0", CulturaBr);

                Console.WriteLine($"Dias em atraso: {diasAtraso}");
                Console.WriteLine($"Taxa: {taxa}% ao dia");
                Console.WriteLine($"Cálculo: {valor.ToString("C", CulturaBr)} × {taxa}% × {diasAtraso} dias");
            }

            Console.WriteLine($"Juros: {juros.ToString("C", CulturaBr)}");
            Console.WriteLine($"Total a pagar: {total.ToString("C", CulturaBr)}");
        }

        private static decimal? LerValor()
        {
            while (true)
            {
                Console.Write("Digite o valor Original cobrado (use vírgula, ex.: 1500,75) ou Enter para voltar: ");

                string? entrada = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(entrada))
                    return null;

                if (decimal.TryParse(entrada.Trim(), NumberStyles.AllowDecimalPoint, CulturaBr, out decimal valor) && valor > 0)
                    return valor;

                Console.WriteLine("Valor inválido. Digite um número maior que zero, com vírgula nos centavos.");
            }
        }

        private static DateTime? LerVencimento()
        {
            while (true)
            {
                Console.Write("Digite a data de vencimento (dd/MM/yyyy) ou Enter para voltar: ");

                string? entrada = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(entrada))
                    return null;

                if (DateTime.TryParseExact(entrada.Trim(), "dd/MM/yyyy", CulturaBr, DateTimeStyles.None, out DateTime vencimento))
                    return vencimento;

                Console.WriteLine("Data inválida. Use o formato dd/MM/yyyy, por exemplo 15/09/2026.");
            }
        }

        private static bool DesejaCalcularOutro()
        {
            Console.WriteLine();
            Console.Write("Deseja calcular outro valor? (S/N): ");

            string? resposta = Console.ReadLine()?.Trim();

            return string.Equals(resposta, "S", StringComparison.OrdinalIgnoreCase);
        }
    }
}