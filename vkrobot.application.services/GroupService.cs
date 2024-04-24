using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using vkborot.application.dto;
using vkrobot.application.data;
using vkrobot.application.exceptions;
using vkrobot.application.interfaces;
using vkrobot.application.services.Utilities;

namespace vkrobot.application.services;

public class GroupService : TraceableService, IGroupService
{
    private readonly ApplicationData _context;
    private readonly ILogger<GroupService> _logger;
    private readonly IMapper _mapper;
    
    public GroupService(ApplicationData context, IMapper mapper, ILogger<GroupService> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupDto>?> GetAsync()
    {
        try
        {
            _trace(_logger, "List groups ");
            var records = await _context.Groups.ToListAsync();
            var dto = _mapper.Map<List<Group>, List<GroupDto>>(records);
            _trace(_logger, "List groups is ready");
            return dto;
        }
        catch (Exception e)
        {
            _trace(_logger, $"List groups exception {e.Message}");
            throw new DataException(e);
        }
    }

    public async Task<GroupDto?> GetAsync(Guid id)
    {
        try
        {
            _trace(_logger, $"Get group {id}");
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                var dto = _mapper.Map<Group, GroupDto>(group);
                _trace_with_data(_logger, $"Group requested {id}", dto);
                return dto;
            }
        }
        catch (Exception e)
        {
            _trace(_logger, $"Get group exception {e.Message}");
            throw new DataException(e);
        }

        _trace(_logger, $"Group not found {id}");
        throw new NotFoundException();
    }

    public async Task<GroupDto?> CreateAsync(GroupDto dto)
    {
        try
        {
            _trace_with_data(_logger, "Create group", dto);
            var group = _mapper.Map<GroupDto, Group>(dto);
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            dto = _mapper.Map<Group, GroupDto>(group);
            _trace_with_data(_logger, "Group created {0}", dto);
            return dto;
        }
        catch (Exception e)
        {
            _trace(_logger, $"Create Group Exception {e.Message}");
            throw new DataException(e);
        }
    }

    public async Task UpdateAsync(Guid id, GroupDto dto)
    {
        var notFound = false;
        try
        {
            _trace(_logger,  $"Update group {id}");
            var entity = await _context.Groups.FindAsync(id);
            if (entity == null)
                notFound = true;
            else
            {
                _mapper.Map<GroupDto, Group>(dto, entity);
                await _context.SaveChangesAsync();
                _trace(_logger, $"Group updated {id}");
            }
        }
        catch (Exception e)
        {
            _trace(_logger, $"Group update exception {e.Message}" );
            throw new DataException(e);
        }
        if (notFound)
        {
            _trace(_logger, $"Group not found {id}");
            throw new NotFoundException();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        bool notFound = false;
        try
        {
            _trace(_logger, $"Delete group {id}" );
            var entity = await _context.Groups.FindAsync(id);
            if (entity == null)
                notFound = true;
            else
            {
                _context.Groups.Remove(entity);
                await _context.SaveChangesAsync();
                _trace(_logger, $"Delete group {id}");
            }
        }
        catch (Exception e)
        {
            _trace(_logger, $"Delete group exception {e.Message}");
            throw new DataException(e);
        }
        
        if (notFound)
        {
            _trace(_logger, $"Group not found {id}");
            throw new NotFoundException();
        }
    }

    public async Task ImportGroups(IEnumerable<ImportGroupDto> groups)
    {
        try
        {
            _trace(_logger, "Import groups" );
            var groupsAdded = false;
            foreach (var group in groups)
            {
                var g = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == group.GroupId);
                if (g != null) continue;
                
                var groupToAdd = new Group
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    Private = group.Private,
                    User = group.User,
                    Password = group.Password,
                    LastScan = DateTime.Now.ToUniversalTime()
                };

                groupsAdded = true;
                _context.Groups.Add(groupToAdd);
            }

            if (groupsAdded)
                await _context.SaveChangesAsync();
            _trace(_logger, "New groups are added");
        }
        catch (Exception e)
        {
            _trace(_logger, $"Import exception {e.Message}");
            throw new DataException(e);
        }
    }
}