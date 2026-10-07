using System.Globalization;
using System.Text.Json;
using TargetDesafio.Models;

namespace TargetDesafio
{
    
    public class Questao1Comissoes
    {
        private const decimal LimiteFaixa1 = 100m;
        private const decimal LimiteFaixa2 = 500m;
        private const decimal PercentualFaixa1 = 0.01m;
        private const decimal PercentualFaixa2 = 0.05m;

        private static readonly CultureInfo CulturaBr = new("pt-BR");

        public void ExecutarQuestao()
        {
            try
            {
                List<Venda> vendas = CarregarVendas();

                if (vendas.Count == 0)
                {
                    Console.WriteLine("Nenhuma venda encontrada em vendas.json.");
                    return;
                }

                Dictionary<string, decimal> totais = CalcularTotaisPorVendedor(vendas);

                ExibirResultado(totais);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Não foi possível ler o arquivo vendas.json: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"O arquivo vendas.json está em formato inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado na questão 1: {ex.Message}");
            }
        }

        public static decimal CalcularComissao(decimal valorVenda)
        {
            if (valorVenda >= LimiteFaixa2)
                return valorVenda * PercentualFaixa2;

            if (valorVenda >= LimiteFaixa1)
                return valorVenda * PercentualFaixa1;

            return 0m;
        }

        private static List<Venda> CarregarVendas()
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            string caminho = Path.Combine(AppContext.BaseDirectory, "Data", "vendas.json");
            string jsonString = File.ReadAllText(caminho);

            VendasJson? dadosRaiz = JsonSerializer.Deserialize<VendasJson>(jsonString, options);

            return dadosRaiz?.Vendas ?? new List<Venda>();
        }

        private static Dictionary<string, decimal> CalcularTotaisPorVendedor(List<Venda> vendas)
        {
            var totais = new Dictionary<string, decimal>();

            foreach (var venda in vendas)
            {
                if (string.IsNullOrWhiteSpace(venda.vendedor))
                {
                    Console.WriteLine("Venda sem vendedor informado foi ignorada.");
                    continue;
                }

                decimal comissao = CalcularComissao(venda.valor);

                totais[venda.vendedor] = totais.GetValueOrDefault(venda.vendedor) + comissao;
            }

            return totais;
        }

        private static void ExibirResultado(Dictionary<string, decimal> totais)
        {
            Console.WriteLine("COMISSÃO DE CADA VENDEDOR");
            Console.WriteLine();

            foreach (var item in totais)
            {
                decimal total = Math.Round(item.Value, 2, MidpointRounding.AwayFromZero);

                Console.WriteLine($"{item.Key}: {total.ToString("C", CulturaBr)}");
            }
        }
    }
}