using AutoMapper;
using Estudo.Model.Enum;
using EstudoReact.Model;
using EstudoReact.Server.DTO;
using EstudoReact.Util;

namespace EstudoReact.Server.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comprador, CompradoresDTO>()
                 .ForMember(dest => dest.CidadeNome, opt => opt.MapFrom(src => src.Cidade.Nome))
                 .ForMember(dest => dest.EstadoNome, opt => opt.MapFrom(src => src.Cidade.UF.Nome))
                 .ForMember(dest => dest.IdEstado, opt => opt.MapFrom(src => src.Cidade.IdUf))
                 .ForMember(dest => dest.IdCidade, opt => opt.MapFrom(src => src.IdCidade))
                 .ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.Documento.SomenteNumeros()))
                 .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            // Mapeamento de CompradoresDTO para Comprador
            CreateMap<CompradoresDTO, Comprador>()
                 .ForMember(dest => dest.IdCidade, opt => opt.MapFrom(src => src.IdCidade))
                 .ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.Documento.SomenteNumeros()))
                 .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Cidade, opt => opt.Ignore()); // Cidade será resolvida separadamente

            CreateMap<Estado, EstadoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome));

            CreateMap<Produto, ProdutoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Origem, opt => opt.MapFrom(src => src.Origem));

            CreateMap<ProdutoDTO, Produto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Origem, opt => opt.MapFrom(src => src.Origem));

            CreateMap<PedidoDTO, Pedido>()
            .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens))
            .ForMember(dest => dest.Comprador, opt => opt.Ignore()); // o relacionamento será resolvido manualmente ou via serviço

            // Pedido → PedidoDTO
            CreateMap<Pedido, PedidoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdComprador, opt => opt.MapFrom(src => src.IdComprador))
                .ForMember(dest => dest.Comprador, opt => opt.MapFrom(src => src.Comprador.Nome))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao))
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens));


            CreateMap<ItemPedidoDTO, ItemPedido>()
             .ForMember(dest => dest.Moeda, opt => opt.MapFrom(src => Enum.Parse<Moeda>(src.Moeda)))
             .ForMember(dest => dest.Produto, opt => opt.Ignore()) // idem
             .ForMember(dest => dest.Pedido, opt => opt.Ignore()); // evitar loop ou referências cíclicas

            // ItemPedido → ItemPedidoDTO
            CreateMap<ItemPedido, ItemPedidoDTO>()
                .ForMember(dest => dest.IdPedido, opt => opt.MapFrom(src => src.IdPedido))
                .ForMember(dest => dest.IdProduto, opt => opt.MapFrom(src => src.IdProduto))
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item))
                .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produto.Nome))
                .ForMember(dest => dest.Moeda, opt => opt.MapFrom(src => Convert.ToInt32(src.Moeda)));
        }
    }
}
