using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBlog.Common.Configuration
{
    public class AppSettingsHelper
    {
        static IConfiguration configuration { get; set; }
        public AppSettingsHelper(string contentPath)
        {
            configuration = new ConfigurationBuilder()
                   .SetBasePath(contentPath) /*Directory.GetCurrentDirectory()*/
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                   .Build();
        }

        public static string GetContent(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return "";
        }
    }
}
