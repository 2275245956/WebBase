using System;

namespace Z2.Core.WebApp.Repository
{
    public class Mst_ProjectModel
    {
        public string ProjectId{get;set;}
        public string ProjectCode{get;set;}
        public string ProjectName{get;set;}
        public int ActualWork {get;set;}
        public int DisplayNo{get;set;}
        public bool DeleteFlag{get;set;}
        public bool EnabledFlag {get;set;}
        public string Description{get;set;}
        public DateTime CreaterTime {get;set;}
        public string CreaterUserId{get;set;}
        public DateTime LastUpdateTime {get;set;}
        public string LastUpdateUserId{get;set;}
        public DateTime DeleteTime {get;set;}
        public string DeleteUserId{get;set;}
    }
}