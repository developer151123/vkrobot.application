using AutoMapper;
using vkborot.application.dto;
using vkrobot.application.data;

namespace vkrobot.application.services.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<GroupDto, Group>().ReverseMap();
            CreateMap<MessageDto, Message>().ReverseMap();

        }
    }
}
