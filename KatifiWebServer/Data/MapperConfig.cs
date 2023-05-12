using AutoMapper;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Models.SecurityModels;

namespace KatifiWebServer.Data;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Address, AddressDTO>();
        CreateMap<Address, AddressDTO>().ReverseMap();

        CreateMap<Church, ChurchDTO>();
        CreateMap<Church, ChurchDTO>().ReverseMap()
            .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Address.Id));

        CreateMap<Mess, MessDTO>();
        CreateMap<Mess, MessDTO>().ReverseMap();

        CreateMap<Community, CommunityDTO>();
        CreateMap<Community, CommunityDTO>().ReverseMap()
            .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Address.Id))
            .ForMember(dest => dest.MemberCount, opt => opt.Ignore());

        CreateMap<Member, MemberDTO>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.UserAge, opt => opt.MapFrom(src => src.User.Age));
        CreateMap<Member, MemberDTO>().ReverseMap();

        CreateMap<Event, EventDTO>();
        CreateMap<Event, EventDTO>().ReverseMap()
            .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Address.Id))
            .ForMember(dest => dest.ParticipantCount, opt => opt.Ignore());

        CreateMap<Participant, ParticipantDTO>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.UserAge, opt => opt.MapFrom(src => src.User.Age));
        CreateMap<Participant, ParticipantDTO>().ReverseMap();

        CreateMap<AppUser, UserDTO>();
        CreateMap<AppUser, UserDTO>().ReverseMap();

        //Identity 
        CreateMap<RegisterModel, AppUser>();
        CreateMap<RegisterModel, AppUser>().ReverseMap();

    }
}
