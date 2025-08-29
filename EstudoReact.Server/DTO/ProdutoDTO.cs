using Estudo.Model.Enum;

namespace EstudoReact.Server.DTO
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public OrigemProduto Origem { get; set; }
    }
}
