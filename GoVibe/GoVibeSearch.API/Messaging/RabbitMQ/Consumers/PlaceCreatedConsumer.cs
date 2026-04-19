using AutoMapper;
using Contracts.Places;
using GoVibeSearch.API.Models;
using GoVibeSearch.API.Services;
using MassTransit;

namespace GoVibeSearch.API.Messaging.RabbitMQ.Consumers
{
    public class PlaceCreatedConsumer : IConsumer<PlaceCreatedEvent>
    {
        private readonly IPlaceSearchService _placeSearchService;
        private readonly IMapper _mapper;

        public PlaceCreatedConsumer(IPlaceSearchService placeSearchService, IMapper mapper)
        {
            _placeSearchService = placeSearchService;
            _mapper = mapper;
        }
        
        public async Task Consume(ConsumeContext<PlaceCreatedEvent> context)
        {
            var message  = context.Message;
            var placeSearch = _mapper.Map<PlaceSearchModel>(message);

            var all = await _placeSearchService.IndexAsync(placeSearch);
            
            // await Task.CompletedTask;
        }
    }
}