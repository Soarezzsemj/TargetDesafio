using TargetDesafio;


int opcao = 0;

do {
    Console.Clear();

    Console.WriteLine("TESTE TÉCNICO TARGET SISTEMAS");
    Console.WriteLine("Escolha a opção");
    Console.WriteLine();

    Console.WriteLine("1 - Questão Comissões");
    Console.WriteLine("2 - Questão Estoque");
    Console.WriteLine("3 - Questão Juros");
    Console.WriteLine("0 - Sair"); 
    Console.WriteLine();
    Console.Write("Digite sua opção: ");

    string? entrada = Console.ReadLine();
    bool sucesso = int.TryParse(entrada, out opcao);

    
    if (!sucesso)
    {
        opcao = -1;
    }


  

    switch (opcao)
    {
        case 1:
            Console.Clear(); 
            Questao1Comissoes questao1 = new Questao1Comissoes();
            questao1.ExecutarQuestao();

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey(); 
            break;

        case 2:
            Console.Clear();
            Questao2Estoque questao2 = new Questao2Estoque();
            questao2.ExecutarQuestao();

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            break;

        case 3:
            Console.Clear();
            Questao3Juros questao3 = new Questao3Juros();
            questao3.ExecutarQuestao();

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            break;

        case 0:
            Console.WriteLine("Encerrando o programa...");
            break;

        default:
           
            Console.WriteLine("\nEntrada inválida! Digite apenas os números do menu.");
            Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
            Console.ReadKey();
            break;
    }


} while (opcao != 0);
