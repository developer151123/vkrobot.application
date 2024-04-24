using vkborot.application.dto;

namespace vkrobot.application.interfaces;

public interface IMessageService
{
    Task<IEnumerable<MessageDto>?> GetListAsync(IDictionary<string, string> criteria, int? skip, int? take);
    Task<MessageDto?> GetAsync(Guid id);
    Task AddAsync(MessageDto dto);
}