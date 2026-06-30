using WebApplicationMinimalAPINetCoreTreinamento.Application.DTOs;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Entities;
using AutoMapper;

namespace WebApplicationMinimalAPINetCoreTreinamento.Application.Mappings
{
    public class CepMappingProfile: Profile
    {
        public CepMappingProfile()
        {
            CreateMap<Cep, CepDtos>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Logradouro, opt => opt.MapFrom(src => src.Logradouro))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.Complemento, opt => opt.MapFrom(src => src.Complemento))
                .ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => src.Bairro))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.Cidade))
                .ForMember(dest => dest.Uf, opt => opt.MapFrom(src => src.Uf));
                //.ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.Pais))
                //.ForMember(dest => dest.Complemento, opt => opt.MapFrom(src => src.Complemento))
                //.ForMember(dest => dest.UnidadeFederativa, opt => opt.MapFrom(src => src.UnidadeFederativa))
                //.ForMember(dest => dest.Ibge, opt => opt.MapFrom(src => src.Ibge))
                //.ForMember(dest => dest.Gia, opt => opt.MapFrom(src => src.Gia))
                //.ForMember(dest => dest.Ddd, opt => opt.MapFrom(src => src.Ddd))
                //.ForMember(dest => dest.Siafi, opt => opt.MapFrom(src => src.Siafi))
                //.ForMember(dest => dest.UltimaAtualizacao, opt => opt.MapFrom(src => src.UltimaAtualizacao))
                //.ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo))
                //.ForMember(dest => dest.Versao, opt => opt.MapFrom(src => src.Versao))
                //.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                //.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                //.ReverseMap();

            CreateMap<CreateCepDto, Cep>()
                .ConstructUsing(src => new Cep(
                    src.Codigo,
                    src.Logradouro,
                    src.Numero,
                    src.Complemento,
                    src.Bairro,
                    src.Cidade,
                    src.Uf//,
                    //src.Pais,
                    //src.Complemento,
                    //src.UnidadeFederativa,
                    //src.Ibge,
                    //src.Gia,
                    //src.Ddd,
                    //src.Siafi
                ));

            CreateMap<UpdateCepDto, Cep>()
                .ForMember(dest => dest.Codigo, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Versao, opt => opt.Ignore());
        }
    }
}
