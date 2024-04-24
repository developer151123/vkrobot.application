using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;

namespace vkrobot.application.webapi.Helpers
{

    internal sealed class DataSourceLoadOptions : DataSourceLoadOptionsBase
    {
        public static ValueTask<DataSourceLoadOptions> BindAsync(HttpContext httpContext)
        {
            DataSourceLoadOptions? loadOptions = new DataSourceLoadOptions();
            DataSourceLoadOptionsParser.Parse(loadOptions, key => httpContext.Request.Query[key]);
            return ValueTask.FromResult(loadOptions);
        }

    }
}
