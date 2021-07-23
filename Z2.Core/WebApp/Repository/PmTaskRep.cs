using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.Operator;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using static Z2.Core.WebApp.Enums.TaskEnum;

namespace Z2.Core.WebApp.Repository
{
    public partial class PmTaskRep
    {
        //2019/1/31  增加一个字段   AssignTypeId 来选择类型查询
        public IEnumerable<Mst_TaskModel> GetList(string searchtxt, string ProjectId, string userId, string AssignTypeId, bool showAllUser, bool showClosed)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.AppendLine(@"
                   SELECT 
                       max(tk.[TaskId]) as [TaskId]
                      ,tk.[LinkTaskId] as  [LinkTaskId]
                      ,tk.[TeamId]
                      ,tk.[ProjectId]
                      ,pm_Project.ProjectName as [ProjectName]
                      ,tk.[TaskName]
                      ,tk.[LoopCount]
                      ,tk.[TaskPriority]
                      ,tk.[TaskCategory]
                      ,tk.[TaskStatus]
                      ,tk.[TaskStatusTime]
                      ,tk.[TaskPercent]
                      ,tk.[StartDate]
		              ,tk.[DueDate] as [DueDate]
                      ,tk.[ActualHours]
                      ,tk.[ParentId]
                  FROM [dbo].[pm_Task] tk
                  left join pm_Project on tk.ProjectId=pm_Project.ProjectId
                  left join pm_TaskAssign pta on tk.TaskId = pta.[TaskId]
                  Where tk.DeleteFlag=0  ");

                try
                {
                    if (!string.IsNullOrEmpty(ProjectId))
                    {
                        db.AddParameter("ProjectId", ProjectId);
                        sql.AppendLine(@" and tk.ProjectId = @ProjectId ");
                    }

                    if (!string.IsNullOrEmpty(searchtxt))
                    {
                        db.AddParameter("searchtxt", $"%{searchtxt}%");
                        sql.AppendLine(@" and (tk.TaskName like @searchtxt"); 
                        sql.AppendLine(@" or tk.[TaskDescription] like @searchtxt ");
                        sql.AppendLine(@" or tk.LinkTaskId like @searchtxt )");
                        
                    }

                    if (!string.IsNullOrEmpty(userId))
                    {
                        db.AddParameter("UserId", userId);
                        //sql.AppendLine(@" and ( (tk.DevUserId =@UserId ) or (tk.TestUserId =@UserId ) or (tk.ReviewUserId =@UserId ) )");
                        //sql.AppendLine(@" and tk.StartDate is not null ");
                        sql.AppendLine(@"
                            and pta.[AssignUserId] = @UserId
                            and pta.[StartDate] is not null
                        ");
                    }

                    if (!string.IsNullOrEmpty(AssignTypeId))
                    {
                        db.AddParameter("AssignTypeId", AssignTypeId);
                        sql.AppendLine(@" and pta.AssignTypeId = @AssignTypeId ");
                    }

                    if (showClosed)
                    {
                        db.AddParameter("TaskStatus", Convert.ToInt16(TaskStatusEnum.Close));
                        sql.AppendLine(@" and tk.[TaskStatus]=@TaskStatus");
                    }
                    else
                    {
                        db.AddParameter("TaskStatus", Convert.ToInt16(TaskStatusEnum.Close));
                        sql.AppendLine(@" and tk.[TaskStatus]<@TaskStatus");
                    }
                    sql.AppendLine(@"
                         group by  tk.[LinkTaskId]
                            , tk.[TeamId]
                            , tk.[ProjectId]
                            , pm_Project.ProjectName
                            , tk.[TaskName]
                            , tk.[LoopCount]
                            , tk.[TaskPriority]
                            , tk.[TaskCategory]
                            , tk.[TaskStatus]
                            , tk.[TaskStatusTime]
                            , tk.[TaskPercent]
                            , tk.[StartDate]
                            , tk.[DueDate]
                            , tk.[ActualHours]
                            , tk.[ParentId]
                            , tk.CreateDateTime

                        order by tk.[TaskPriority] desc, tk.CreateDateTime asc 
                    ");
                    var rst = db.ReaderModelList<Mst_TaskModel>(sql.ToString());
                    return rst;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<Mst_Assign> GetAssignList(string TaskId)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.AppendLine(@"
                    SELECT pta.[TaskAssignId]
                        ,pta.[TaskId]
                        ,pta.[AssignTypeId]
                        ,s.ItemName as AssignTypeName
                        ,pta.[AssignUserId]
                        ,pta.[EstimatedHours]
                        ,pta.[StartDate]
                        ,pta.[DueDate]
                        ,pta.[AssignPercent]
                        ,pta.[AssignOrder]
                        ,pta.[AssignDescription]
                        ,pta.[CreateUserId]
                        ,pta.[CreateDateTime]
                        ,pta.[UpdateUserId]
                        ,pta.[UpdateDateTime]
                        ,pta.[DeleteFlag]
                        ,pta.[DeleteTime]
                        ,pta.[DeleteUserId]
	                    ,u.RealName as AssignUserName
                    FROM [dbo].[pm_TaskAssign] pta
                    left join [dbo].[SysUser] u on u.Id = pta.[AssignUserId]
                    left join sysitem s on s.Id = pta.AssignTypeId 
                    Where pta.[DeleteFlag]=0
                    ");

                if (!string.IsNullOrEmpty(TaskId))
                {
                    db.AddParameter("TaskId", TaskId);
                    sql.AppendLine(@" and pta.[TaskId] = @TaskId");
                }

                sql.AppendLine(" order by pta.AssignOrder asc");

                try
                {
                    var rst = db.ReaderModelList<Mst_Assign>(sql.ToString());
                    //rst.Insert(0, new Mst_Assign {TaskId = TaskId, TaskAssignId = "", AssignTypeName = "未指定" });
                    return rst.ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public IEnumerable<Mst_Assign> GetAssignDataList(string TaskId)
        {
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.AppendLine(@"
                    with agc as (
	                    SELECT si.[Id] as [AssignTypeId]
		                      ,si.[ItemName] as [AssignTypeName]
	                    FROM [dbo].[SysItem] si
	                    inner join [dbo].[SysItemCategory] sic on sic.Id=si.ItemCategoryId
	                    where sic.CategoryCode='AssignType' and si.DeleteFlag=0
                    )
                    SELECT ta.[TaskAssignId]
                        ,ta.[TaskId]
                        ,ta.[AssignTypeId]
						,agc.[AssignTypeName]
                        ,ta.[AssignUserId]
						,su.RealName as [AssignUserName]
                        ,ta.[EstimatedHours]
                        ,ta.[StartDate]
                        ,ta.[DueDate]
                        ,ta.[AssignPercent]
                        ,ta.[AssignOrder]
                        ,ta.[AssignDescription]
                        ,ta.[CreateUserId]
                        ,ta.[CreateDateTime]
                        ,ta.[UpdateUserId]
                        ,ta.[UpdateDateTime]
                        ,ta.[DeleteFlag]
                        ,ta.[DeleteTime]
                        ,ta.[DeleteUserId]
                    FROM [dbo].[pm_TaskAssign] ta
					left join SysUser su on su.Id=[AssignUserId]
					left join agc on agc.[AssignTypeId]=ta.AssignTypeId
                    where ta.[TaskId]=@TaskId and ta.[DeleteFlag]=0");

                sql.AppendLine(@" order by ta.AssignOrder");

                try
                {
                    utility.AddParameter("TaskId", TaskId);
                    var rst = utility.ReaderModelList<Mst_Assign>(sql.ToString());
                    return rst;
                }
                catch (Exception e)
                {
                    utility.Rollback();
                    throw e;
                }
            }
        }
        public IEnumerable<Mst_Assign> GeTasktList(string searchtxt, string ProjectId, string userId, string AssignTypeId, bool showClosed)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();

                sql.AppendLine(@"

                    SELECT task.TaskId
                          ,task.ProjectId
                          ,task.LinkTaskId
	                      ,task.TaskName
                          ,task.TaskStatus
	                      ,proj.ProjectName
	                      ,taskAssign.AssignUserId
	                      ,taskAssign.AssignTypeId
	                      ,taskAssign.StartDate
	                      ,taskAssign.DueDate
	                      ,taskAssign.EstimatedHours
	                      ,taskAssign.AssignPercent
	                      ,assignUser.RealName as AssignUserName
	                      ,assignType.ItemCode as AssignTypeCode
	                      ,assignType.ItemName as AssignTypeName
                          ,taskAssign.pm_IsRemind
                      FROM [dbo].[pm_Task] task
                      left join [dbo].[pm_Project] proj on task.ProjectId = proj.ProjectId
                      left join [dbo].[pm_TaskAssign] taskAssign on task.[TaskId] = taskAssign.[TaskId]
                      left join [dbo].[SysUser] assignUser on taskAssign.AssignUserId = assignUser.Id
                      left join [dbo].[SysItem] assignType on assignType.Id = taskAssign.AssignTypeId

                     

                      where task.DeleteFlag=0 and taskAssign.DeleteFlag=0 ");

                if (!string.IsNullOrEmpty(ProjectId))
                {
                    db.AddParameter("ProjectId", ProjectId);
                    sql.AppendLine(@" and task.ProjectId = @ProjectId");
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    db.AddParameter("userId", userId);
                    sql.AppendLine(@" and taskAssign.[AssignUserId] = @userId");
                }

                db.AddParameter("TaskStatus", Convert.ToInt16(TaskStatusEnum.Close));
                sql.AppendLine(showClosed
                    ? @" and task.TaskStatus = @TaskStatus"
                    : @" and task.TaskStatus < @TaskStatus");

                if (!string.IsNullOrEmpty(AssignTypeId))
                {
                    db.AddParameter("AssignTypeId", AssignTypeId);
                    sql.AppendLine(@" and taskAssign.AssignTypeId = @AssignTypeId");
                }
                if (!string.IsNullOrEmpty(searchtxt))
                {
                    sql.AppendLine(@" and (proj.ProjectName like '%" + searchtxt + "%'");//项目号
                    sql.AppendLine(@"or task.TaskName like '%" + searchtxt + "%'");   //任务名
                    sql.AppendLine(@" or task.LinkTaskId like '%" + searchtxt + "%')");//任务号
                }

                sql.AppendLine(@" order by taskAssign.StartDate ,task.CreateDateTime,taskAssign.AssignOrder");

                try
                {
                    var rst = db.ReaderModelList<Mst_Assign>(sql.ToString());
                    return rst;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public Mst_TaskModel GetTaskData(string TaskID)
        {
            var model = new Mst_TaskModel();
            using (var utility = DbUtility.GetInstance())
            {
                var sb = new StringBuilder();
                sb.AppendFormat(@"
                    with asg as(
                        SELECT TOP 1 
                        [TaskAssignId]
                        ,s.[ItemName] as AssignTypeName
                        ,s.ItemCode as  AssignTypeCode
                        ,[AssignTypeId]
                        --,[AssignPercent] as TaskPercent
                        ,[TaskId] as asgTaskId
                        FROM [dbo].[pm_TaskAssign]
                        left join [dbo].[SysItem] s on s.Id = [AssignTypeId]
                        where [TaskId] = @TaskId and [pm_TaskAssign].DeleteFlag=0
                        order by [AssignOrder] asc) 
 
                    select
                        pm_Task.[TaskId],
                        pm_Task.[TaskName],
                        pm_Task.[LinkTaskId],
                        pm_Task.[ProjectId],
                        pm_Project.ProjectName as [ProjectName],
                        pm_Task.[LoopCount],
                        pm_Task.StartDate,
                        pm_Task.DueDate,
                        pm_Task.EstimatedHours,
                        pm_Task.[ActualHours],
                        pm_Task.DevEstimatedHours,
                        pm_Task.ReviewEstimatedHours,
                        pm_Task.TestEstimatedHours,
                        pm_Task.TaskStatus,
                        pm_Task.TaskDescription,        
                        pm_Task.[TaskPercent],        
                        pm_Task.[TaskPriority],  
                        pm_Task.[TaskCategory],
                        pm_Task.[ReviewUserId],
                        --(SELECT TOP 1 [TaskAssignId]
                        --FROM [dbo].[pm_TaskAssign]
                        --where [TaskId] = @TaskId
                        --order by [AssignOrder] asc) as TaskAssignId
                        asg.*
                    from [dbo].[pm_Task]
                    left join pm_Project on pm_Task.ProjectId=pm_Project.ProjectId
                    left join asg on asg.asgTaskId = pm_Task.TaskId
                    where pm_Task.TaskId=@TaskId");
                utility.AddParameter("TaskId", TaskID);
                try
                {
                    utility.ExecuteReaderModel(sb.ToString(), model);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return model;
        }
        public List<pm_TaskFile> GetFileData(string TaskId, string TaskDetail)
        {
            var model = new List<pm_TaskFile>();
            using (var utility = DbUtility.GetInstance())
            {
                var sb = new StringBuilder();
                sb.Append(@"select
                                   [pm_TaskFile].[Id]
                                  ,[pm_TaskFile].[TaskId]
                                  ,[pm_TaskFile].[TaskDetailId]
                                  ,[pm_TaskFile].[FileName]
                                  ,[pm_TaskFile].[FilePath]
                                  ,[pm_TaskFile].[CreateUserId]
                                  ,SysUser.RealName as [CreateUserName]
                                  ,[pm_TaskFile].[CreateDateTime]
                                FROM [dbo].[pm_TaskFile]
                                left join SysUser on pm_TaskFile.CreateUserId=SysUser.Id
                                where   [pm_TaskFile].DeleteFlag=0
                                and     [pm_TaskFile].TaskId=@TaskId");
                utility.AddParameter("TaskId", TaskId);
                if (!string.IsNullOrEmpty(TaskDetail))
                {
                    sb.Append(@"
                                and     [pm_TaskFile].TaskDetailId=@TaskDetail");
                    utility.AddParameter("TaskDetail", TaskDetail);
                }
                try
                {
                    utility.ExecuteReaderModelList(sb.ToString(), model);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return model;
        }
        public List<pm_TaskDetail> GetFileDetail(string TaskId, string TaskDetailId,DateTime? startDate,DateTime? endDate)
        {
            var model = new List<pm_TaskDetail>();
            using (var utility = DbUtility.GetInstance())
            {
                var sb = new StringBuilder();
                sb.AppendFormat(@"
                    select 
                        tkd.[TaskDetailId]
                        ,tkd.[TaskId]
                        ,tkd.[LoopCount]
                        ,tkd.[TaskStatus]
                        ,tkd.[TaskPercent]
                        ,tkd.[ProcessType]
                        ,tkd.[Date]
                        ,tkd.[StartDate]
                        ,tkd.[EndDate]
                        ,tkd.[ActualHours]
                        ,tkd.[TaskContent]
                        ,tkd.[ProcessResult]
                        ,tkd.[CreateUserId]
                        ,CreateUser.[RealName] as [CreateUserName]
                        ,tkd.[CreateDateTime]
                        ,tkd.[UpdateUserId]
                        ,UpdateUser.[RealName] as [UpdateUserName]
                        ,tkd.[UpdateDateTime]
                        ,[pm_TaskAssign].AssignTypeId
	                    ,SysItem.ItemName as AssignTypeName
                    FROM [dbo].[pm_TaskDetail] tkd
                    left join [dbo].[SysUser] as CreateUser on tkd.CreateUserId=CreateUser.Id
                    left join [dbo].[SysUser] as UpdateUser on tkd.UpdateUserId=UpdateUser.Id
                    left join [dbo].[pm_TaskAssign] on [pm_TaskAssign].TaskAssignId = tkd.[TaskAssignId]
                    left join [dbo].[SysItem] on SysItem.Id = [pm_TaskAssign].AssignTypeId
                    where tkd.TaskId=@TaskId
                    ");

                utility.AddParameter("TaskId", TaskId);

                if (!string.IsNullOrEmpty(TaskDetailId))
                {
                    sb.AppendFormat(@" and TaskDetailId=@TaskDetailId");
                    utility.AddParameter("TaskDetailId", TaskDetailId);
                }
                if (startDate.HasValue)
                {
                    sb.AppendFormat(@" and tkd.[Date]>=@startDate");
                    utility.AddParameter("startDate", startDate);
                }
                if (endDate.HasValue)
                {
                    sb.AppendFormat(@" and tkd.[Date]<@endDate");
                    utility.AddParameter("endDate", endDate);
                }
                sb.AppendFormat(@" order by CreateDateTime desc");
                try
                {
                    utility.ExecuteReaderModelList(sb.ToString(), model);
                }
                catch (Exception e)
                {

                }
            }
            return model;
        }

        public pm_TaskFile GetTaskFile(string Id)
        {
            var model = new pm_TaskFile();
            using (var utility = DbUtility.GetInstance())
            {
                var sb = new StringBuilder();
                sb.AppendFormat(@"select
                                   tkf.[Id]
                                  ,tkf.[TaskId]
                                  ,tkf.[TaskDetailId]
                                  ,tkf.[FileName]
                                  ,tkf.[FilePath]
                                FROM [dbo].[pm_TaskFile] tkf
                                where tkf.Id=@Id
                                and tkf.DeleteFlag=0 ");
                utility.AddParameter("Id", Id);
                try
                {
                    utility.ExecuteReaderModel(sb.ToString(), model);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return model;
        }
        public bool DeleteFile(string FileID, string UserRole, string CreateUserID)
        {
            var a = 0;
            using (var utility = DbUtility.GetInstance())
            {
                var sb = new StringBuilder();
                sb.AppendFormat(@"update [dbo].[pm_TaskFile] set DeleteFlag=1
                                    where 1=1");
                sb.AppendFormat(@" and Id=@FileID");
                sb.AppendFormat(@" and CreateUserID=@CreateUserID");
                utility.AddParameter("FileID", FileID);
                utility.AddParameter("CreateUserID", CreateUserID);
                try
                {
                    a = utility.ExecuteNonQuery(sb.ToString());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return a == 1;
        }
        public bool CreateTask(Mst_TaskModel task)
        {
            var bl = false;
            using (var db = DbUtility.GetInstance())
            {

                db.BeginTransaction();
                var sql = @"
                INSERT INTO [dbo].[pm_Task]
                   ([TaskId]
                   ,[LinkTaskId]
                   ,[TeamId]
                   ,[ProjectId]
                   ,[TaskName]
                   ,[LoopCount]
                   ,[TaskPriority]
                   ,[TaskCategory]
                   ,[TaskStatus]
                   ,[TaskStatusTime]
                   ,[TaskDescription]
                   ,[TaskPercent]
                   ,[StartDate]
                   ,[DueDate]
                   ,[EstimatedHours]
                   ,[ActualHours]
                   ,[ParentId]
                   ,[DevUserId]
                   ,[TestUserId]
                   ,[CreateUserId]
                   ,[CreateDateTime]
                    ,[ReviewUserId]
                    ,[DevEstimatedHours]
                    ,[ReviewEstimatedHours]
                    ,[TestEstimatedHours]
                   )
             VALUES
                   (@TaskId 
                   ,@LinkTaskId 
                   ,@TeamId 
                   ,@ProjectId 
                   ,@TaskName 
                   ,@LoopCount 
                   ,@TaskPriority
                   ,@TaskCategory
                   ,@TaskStatus 
                   ,@TaskStatusTime 
                   ,@TaskDescription 
                   ,@TaskPercent
                   ,@StartDate
		           ,@DueDate 
                   ,@EstimatedHours 
                   ,@ActualHours 
                   ,@ParentId 
                   ,@DevUserId 
                   ,@TestUserId 
                   ,@CreateUserId 
                   ,GETDATE()
                    ,@ReviewUserId
                    ,@DevEstimatedHours
                    ,@ReviewEstimatedHours
                    ,@TestEstimatedHours)
               ";
                db.AddParameter("TaskId             ", task.TaskId);
                db.AddParameter("LinkTaskId         ", task.LinkTaskId);
                db.AddParameter("TeamId             ", task.TeamId);
                db.AddParameter("ProjectId          ", task.ProjectId);
                db.AddParameter("TaskName           ", task.TaskName);
                db.AddParameter("LoopCount          ", task.LoopCount);
                db.AddParameter("TaskPriority       ", task.TaskPriority);
                db.AddParameter("TaskCategory       ", task.TaskCategory);
                db.AddParameter("TaskStatus         ", task.TaskStatus);
                db.AddParameter("TaskStatusTime     ", task.TaskStatusTime);
                db.AddParameter("TaskDescription    ", task.TaskDescription);
                db.AddParameter("TaskPercent        ", task.TaskPercent);
                db.AddParameter("StartDate          ", task.StartDate);
                db.AddParameter("DueDate            ", task.DueDate);
                db.AddParameter("EstimatedHours     ", task.EstimatedHours);
                db.AddParameter("ActualHours        ", task.ActualHours);
                db.AddParameter("ParentId           ", task.ParentId);
                db.AddParameter("DevUserId          ", task.DevUserId);
                db.AddParameter("TestUserId         ", task.TestUserId);
                db.AddParameter("CreateUserId       ", task.CreateUserId);
                db.AddParameter("ReviewUserId", task.ReviewUserId);
                db.AddParameter("DevEstimatedHours", task.DevEstimatedHours);
                db.AddParameter("ReviewEstimatedHours", task.ReviewEstimatedHours);
                db.AddParameter("TestEstimatedHours", task.TestEstimatedHours);

                try
                {
                    var r = db.ExecuteNonQuery(sql);
                    CreateTaskDetail(db, task.TaskDetails[0]);
                    if (task.Assigns != null && task.Assigns.Count > 0)
                    {
                        CreateAssign(db, task.Assigns);
                    }
                    if (task.TaskFiles != null && task.TaskFiles.Count > 0)
                    {
                        CreateTaskFile(db, task.TaskFiles[0]);
                    }
                    bl = r == 1;
                    db.Commit();
                }
                catch (Exception e)
                {
                    db.Rollback();
                    throw e;
                }
            }
            return bl;
        }

        public void CreateAssign(DbUtility utility, List<Mst_Assign> assignLst)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"
                insert into [dbo].[pm_TaskAssign]
                (
                     [TaskAssignId]
                    ,[TaskId]
                    ,[AssignTypeId]
                    ,[AssignUserId]
                    ,[EstimatedHours]
                    ,[StartDate]
                    ,[DueDate]
                    ,[AssignOrder]
                    ,[AssignDescription]
                    ,[CreateUserId]
                    ,[CreateDateTime]
                    ,[UpdateUserId]
                    ,[UpdateDateTime]
                    ,[DeleteFlag]
                )
                values
                (
                     newid()
                    ,@TaskId
                    ,@AssignTypeId
                    ,@AssignUserId
                    ,@EstimatedHours
                    ,@StartDate
                    ,@DueDate
                    ,@AssignOrder
                    ,@AssignDescription
                    ,@CreateUserId
                    ,@CreateDateTime
                    ,@CreateUserId
                    ,@CreateDateTime
                    ,0 -- DeleteFlag
                        )");
            foreach (var assign in assignLst)
            {
                utility.ExecuteNonQuery(sb.ToString(), assign);
            }
        }

        public void EditAssign(DbUtility utility, List<Mst_Assign> assignDatas)
        {
            var sql1 = @"
                    update [dbo].[pm_TaskAssign] set
                        [TaskId]=@TaskId,
                        [AssignTypeId]=@AssignTypeId,
                        [AssignUserId]=@AssignUserId,
                        [EstimatedHours]=@EstimatedHours,
                        [StartDate]=@StartDate,
                        [DueDate]=@DueDate,
                        [AssignOrder]=@AssignOrder,
                        [AssignDescription]=@AssignDescription,
                        [UpdateUserId]=@UpdateUserId,
                        [UpdateDateTime]=@UpdateDateTime
                    where [TaskAssignId]=@TaskAssignId";

            var sql2 = @"
                    update [dbo].[pm_TaskAssign] set
                        [DeleteFlag]=1,
                        [DeleteUserId]=@UpdateUserId,
                        [DeleteTime]=@UpdateDateTime
                    where [TaskAssignId]=@TaskAssignId";

            foreach (var assignData in assignDatas)
            {
                if (!assignData.DeleteFlag)
                {
                    if (assignData.TaskAssignId.StartsWith("-N-"))
                    {
                        CreateAssign(utility, new List<Mst_Assign> { assignData });
                    }
                    else
                    {
                        utility.ExecuteNonQuery(sql1, assignData);
                    }
                }
                else
                {
                    utility.ExecuteNonQuery(sql2, assignData);
                }
            }
        }

        //public bool DeleteAssign(Mst_Assign assignData)
        //{
        //    var sql = new StringBuilder();
        //    var bl = false;
        //    using (var utiilty = DbUtility.GetInstance())
        //    {
        //        utiilty.BeginTransaction();
        //        sql.AppendFormat(@"update [dbo].[pm_TaskAssign] set
        //            [DeleteFlag]=1,[DeleteTime]=@DeleteTime,[DeleteUserId]=@DeleteUserId
        //            where TaskAssignId=@TaskAssignId");
        //        utiilty.AddParameter("DeleteTime", assignData.DeleteTime);
        //        utiilty.AddParameter("DeleteUserId", assignData.DeleteUserId);
        //        utiilty.AddParameter("TaskAssignId", assignData.TaskAssignId);
        //        try
        //        {
        //            bl = utiilty.ExecuteNonQuery(sql.ToString()) == 1;
        //            utiilty.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            utiilty.Rollback();
        //            throw;
        //        }
        //        return bl;
        //    }
        //}

        public void CreateTaskDetail(DbUtility db, pm_TaskDetail taskDetail)
        {
            var sql = @"
            INSERT INTO [dbo].[pm_TaskDetail]
                       ([TaskDetailId]
                       ,[TaskId]
                       ,[LoopCount]
                       ,[ProcessType]
                       ,[Date]
                       ,[StartDate]
                       ,[TaskContent]
                       ,[CreateUserId]
                       ,[CreateDateTime]
                      )
                 VALUES
                       (@TaskDetailId
                       ,@TaskId
                       ,0
                       ,1
                       ,GETDATE()
                       ,@StartDate
                       ,@TaskContent
                       ,@CreateUserId
                       ,@CreateDateTime)
            ";
            db.AddParameter("TaskDetailId", taskDetail.TaskDetailId);
            db.AddParameter("TaskId      ", taskDetail.TaskId);
            db.AddParameter("StartDate   ", taskDetail.StartDate);
            db.AddParameter("TaskContent ", taskDetail.TaskContent);
            db.AddParameter("CreateUserId", taskDetail.CreateUserId);
            db.AddParameter("CreateDateTime", taskDetail.CreateDateTime);
            db.ExecuteNonQuery(sql);
        }

        public void CreateTaskFile(DbUtility db, pm_TaskFile taskFile)
        {
            var sql = @"
            INSERT INTO [dbo].[pm_TaskFile]
                       ([Id]
                       ,[TaskId]
                       ,[TaskDetailId]
                       ,[FileName]
                       ,[FilePath]
                       ,[DeleteFlag]
                       ,[CreateUserId]
                       ,[CreateDateTime])
                 VALUES
                       (@Id
                       ,@TaskId
                       ,@TaskDetailId
                       ,@FileName
                       ,@FilePath
                       ,0
                       ,@CreateUserId
                       ,GETDATE()
            )";
            db.AddParameter("Id           ", taskFile.Id);
            db.AddParameter("TaskId       ", taskFile.TaskId);
            db.AddParameter("TaskDetailId ", taskFile.TaskDetailId);
            db.AddParameter("FileName     ", taskFile.FileName);
            db.AddParameter("FilePath     ", taskFile.FilePath);
            db.AddParameter("CreateUserId ", taskFile.CreateUserId);
            db.AddParameter("", null);
            db.ExecuteNonQuery(sql);
        }

        public static List<Mst_ProjectModel> getProjects()
        {

            var model = new List<Mst_ProjectModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sb = new StringBuilder();
                sb.AppendFormat(@"SELECT [ProjectId]
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
                  Where [DeleteFlag]=0
                ");
                try
                {
                    utility.ExecuteReaderModelList(sb.ToString(), model);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return model;
        }


        public bool UpdateTask(Mst_TaskModel task)
        {

            using (var db = DbUtility.GetInstance())
            {

                db.BeginTransaction();
                var sql = @"
                UPDATE [dbo].[pm_Task]
		                SET	 [LinkTaskId]=@LinkTaskId
			                ,[TeamId]=@TeamId
			                ,[ProjectId]=@ProjectId
			                ,[TaskName]=@TaskName
			                ,[TaskPriority]=@TaskPriority
			                ,[TaskCategory]=@TaskCategory
			                ,[TaskStatus]=@TaskStatus
			                ,[TaskStatusTime]=GETDATE()
			                ,[TaskDescription]=@TaskDescription
			                ,[TaskPercent]=@TaskPercent
			                ,[StartDate]=@StartDate
			                ,[DueDate]=@DueDate
			                ,[EstimatedHours]=@EstimatedHours
                            ,[DevEstimatedHours] = @DevEstimatedHours
							,[ReviewEstimatedHours] = @ReviewEstimatedHours
							,[TestEstimatedHours] = @TestEstimatedHours
			                ,[ActualHours]=(select isnull(sum([ActualHours]),0) from [dbo].[pm_TaskDetail] where [TaskId]=@TaskId)
			                ,[DevUserId]=@DevUserId
                            ,[ReviewUserId] = @ReviewUserId
			                ,[TestUserId]=@TestUserId
			                ,[UpdateUserId]=@UpdateUserId
			                ,[UpdateDateTime]=GETDATE()
                Where		[TaskId]=@TaskId
               ";
                db.AddParameter("TaskId             ", task.TaskId);
                db.AddParameter("LinkTaskId         ", task.LinkTaskId);
                db.AddParameter("TeamId             ", task.TeamId);
                db.AddParameter("ProjectId          ", task.ProjectId);
                db.AddParameter("TaskName           ", task.TaskName);
                db.AddParameter("LoopCount          ", task.LoopCount);
                db.AddParameter("TaskPriority       ", task.TaskPriority);
                db.AddParameter("TaskCategory       ", task.TaskCategory);
                db.AddParameter("TaskStatus         ", task.TaskStatus);
                db.AddParameter("TaskStatusTime     ", task.TaskStatusTime);
                db.AddParameter("TaskDescription    ", task.TaskDescription);
                db.AddParameter("TaskPercent        ", task.TaskPercent);
                db.AddParameter("StartDate          ", task.StartDate);
                db.AddParameter("DueDate            ", task.DueDate);
                db.AddParameter("EstimatedHours     ", task.EstimatedHours);
                db.AddParameter("DevEstimatedHours     ", task.DevEstimatedHours);
                db.AddParameter("ReviewEstimatedHours     ", task.ReviewEstimatedHours);
                db.AddParameter("TestEstimatedHours     ", task.TestEstimatedHours);
                db.AddParameter("DevUserId          ", task.DevUserId);
                db.AddParameter("ReviewUserId          ", task.ReviewUserId);
                db.AddParameter("TestUserId         ", task.TestUserId);
                db.AddParameter("UpdateUserId       ", task.UpdateUserId);
                try
                {
                    var r = db.ExecuteNonQuery(sql);
                    EditAssign(db, task.Assigns);
                    db.Commit();
                    return r == 1;
                }
                catch (Exception e)
                {
                    db.Rollback();
                    throw e;
                }
            }
        }

        public List<Mst_Assign> GetAssignTypeList()
        {
            var result = new List<Mst_Assign>();
            var sql = new StringBuilder();
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();
                sql.AppendFormat(@"
                    SELECT 
                         a.[ItemCode]
                        ,a.[ItemName]
                    FROM [dbo].[SysItem] a
                    left join [dbo].[SysItemCategory] b on a.ItemCategoryId = b.Id
                    where b.CategoryCode = @CategoryCode");
                try
                {
                    utility.AddParameter("CategoryCode", "AssignType");
                    utility.ExecuteReaderModelList(sql.ToString(), result);
                    utility.Commit();
                }
                catch (Exception)
                {
                    utility.Rollback();
                    throw;
                }
            }
            return result;
        }
        public void updateRemindTask(List<string> list)
        {
            var sql = @"
                UPDATE [dbo].[pm_TaskAssign]
                   SET 
                      [pm_IsRemind] = 2
                 WHERE TaskAssignId = @TaskAssignId
                ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {

                    db.BeginTransaction();
                    foreach (var AssignId in list)
                    {
                        db.AddParameter("TaskAssignId", AssignId);
                        db.ExecuteNonQuery(sql);
                    }
                    db.Commit();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public List<Mst_TaskModel> GetNeedRemindTaskListByCurrentUserId(string userId)
        {
            var sql = @"
                            SELECT [pm_Task].[TaskId]
                                  ,[pm_Task].[LinkTaskId]
                                  ,[pm_Task].[TeamId]
                                  ,[pm_Task].[ProjectId]
                                  ,[pm_Task].[TaskName]
                                  ,[pm_Task].[TaskCategory]
                                  ,[pm_Task].[LoopCount]
                                  ,[pm_Task].[TaskPriority]
                                  ,[pm_Task].[TaskStatus]
                                  ,[pm_Task].[TaskStatusTime]
                                  ,[pm_Task].[TaskDescription]
                                  ,[pm_Task].[TaskPercent]
                                  ,[pm_Task].[StartDate]
                                  ,[pm_Task].[DueDate]
                                  ,[pm_Task].[EstimatedHours]
                                  ,[pm_Task].[ActualHours]
                                  ,[pm_Task].[ParentId]
                                  ,[pm_Task].[DevUserId]
                                  ,[pm_Task].[DevEstimatedHours]
                                  ,[pm_Task].[ReviewUserId]
                                  ,[pm_Task].[ReviewEstimatedHours]
                                  ,[pm_Task].[TestUserId]
                                  ,[pm_Task].[TestEstimatedHours]
                                  ,[pm_Task].[CreateUserId]
                                  ,[pm_Task].[CreateDateTime]
                                  ,[pm_Task].[UpdateUserId]
                                  ,[pm_Task].[UpdateDateTime]
                                  ,[pm_Task].[DeleteFlag]
                                  ,[pm_Task].[DeleteTime]
                                  ,[pm_Task].[DeleteUserId]
                                  ,[pm_TaskAssign].TaskAssignId
                              FROM [dbo].[pm_Task]
                              inner join pm_TaskAssign on pm_TaskAssign.TaskId = pm_Task.TaskId
                              where [pm_Task].DeleteFlag=0 and pm_TaskAssign.DeleteFlag=0 and pm_IsRemind=1 and pm_TaskAssign.AssignUserId=@UserId";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var data = db.ReaderModelList<Mst_TaskModel>(sql, new { UserId = userId });
                    return data.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public Mst_Assign GetAssign(string AssignId)
        {
            var result = new Mst_Assign();
            if (string.IsNullOrEmpty(AssignId))
            {
                return result;
            }
            var sql = new StringBuilder();
            using (var utility = DbUtility.GetInstance())
            {
                sql.AppendFormat(@"
                    SELECT 
                       taskAssign.[TaskAssignId]
                      ,taskAssign.[TaskId]
                      ,taskAssign.[AssignTypeId]
                      ,taskAssign.[AssignUserId]
                      ,taskAssign.[EstimatedHours]
                      ,taskAssign.[StartDate]
                      ,taskAssign.[DueDate]
                      ,taskAssign.[AssignPercent]
                      ,taskAssign.[AssignOrder]
                      ,taskAssign.[AssignDescription]
                      ,taskAssign.[CreateUserId]
                      ,taskAssign.[CreateDateTime]
                      ,taskAssign.[UpdateUserId]
                      ,taskAssign.[UpdateDateTime]
                      ,taskAssign.[DeleteFlag]
                      ,taskAssign.[DeleteTime]
                      ,taskAssign.[DeleteUserId]
                      ,taskAssign.[pm_IsRemind]
	                  ,assignType.ItemName as AssignTypeName
                      ,assignType.ItemCode as AssignTypeCode
                      ,assignType.ExtendData as AssignTypeExtendData
                  FROM [dbo].[pm_TaskAssign] taskAssign
                  left join [dbo].[SysItem] assignType on taskAssign.AssignTypeId = assignType.Id
                  where taskAssign.TaskAssignId = @TaskAssignId
                ");
                try
                {
                    utility.AddParameter("TaskAssignId", AssignId);
                    utility.ExecuteReaderModel(sql.ToString(), result);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }


        /// <summary>
        /// 指派类型获取
        /// </summary>
        /// <returns></returns>
        public List<SysItem> GetTaskStatusType()
        {
            var res = new List<SysItem>();
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                                SELECT  SysItem.[Id]
                                      ,[ItemCategoryId]
                                      ,[ParentId]
                                      ,[ItemCode]
                                      ,[ItemName]
                                      ,[SimpleSpelling]
                                      ,[IsDefault]
                                      ,[Layers]
                                      ,[DisplayNo]
                                    
                                      ,[ExtendData]
                                      ,[Description]
                                      ,[CreaterTime]
                                      ,[CreaterUserId]
                                      ,[LastUpdateTime]
                                      ,[LastUpdateUserId]
                                      ,[DeleteTime]
                                      ,[DeleteUserId]
                                  FROM [dbo].[SysItem]
                                  inner join SysItemCategory
                                  on SysItemCategory.Id=SysItem.ItemCategoryId
                                  where SysItemCategory.CategoryCode=@CategoryCode
                                  and SysItem.DeleteFlag=0 
                            ";

                db.AddParameter("CategoryCode", "AssignType");
                try
                {

                    res = db.ReaderModelList<SysItem>(sql).ToList();
                }
                catch (Exception)
                {

                    throw;
                }

                return res;
            }
        }

        public List<RenWuMingXiModel> GetRenWuMingXiList(string projectId, string workerId, string stratDate, string endDate)
        {
            var list = new List<RenWuMingXiModel>();
            using (var db = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.AppendLine(@"
                    SELECT tk.[TaskId]
		                ,proj.ProjectName
		                ,tk.[LinkTaskId]
		                ,tk.[TaskName]
		                ,tka.AssignTypeId
		                ,sysItm.[ItemName] as AssignTypeName
		                ,tka.AssignDescription
		                ,tkd.[Date]
		                ,tkd.ActualHours
		                ,tka.AssignPercent
		                ,su.RealName
                   FROM [dbo].[pm_TaskDetail] tkd
                   inner join [dbo].[pm_Task] tk on tkd.TaskId=tk.TaskId
                   left join [dbo].[pm_TaskAssign] tka on tkd.TaskId=tka.TaskId and tkd.TaskAssignId=tka.TaskAssignId
                   left join [dbo].f_SysItem('AssignType') sysItm on sysItm.Id=tka.AssignTypeId
                   left join [dbo].[pm_Project] proj on proj.ProjectId=tk.ProjectId
                   left join [dbo].[SysUser] su on su.Id=tkd.CreateUserId
                   where tk.[ProjectId]=@ProjectId -- and sysItm.[ItemCode]='Close'
                   and tkd.[Date]>=@StartDate 
                   and tkd.[Date]<=@endDate 
                   and tkd.CreateUserId=@UserId	
order by tkd.[Date] desc
                ");

                try
                {
                    db.AddParameter("ProjectId ", projectId);
                    db.AddParameter("UserId ", workerId);
                    db.AddParameter("StartDate ", stratDate);
                    db.AddParameter("endDate ", endDate);
                    db.ExecuteReaderModelList(sql.ToString(), list);
                    //rst.Insert(0, new Mst_Assign {TaskId = TaskId, TaskAssignId = "", AssignTypeName = "未指定" });
                    return list;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}

