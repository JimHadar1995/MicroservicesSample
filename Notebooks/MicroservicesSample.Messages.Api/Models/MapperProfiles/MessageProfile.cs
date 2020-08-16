using AutoMapper;
using Google.Protobuf.WellKnownTypes;

namespace MicroservicesSample.Notebooks.Api.Models.MapperProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageProfile : Profile
    {
        /// <inheritdoc />
        public MessageProfile()
        {
            CreateMap<CreateMessageGrpc, CreateNotebookDto>()
                .ForAllOtherMembers(_ => _.UseDestinationValue());
            
            CreateMap<NotebookDto, MessageGrpc>()
                .ForMember(_ => _.CreatedAt, o => 
                    o.MapFrom(_ => _.CreatedAt.ToUniversalTime().ToTimestamp()))
                // .ForMember(_ => _.CreatedAt, o => o.Ignore())
                .ForAllOtherMembers(_ => _.UseDestinationValue());
            
            
                
        }
    }
}
