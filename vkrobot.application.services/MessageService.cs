using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using vkborot.application.dto;
using vkrobot.application.data;
using vkrobot.application.exceptions;
using vkrobot.application.interfaces;
using vkrobot.application.services.Utilities;

namespace vkrobot.application.services;

public class MessageService: TraceableService, IMessageService
{
    private readonly ApplicationData _context;
    private readonly ILogger<MessageService> _logger;
    private readonly IMapper _mapper;
    
    public MessageService(ApplicationData context, IMapper mapper, ILogger<MessageService> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MessageDto>?> GetListAsync(IDictionary<string, string> criteria, int? skip, int? take)
    {
        try
        {

            var query = _context.Messages.AsQueryable().AsNoTracking();
            
            foreach (var search in criteria)
                query = query.BuildExpression(_context, search.Key, search.Value);
            
            if (skip.HasValue && take.HasValue)
                query = query.Skip(skip.Value).Take(take.Value);
            
            var records = await query.ToListAsync();
            var dto = _mapper.Map<List<Message>, List<MessageDto>>(records);
            _trace(_logger,  "List messaged exeeuted");
            return dto;
        }
        catch (Exception e)
        {
            _trace(_logger, ": List messages exception {e.Message}");
            throw new DataException(e);
        }
    }

    public async Task<MessageDto?> GetAsync(Guid id)
    {
        try
        {
            _trace(_logger, "Get message {id}");
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                var dto = _mapper.Map<Message, MessageDto>(message);
                _trace_with_data(_logger, "Message requested {id}", dto);
                return dto;
            }
        }
        catch (Exception e)
        {
            _trace(_logger, "Get message exception {e.Message}");
            throw new DataException(e);
        }

        _trace(_logger, "Message not found {id}");
        throw new NotFoundException();
    }

    public async Task AddAsync(MessageDto dto)
    {
        try
        {
            _trace_with_data(_logger, "Add message", dto);
            var message = _mapper.Map<MessageDto, Message>(dto);
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            _trace_with_data(_logger, "message created {0}", dto);
        }
        catch (Exception e)
        {
            _trace(_logger, "Create message Exception {e.Message}");
            throw new DataException(e);
        }
    }
}