using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using CoreBlog.Common.Configuration;
using Microsoft.IdentityModel.Tokens;
using CoreBlog.Extensions.Authorizations.Policys;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CoreBlog.Extensions.ServiceSetup
{
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // 以下四种常见的授权方式。
            // 1、这个很简单，其他什么都不用做， 只需要在API层的controller上边，增加特性即可
            // [Authorize(Roles = "Admin,System")]

            // 2、这个和上边的异曲同工，好处就是不用在controller中，写多个 roles 。
            // 然后这么写 [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
                options.AddPolicy("A_S_O", policy => policy.RequireRole("Admin", "System", "Others"));
            });

            #region 参数
            //读取配置文件
            var symmetricKeyAsBase64 = AppSettingsConstVars.JwtConfigSecretKey;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var issuer = AppSettingsConstVars.JwtConfigIssuer;
            var audience = AppSettingsConstVars.JwtConfigAudience;

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,//参数配置在下边
                    ValidateIssuer = true,
                    ValidIssuer = issuer,//发行人
                    ValidateAudience = true,
                    ValidAudience = audience,//订阅人
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
                    RequireExpirationTime = true,
                };
            });



            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<PermissionItem>();

            // 角色与接口的权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",// 拒绝授权的跳转地址（目前无用）
                permission,
                ClaimTypes.Role,//基于角色的授权
                issuer,//发行人
                audience,//听众
                signingCredentials,//签名凭据
                expiration: TimeSpan.FromSeconds(60 * 60 * 1)//接口的过期时间
                );
            #endregion

            // 3、自定义复杂的策略授权
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(Permissions.Name,
            //             policy => policy.Requirements.Add(permissionRequirement));
            //});



        }
    }
}
