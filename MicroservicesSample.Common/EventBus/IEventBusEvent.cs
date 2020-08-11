using System;
using Newtonsoft.Json;

namespace MicroservicesSample.Common.EventBus
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventBusEvent
    {
        // /// <summary>
        // /// 
        // /// </summary>
        // public EventBusEvent()
        // {
        //     Id = Guid.NewGuid().ToString();
        //     CreationDate = DateTime.UtcNow;
        // }
        //
        // /// <summary>
        // /// 
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="createDate"></param>
        // [JsonConstructor]
        // public EventBusEvent(string id, DateTime createDate)
        // {
        //     Id = id;
        //     CreationDate = createDate;
        // }
        //
        // /// <summary>
        // /// идентификтаор события
        // /// </summary>
        // [JsonProperty]
        // public string Id { get; private set; }
        //
        // /// <summary>
        // /// Время создания события
        // /// </summary>
        // [JsonProperty]
        // public DateTime CreationDate { get; private set; }
    }
}
