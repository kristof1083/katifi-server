using AutoMapper;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Models.SecurityModels;
using Microsoft.Build.Framework;

namespace KatifiWebServer.Data;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Address, AddressDTO>();
        CreateMap<Address, AddressDTO>().ReverseMap();

        CreateMap<Church, ChurchDTO>();
        CreateMap<Church, ChurchDTO>().ReverseMap();

        CreateMap<Mess, MessDTO>();
        CreateMap<Mess, MessDTO>().ReverseMap();

        CreateMap<Community, CommunityDTO>();
        CreateMap<Community, CommunityDTO>().ReverseMap();

        CreateMap<AppUser, UserDTO>();
        CreateMap<AppUser, UserDTO>().ReverseMap();

        CreateMap<Member, MemberDTO>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CommunityName, opt => opt.MapFrom(src => src.Community.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));
        CreateMap<Member, MemberDTO>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CommunityName, opt => opt.MapFrom(src => src.Community.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName)).ReverseMap();

        CreateMap<Event, EventDTO>();
        CreateMap<Event, EventDTO>().ReverseMap();

        CreateMap<Participant, ParticipantDTO>();
        CreateMap<Participant, ParticipantDTO>().ReverseMap();


        //Identity 
        CreateMap<RegisterModel, AppUser>();
        CreateMap<RegisterModel, AppUser>().ReverseMap();

    }
}
