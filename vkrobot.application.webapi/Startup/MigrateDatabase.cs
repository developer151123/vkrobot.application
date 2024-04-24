using Autofac;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using vkborot.application.dto;
using vkrobot.application.data;
using vkrobot.application.interfaces;
using vkrobot.application.webapi.Configuration;

namespace vkrobot.application.webapi.Startup
{
    internal class MigrateDatabase : IStartable
    {
        private readonly ApplicationData _context;
        private readonly ApplicationConfiguration _config;
        private readonly ILogger<MigrateDatabase> _logger;
        private readonly IGroupService _groupService;
        private readonly bool _isStartedFromTool;
        
      
        public MigrateDatabase(ApplicationData context, ApplicationConfiguration config, IGroupService groupService, ILogger<MigrateDatabase> logger, bool isStartedFromTool)
        {
            _context = context;
            _logger = logger;
            _config = config;
            _groupService = groupService;
            _isStartedFromTool = isStartedFromTool;
        }

        public async void Start()
        {
            try
            {
                if (_isStartedFromTool)
                {
                    _logger.LogInformation("Init database migrations from CLI");
                    return;
                }
                else
                {
                    _logger.LogInformation("Start database migrations for connection as {0}", _config.ConnectionString);
                    await _context.Database.MigrateAsync();
                    _logger.LogInformation("Migrations are set up");

                    if (File.Exists(_config.GroupFile))
                    {
                        List<ImportGroupDto>? groups = null;
                        using (StreamReader r = new StreamReader(_config.GroupFile))
                        {
                            var json = await r.ReadToEndAsync();
                            groups = JsonConvert.DeserializeObject<List<ImportGroupDto>>(json);
                            var numberOfGroups = groups.Count; 
                            _logger.LogInformation("{numberOfGroups} Groups were read from file", numberOfGroups);
                        }

                        await _groupService.ImportGroups(groups);
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Model database migration exception");
            }
        }
    }
}
