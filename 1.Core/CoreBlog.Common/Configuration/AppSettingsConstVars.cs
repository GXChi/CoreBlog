using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBlog.Common.Configuration
{
    public class AppSettingsConstVars
    {

        #region Jwt授权配置================================================================================

        public static readonly string JwtConfigSecretKey = AppSettingsHelper.GetContent("JwtConfig", "SecretKey");
        public static readonly string JwtConfigIssuer = AppSettingsHelper.GetContent("JwtConfig", "Issuer");
        public static readonly string JwtConfigAudience = AppSettingsHelper.GetContent("JwtConfig", "Audience");
        #endregion

    }
}
