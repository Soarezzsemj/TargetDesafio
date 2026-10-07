using System.Globalization;
using System.Text.Json;
using TargetDesafio.Models;

namespace TargetDesafio
{
    
    public class Questao2Estoque
    {
        private static readonly CultureInfo CulturaBr = new("pt-BR");

        private readonly List<Movimentacao> movimentacoes = new();
        private List<Produto> produtos = new();
        private bool carregado = false;
        private int proximoId = 1;

        public void ExecutarQuestao()
        {
            if (!carregado && !TentarCarregarProdutos())
                return;

            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("QUESTÃO ESTOQUE");
                Console.WriteLine();
                Console.WriteLine("1 - Lançar movimentação");
                Console.WriteLine("2 - Consultar estoque");
                Console.WriteLine("3 - Histórico de movimentações");
                Console.WriteLine("0 - Voltar ao menu principal");
                Console.WriteLine();
                Console.Write("Digite sua opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                    opcao = -1;

                switch (opcao)
                {
                    case 1:
                        LancarMovimentacao();
                        Pausar();
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("ESTOQUE ATUAL");
                        Console.WriteLine();
                        ExibirProdutos();
                        Pausar();
                        break;

                    case 3:
                        ExibirHistorico();
                        Pausar();
                        break;

                    case 0:
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Opção inválida! Digite apenas os números do menu.");
                        Pausar();
                        break;
                }
            } while (opcao != 0);
        }

        private bool TentarCarregarProdutos()
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                string caminho = Path.Combine(AppContext.BaseDirectory, "Data", "estoque.json");
                string jsonString = File.ReadAllText(caminho);

                EstoqueJson? dadosRaiz = JsonSerializer.Deserialize<EstoqueJson>(jsonString, options);

                produtos = dadosRaiz?.Estoque ?? new List<Produto>();

                if (produtos.Count == 0)
                {
                    Console.WriteLine("Nenhum produto encontrado em estoque.json.");
                    Pausar();
                    return false;
                }

                carregado = true;
                return true;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Não foi possível ler o arquivo estoque.json: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"O arquivo estoque.json está em formato inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado na questão 2: {ex.Message}");
            }

            Pausar();
            return false;
        }

        private void LancarMovimentacao()
        {
            Console.Clear();
            Console.WriteLine("LANÇAR MOVIMENTAÇÃO");
            Console.WriteLine();
            ExibirProdutos();
            Console.WriteLine();

            Produto? produto = LerProduto();
            if (produto == null)
                return;

            TipoMovimentacao? tipo = LerTipo();
            if (tipo == null)
                return;

            int? quantidade = LerQuantidade();
            if (quantidade == null)
                return;

            if (tipo == TipoMovimentacao.Saida && quantidade > produto.Estoque)
            {
                Console.WriteLine();
                Console.WriteLine($"Saída recusada: o saldo de '{produto.DescricaoProduto}' é {produto.Estoque} e você pediu {quantidade}.");
                return;
            }

            if (tipo == TipoMovimentacao.Entrada && quantidade > int.MaxValue - produto.Estoque)
            {
                Console.WriteLine();
                Console.WriteLine("Entrada recusada: a quantidade ultrapassa o limite permitido para o estoque.");
                return;
            }

            Console.Write("Descrição da movimentação (ex.: Compra, Venda, Devolução): ");
            string? descricao = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(descricao))
                descricao = tipo == TipoMovimentacao.Entrada ? "Entrada de mercadoria" : "Saída de mercadoria";

            produto.Estoque += tipo == TipoMovimentacao.Entrada ? quantidade.Value : -quantidade.Value;

            var movimentacao = new Movimentacao
            {
                Id = proximoId++,
                CodigoProduto = produto.CodigoProduto,
                Tipo = tipo.Value,
                Quantidade = quantidade.Value,
                Descricao = descricao.Trim(),
                DataHora = DateTime.Now
            };

            movimentacoes.Add(movimentacao);

            Console.WriteLine();
            Console.WriteLine($"Movimentação nº {movimentacao.Id} registrada.");
            Console.WriteLine($"Produto: {produto.CodigoProduto} - {produto.DescricaoProduto}");
            Console.WriteLine($"Tipo: {movimentacao.Tipo} | Quantidade: {movimentacao.Quantidade}");
            Console.WriteLine($"Descrição: {movimentacao.Descricao}");
            Console.WriteLine($"Quantidade final em estoque: {produto.Estoque}");
        }

        private Produto? LerProduto()
        {
            Console.Write("Código do produto: ");

            if (!int.TryParse(Console.ReadLine(), out int codigo))
            {
                Console.WriteLine("Código inválido. Digite apenas números.");
                return null;
            }

            Produto? produto = produtos.FirstOrDefault(p => p.CodigoProduto == codigo);

            if (produto == null)
                Console.WriteLine($"Produto {codigo} não encontrado.");

            return produto;
        }

        private static TipoMovimentacao? LerTipo()
        {
            Console.Write("Tipo (1 - Entrada, 2 - Saída): ");

            string? entrada = Console.ReadLine()?.Trim();

            if (entrada == "1")
                return TipoMovimentacao.Entrada;

            if (entrada == "2")
                return TipoMovimentacao.Saida;

            Console.WriteLine("Tipo inválido. Digite 1 para entrada ou 2 para saída.");
            return null;
        }

        private static int? LerQuantidade()
        {
            Console.Write("Quantidade: ");

            if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
            {
                Console.WriteLine("Quantidade inválida. Digite um número inteiro maior que zero.");
                return null;
            }

            return quantidade;
        }

        private void ExibirProdutos()
        {
            foreach (var produto in produtos)
            {
                Console.WriteLine($"{produto.CodigoProduto} - {produto.DescricaoProduto}: {produto.Estoque} un.");
            }
        }

        private void ExibirHistorico()
        {
            Console.Clear();
            Console.WriteLine("HISTÓRICO DE MOVIMENTAÇÕES");
            Console.WriteLine();

            if (movimentacoes.Count == 0)
            {
                Console.WriteLine("Nenhuma movimentação lançada até agora.");
                return;
            }

            foreach (var m in movimentacoes)
            {
                string nomeProduto = produtos.FirstOrDefault(p => p.CodigoProduto == m.CodigoProduto)?.DescricaoProduto ?? "?";

                Console.WriteLine(
                    $"#{m.Id} | {m.DataHora.ToString("dd/MM/yyyy HH:mm", CulturaBr)} | " +
                    $"{m.CodigoProduto} - {nomeProduto} | {m.Tipo} | {m.Quantidade} un. | {m.Descricao}");
            }
        }

        private static void Pausar()
        {
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}