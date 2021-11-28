using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class Account
    {
        public int id { get; set; }
        public string name { get; set; }
        public string uuid { get; set; }
        public int parent_account_id { get; set; }
        public int root_account_id { get; set; }
        public int default_storage_quota_mb { get; set; }
        public int default_user_storage_quota_mb { get; set; }
        public int default_group_storage_quota_mb { get; set; }
        public string default_time_zone { get; set; }
        public string sis_account_id { get; set; }
        public string integration_id { get; set; }
        public int sis_import_id { get; set; }
        public string lti_guid { get; set; }
        public string workflow_state { get; set; }
    }
}