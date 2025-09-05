using AutoMapper;
using EstudoReact.Model;
using EstudoReact.Server.DTO;

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
                 .ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.Documento))
                 .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            // Mapeamento de CompradoresDTO para Comprador
            CreateMap<CompradoresDTO, Comprador>()
                 .ForMember(dest => dest.IdCidade, opt => opt.MapFrom(src => src.IdCidade))
                 .ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.Documento))
                 .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Cidade, opt => opt.Ignore()); // Cidade será resolvida separadamente
        }
    }
}
