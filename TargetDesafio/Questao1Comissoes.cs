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

                Dictionary<string, List<Venda>> vendasPorVendedor = AgruparPorVendedor(vendas);

                ExibirResumo(vendasPorVendedor);

                if (DesejaVerDetalhes())
                    ExibirDetalhes(vendasPorVendedor);
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

        private static Dictionary<string, List<Venda>> AgruparPorVendedor(List<Venda> vendas)
        {
            var vendasPorVendedor = new Dictionary<string, List<Venda>>();

            foreach (var venda in vendas)
            {
                if (string.IsNullOrWhiteSpace(venda.vendedor))
                {
                    Console.WriteLine("Venda sem vendedor informado foi ignorada.");
                    continue;
                }

                if (!vendasPorVendedor.TryGetValue(venda.vendedor, out List<Venda>? lista))
                {
                    lista = new List<Venda>();
                    vendasPorVendedor[venda.vendedor] = lista;
                }

                lista.Add(venda);
            }

            return vendasPorVendedor;
        }

        private static decimal CalcularTotal(List<Venda> vendas)
        {
            return vendas.Sum(venda => CalcularComissao(venda.valor));
        }

        private static void ExibirResumo(Dictionary<string, List<Venda>> vendasPorVendedor)
        {
            Console.WriteLine("COMISSÃO DE CADA VENDEDOR");
            Console.WriteLine();

            foreach (var item in vendasPorVendedor)
            {
                decimal total = Math.Round(CalcularTotal(item.Value), 2, MidpointRounding.AwayFromZero);
                string rotulo = item.Value.Count == 1 ? "venda" : "vendas";

                Console.WriteLine($"{item.Key}: {total.ToString("C", CulturaBr)} ({item.Value.Count} {rotulo})");
            }
        }

        private static bool DesejaVerDetalhes()
        {
            Console.WriteLine();
            Console.Write("Deseja ver o detalhamento das vendas? (S/N): ");

            string? resposta = Console.ReadLine()?.Trim();

            return string.Equals(resposta, "S", StringComparison.OrdinalIgnoreCase);
        }

        private static void ExibirDetalhes(Dictionary<string, List<Venda>> vendasPorVendedor)
        {
            Console.WriteLine();
            Console.WriteLine("DETALHAMENTO DAS VENDAS");

            foreach (var item in vendasPorVendedor)
            {
                Console.WriteLine();
                Console.WriteLine(item.Key);

                foreach (var venda in item.Value)
                {
                    decimal comissao = CalcularComissao(venda.valor);

                    Console.WriteLine($"  Venda de {venda.valor.ToString("C", CulturaBr)} -> comissão {comissao.ToString("C4", CulturaBr)}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Comissões por venda exibidas com 4 casas, o total soma sem arredondar e arredonda só no final");
        }
    }
}