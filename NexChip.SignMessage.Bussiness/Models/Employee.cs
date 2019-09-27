using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Bussiness.Models
{
    #region 人员实体

    /// <summary>
    /// 人员信息表
    /// </summary>
    public class Employee
    {
        public string emp_id { get; set; }
        public string cname { get; set; }
        public string cname_u { get; set; }

        /// <summary>
        /// 部门代码
        /// </summary>
        public string dept_id { get; set; }
        /// <summary>
        /// 课别代码
        /// </summary>
        public string subdept_id { get; set; }
        public string email { get; set; }
        public string direct { get; set; }
        /// <summary>
        /// 班别
        /// </summary>
        public string cls { get; set; }
        /// <summary>
        /// 是否在职
        /// </summary>
        public string is_active { get; set; }
        /// <summary>
        /// 分机厂别代码
        /// </summary>
        public string fab_code { get; set; }
        /// <summary>
        /// 分级号码
        /// </summary>
        public string ext { get; set; }
        /// <summary>
        /// 组织代码
        /// </summary>
        public string org_code { get; set; }

    }
    #endregion

}
