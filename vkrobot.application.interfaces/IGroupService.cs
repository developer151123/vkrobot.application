using vkborot.application.dto;

namespace vkrobot.application.interfaces;

public interface IGroupService
{
    Task<IEnumerable<GroupDto>?> GetAsync();
    Task<GroupDto?> GetAsync(Guid id);
    Task<GroupDto?> CreateAsync(GroupDto dto);
    Task UpdateAsync(Guid id, GroupDto dto);        
    Task DeleteAsync(Guid id);
    Task ImportGroups(IEnumerable<ImportGroupDto> groups);
}