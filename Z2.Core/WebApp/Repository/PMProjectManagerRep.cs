using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class PMProjectManagerRep
    {
        public List<PMProjectManagerModel> GetFormJson(string keyValue, string keyWrod, string Order)
        {
            var sb = new StringBuilder();
            var result = new List<PMProjectManagerModel>();
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();
                sb.AppendFormat(@"select 
                               [ProjectId]
                              ,[ProjectCode]
                              ,[ProjectName]
                              ,[ActualWork]
                              ,[DisplayNo]
                              ,[DeleteFlag]
                              ,[EnabledFlag]
                              ,[Description]
                              ,[CreaterTime]
                              ,[CreaterUserId]
                              ,[LastUpdateTime]
                              ,[LastUpdateUserId]
                              ,[DeleteTime]
                              ,[DeleteUserId]
                          FROM [dbo].[pm_Project]
                            where 1=1
                            and DeleteFlag<>1");
                if (!string.IsNullOrEmpty(keyValue))
                {
                    sb.AppendFormat(@" and ProjectId=@ProjectId");
                    utility.AddParameter("ProjectId", keyValue);
                }
                if (!string.IsNullOrEmpty(keyWrod))
                {
                    sb.AppendFormat(@" and ProjectName like @ProjectName");
                    utility.AddParameter("ProjectName", "%" + keyWrod + "%");
                }
                if (string.IsNullOrEmpty(Order))
                {
                    sb.AppendFormat(@" order by DisplayNo asc");
                }
                else
                {
                    sb.AppendFormat(@" order by DisplayNo @Order");
                    utility.AddParameter("@Order", Order);
                }
                try
                {
                    utility.ExecuteReaderModelList(sb.ToString(), result);
                    utility.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    utility.Rollback();
                    throw;
                }
            }
        }
        public bool AlterProject(PMProjectManagerModel projectData)
        {
            var sql = new StringBuilder();
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();
                sql.AppendFormat(@"
                                update [dbo].[pm_Project] set
                                ProjectCode=@ProjectCode,
                                ProjectName=@ProjectName,
                                ActualWork=@ActualWork,
                                DisplayNo=@DisplayNo,
                                EnabledFlag=@EnabledFlag,
                                Description=@Description,
                                LastUpdateTime=@LastUpdateTime,
                                LastUpdateUserId=@LastUpdateUserId
                                where ProjectId=@ProjectId");
                utility.AddParameter("ProjectCode", projectData.ProjectCode);
                utility.AddParameter("ProjectName", projectData.ProjectName);
                utility.AddParameter("ActualWork", projectData.ActualWork);
                utility.AddParameter("DisplayNo", projectData.DisplayNo);
                utility.AddParameter("EnabledFlag", projectData.EnabledFlag);
                utility.AddParameter("Description", projectData.Description);
                utility.AddParameter("LastUpdateTime", projectData.LastUpdateTime);
                utility.AddParameter("LastUpdateUserId", projectData.LastUpdateUserId);
                utility.AddParameter("ProjectId", projectData.ProjectId);
                var a = 0;
                try
                {
                    a = utility.ExecuteNonQuery(sql.ToString());
                    utility.Commit();
                }
                catch (Exception)
                {
                    utility.Rollback();
                    throw;
                }
                return a == 1;
            }
        }
        public bool AddProject(PMProjectManagerModel projectData)
        {
            var sql = new StringBuilder();
            if (projectData.DisplayNo == 0)
            {
                var sb = new StringBuilder();
                using (var utility = DbUtility.GetInstance())
                {
                    var NumModel = new PMProjectManagerModel();
                    sb.AppendFormat(@"select Max(DisplayNo) as DisplayNo from [dbo].[pm_Project]");
                    utility.ExecuteReaderModel(sb.ToString(), NumModel);
                    projectData.DisplayNo = NumModel.DisplayNo + 1;
                }
            }
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();
                sql.AppendFormat(@"insert into [dbo].[pm_Project] 
                                    (ProjectId,
                                    ProjectCode,
                                    ProjectName,
                                    ActualWork,
                                    DisplayNo,
                                    DeleteFlag,
                                    EnabledFlag,
                                    Description,
                                    CreaterTime,
                                    CreaterUserId)
                                    values
                                    (@ProjectId,
                                    @ProjectCode,
                                    @ProjectName,
                                    @ActualWork,
                                    @DisplayNo,
                                    @DeleteFlag,
                                    @EnabledFlag,
                                    @Description,
                                    @CreaterTime,
                                    @CreaterUserId)");
                var a = 0;
                try
                {
                    a = utility.ExecuteNonQuery(sql.ToString(), projectData);
                    utility.Commit();
                }
                catch (Exception e)
                {
                    utility.Rollback();
                    throw;
                }
                return a == 1;
            }
        }
        public bool DeleteProject(PMProjectManagerModel projectData)
        {
            var sql = new StringBuilder();
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();
                sql.AppendFormat(@"
                                update [dbo].[pm_Project] set
                                DeleteFlag=1,DeleteTime=@DeleteTime,DeleteUserId=@DeleteUserId
                                where ProjectId=@ProjectId");
                utility.AddParameter("DeleteTime", projectData.DeleteTime);
                utility.AddParameter("DeleteUserId", projectData.DeleteUserId);
                utility.AddParameter("ProjectId", projectData.ProjectId);
                var a = 0;
                try
                {
                    a = utility.ExecuteNonQuery(sql.ToString());
                    utility.Commit();
                }
                catch (Exception e)
                {
                    utility.Rollback();
                    throw;
                }
                return a == 1;
            }
        }
    }
}
