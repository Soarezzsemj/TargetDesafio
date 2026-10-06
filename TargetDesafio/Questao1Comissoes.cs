using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TargetDesafio.Models;

namespace TargetDesafio
{
    public class Questao1Comissoes
    {

        public void ExecutarQuestao()
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            try {

                string jsonString = File.ReadAllText("C:\\Users\\carlo\\source\\repos\\TargetDesafio\\TargetDesafio\\Data\\vendas.json");

                VendasJson dadosRaiz = JsonSerializer.Deserialize<VendasJson>(jsonString, options);

                List<Venda> vendas = dadosRaiz.Vendas;

                Console.WriteLine($"Sucesso! Foram lidas {vendas.Count} vendas.");

            }
            catch (FileNotFoundException)
            {

                Console.WriteLine("Arquivo vendas.json não encontrado.");

                return;

            }
            catch (JsonException ex)
            {

                Console.WriteLine(ex.Message);

            }





        }





    }
}
