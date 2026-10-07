# Desafio Técnico - Target Sistemas

Solução em C# (.NET 8) para o desafio técnico da Target Sistemas. O programa é um aplicativo de console com um menu que dá acesso às três questões do desafio.

Autor: Carlos Eduardo Soares

## Como executar

Pré-requisito: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
git clone https://github.com/Soarezzsemj/TargetDesafio.git
cd TargetDesafio/TargetDesafio
dotnet run
```

Também é possível abrir `TargetDesafio.sln` no Visual Studio e executar com F5.

Os arquivos `vendas.json` e `estoque.json` ficam na pasta `Data` e são copiados para a pasta de saída do build.

## Menu principal

```
1 - Questão Comissões
2 - Questão Estoque
3 - Questão Juros
0 - Sair
```

Entradas inválidas no menu são recusadas com uma mensagem, sem encerrar o programa.

## Questões

### 1. Cálculo de comissão

Lê `Data/vendas.json` e calcula a comissão de cada venda, somando por vendedor:

| Valor da venda | Comissão |
| --- | --- |
| abaixo de R$ 100,00 | sem comissão |
| de R$ 100,00 até abaixo de R$ 500,00 | 1% |
| a partir de R$ 500,00 | 5% |

Resultado com os dados do desafio:

```
COMISSÃO DE CADA VENDEDOR

João Silva: R$ 495,68 (10 vendas)
Maria Souza: R$ 465,95 (9 vendas)
Carlos Oliveira: R$ 379,37 (8 vendas)
Ana Lima: R$ 404,98 (9 vendas)

Deseja ver o detalhamento das vendas? (S/N):
```

### 2. Movimentação de estoque

Lê `Data/estoque.json` e abre um submenu para:

- **Lançar movimentação:** escolhe o produto pelo código, o tipo (entrada ou saída), a quantidade e uma descrição da movimentação. Ao final, mostra a quantidade final em estoque do produto.
- **Consultar estoque:** lista todos os produtos com o saldo atual.
- **Histórico de movimentações:** lista os lançamentos feitos na execução, com ID, data e hora, produto, tipo, quantidade e descrição.

Cada movimentação recebe um identificador único e sequencial. Exemplo: a Caneta Azul começa com 150 unidades; uma entrada de 50 resulta em 200, e uma saída de 30 resulta em 170.

Validações: produto inexistente, quantidade menor ou igual a zero e saída maior que o saldo são recusadas com mensagem.

### 3. Cálculo de juros

Recebe um valor e uma data de vencimento (`dd/MM/yyyy`) e calcula os juros até a data de hoje, com taxa de 2,5% ao dia. Mostra o valor original, os dias em atraso, os juros e o total a pagar.

Exemplo: R$ 1.000,00 vencidos há 10 dias geram R$ 250,00 de juros (R$ 25,00 por dia), totalizando R$ 1.250,00.

## Decisões e suposições

- **Limites da comissão:** uma venda de exatamente R$ 100,00 entra na faixa de 1%, e uma de exatamente R$ 500,00 entra na faixa de 5%.
- **Arredondamento da comissão:** a comissão de cada venda é somada sem arredondar, e o arredondamento para duas casas (meio para cima) é feito apenas no total de cada vendedor. Arredondar venda a venda mudaria o total do João Silva em um centavo.
- **Valores monetários:** são tratados com `decimal`, nunca com `double`, para evitar erros de arredondamento.
- **Juros:** o enunciado fala em "multa de 2,5% ao dia", mas foi tratado como juros simples por dia de atraso (valor × 2,5% × dias). Com essa taxa, atrasos longos geram valores muito altos, o que é consequência direta da regra do enunciado.
- **Título não vencido:** se o vencimento é hoje ou uma data futura, os juros são zero.
- **Entrada de valores:** na questão 3, o valor deve ser digitado com vírgula nos centavos (ex.: `1500,75`). Isso evita que `1000.50` seja lido como 100050 por causa do separador de milhar.
- **Estoque:** o estoque e o histórico ficam em memória durante a execução do programa. Os arquivos JSON não são alterados.
- **Descrição da movimentação:** se o usuário deixar a descrição em branco, é usada uma descrição padrão ("Entrada de mercadoria" ou "Saída de mercadoria").
- **Formatação:** valores em reais e datas usam a cultura `pt-BR`, para a saída ser a mesma em qualquer computador.
- **Tratamento de erros:** arquivo ausente, JSON inválido ou lista vazia geram uma mensagem clara em vez de encerrar o programa com erro.

## Estrutura do projeto

```
TargetDesafio/
├── README.md
├── TargetDesafio.sln
└── TargetDesafio/
    ├── Program.cs              (menu principal)
    ├── Questao1Comissoes.cs
    ├── Questao2Estoque.cs
    ├── Questao3Juros.cs
    ├── Models/
    │   ├── Venda.cs
    │   ├── Produto.cs
    │   ├── EstoqueJson.cs
    │   ├── Movimentacao.cs
    │   └── TipoMovimentacao.cs
    └── Data/
        ├── vendas.json
        └── estoque.json
```

## Melhorias possíveis

- Testes unitários para as regras de comissão (limites de R$ 100,00 e R$ 500,00) e de juros.
- Persistir as movimentações de estoque em arquivo ou banco de dados.
- Expor as regras por uma API, com o front-end enviando os dados em JSON.
