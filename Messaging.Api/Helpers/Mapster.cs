using System.Reflection;
using Mapster;
using MapsterMapper;
using Messaging.Api.Domain.Dto;
using Messaging.Api.Domain.Entities;

namespace Messaging.Api.Helpers
{
    public static class MappingExtension
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
        public static T To<T>(this object from) => TypeAdapter.Adapt<T>(from);
    }

    public class Mappers : IRegister
    {
        void IRegister.Register(TypeAdapterConfig config)
        {
            config.NewConfig<Message, MessageDto>() 
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.Room, src => src.Room)
                .TwoWays()
                .MaxDepth(2);
            
            config.NewConfig<Room, RoomDto>()
                .Map(dest => dest.Users, src => src.Users)
                .Map(dest => dest.Messages, src => src.Messages)
                .TwoWays()
                .MaxDepth(2);
            
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.Rooms, src => src.Rooms)
                .Map(dest => dest.Messages, src => src.Messages)
                .TwoWays()
                .MaxDepth(2);
        }
    }
}