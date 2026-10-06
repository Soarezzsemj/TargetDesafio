namespace TargetDesafio.Models
{
    public class Venda
    {
        public string vendedor { get; set; }

        public decimal valor { get; set; }

        

    }
    public class VendasJson
    {
       
        public List<Venda> Vendas { get; set; }
    }

}
