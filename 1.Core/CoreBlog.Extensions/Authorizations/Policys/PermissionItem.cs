using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBlog.Extensions.Authorizations.Policys
{
    /// <summary>
    /// 用户或角色或其他凭据实体,就像是订单详情一样
    /// 
    /// </summary>
    public class PermissionItem
    {
        /// <summary>
        /// 用户或角色或其他凭证名称
        /// </summary>
        public virtual string Role { get; set; }

        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public virtual string Authority { get; set; }

        /// <summary>
        /// 路由标识Url
        /// </summary>
        public virtual string RouteUrl { get; set; }
    }
}
