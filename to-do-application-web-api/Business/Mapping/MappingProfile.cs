using AutoMapper;

namespace to_do_application_web_api.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Data.Entity.VpTodoItem, Data.Schema.TodoItemResponse>()
                                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))

                ;
            CreateMap<Data.Schema.TodoItemRequest, Data.Entity.VpTodoItem>()
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                ;

        }
    }
}
