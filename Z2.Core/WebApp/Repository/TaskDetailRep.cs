using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.Operator;
using Z2.Core.WebApp.Model;
using static Z2.Core.WebApp.Enums.TaskEnum;

namespace Z2.Core.WebApp.Repository
{
    public class TaskDetailRep
    {

        public List<pm_TaskDetail> GetModelDetail(string keyValue)
        {
            const string sql = @"SELECT 
                                   [TaskDetailId]
                                  ,[TaskId]
                                  ,[LoopCount]
                                  ,[ProcessType]
                                  ,[Date]
                                  ,[StartDate]
                                  ,[EndDate]
                                  ,[ActualHours]
                                  ,[TaskContent]
                                  ,[ProcessResult]
                                  ,[CreateUserId]
                                  ,[CreateDateTime]
                                  ,[UpdateUserId]
                                  ,[UpdateDateTime]
                                FROM [dbo].[pm_TaskDetail]
                                Where TaskId=@TaskId
                                order by ProcessType  
                                ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ReaderModelList<pm_TaskDetail>(sql, new { TaskId = keyValue });
                    return res.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public List<SysUser> getUserList()
        {
            var userlist = new List<SysUser>();
            string sql = @"
                    SELECT
                         [Id]
                        ,[RealName]
                    FROM[dbo].[SysUser]
                    where [EnabledFlag]=1
                ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    db.ExecuteReaderModelList(sql, userlist);
                }
                catch (Exception)
                {

                    throw;
                }
                return userlist;
            }
        }

        public List<SysUser> getUserList2()
        {
            var userlist = new List<SysUser>();
            string sql = @"
                     SELECT
	                    [Id]
	                    ,[RealName]
	                    ,sum(0) as JiaDongLv
	
                    FROM[dbo].[SysUser] a
                    left join [dbo].[pm_TaskDetail] b on a.Id = b.CreateUserId
                    group by [Id]
	                    ,[RealName]
                ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    db.ExecuteReaderModelList(sql, userlist);
                }
                catch (Exception)
                {

                    throw;
                }
                return userlist;
            }
        }

        public List<Mst_TaskModel> getUserProject(DateTime searchDate)
        {
            var lists = new List<Mst_TaskModel>();
            string sql = @"

                   SELECT tsk.[TaskId]
                             ,tsk.[ProjectId]
                             ,tsk.[TaskName] 
	                         ,proj.ProjectName
	                         ,sum(isnull(tskDtl.ActualHours,0)) as ActualHours
	                       --,tskDtl.CreateUserId
                             FROM [dbo].[pm_Task] tsk
                             inner join pm_Project proj on tsk.ProjectId=proj.ProjectId
                             inner join [dbo].[pm_TaskDetail] tskDtl on tskDtl.TaskId=tsk.TaskId
                             where isnull(tsk.DeleteFlag,0)=0
                             and tskDtl.[Date]>=@Date1
                             and tskDtl.[Date]<@Date2
                             group by tsk.[TaskId],tsk.[ProjectId]
                       	  ,proj.ProjectName,tsk.[TaskName] 
                  having sum(isnull(tskDtl.ActualHours,0)) >0 
                  order by ProjectId
                ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    db.AddParameter("Date1", searchDate);
                    db.AddParameter("Date2", searchDate.AddMonths(1));
                    db.ExecuteReaderModelList(sql, lists);
                }
                catch (Exception)
                {

                    throw;
                }
                return lists;
            }
        }

        public List<pm_TaskDetail> GetRenWuTongJi(DateTime searchDate, string userId)
        {
            var list = new List<pm_TaskDetail>();
            var sql = @"
                select 
	            datetable.dangyueriqi
	            --,tbViewTmp.[CreateUserId] as userId
	            ,isnull(tbViewTmp.[ActualHours],0) as [ActualHours]
                from 
                (	select convert(date,dateadd(day,number,dateadd(d,-day(@StratRiQi)+1,@StratRiQi)),120)  as dangyueriqi
	                from
	                master..spt_values 
	                where 
	                number>=0  and number< [dbo].[DaysInMonth](@StratRiQi)
                and type='p'
                ) datetable
                left join (
                SELECT 
                max(a.[CreateUserId]) as [CreateUserId]
                ,c.RealName as RealName
                ,max(a.[Date])as [Date]
                ,sum(a.[ActualHours]) as [ActualHours]
                FROM 
                [dbo].[pm_TaskDetail] a
                left join [dbo].[pm_Task]  b on a.TaskId = b.TaskId
                left join sysuser c on c.Id = a.CreateUserId
                where a.Date >= @StratRiQi and a.Date < @EndRiQi
                and a.[CreateUserId] = @UserId

                group by a.[CreateUserId],a.[Date],c.RealName 

                ) tbViewTmp on datetable.dangyueriqi = tbViewTmp.[Date]
            ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    db.AddParameter("StratRiQi", searchDate);
                    db.AddParameter("EndRiQi", searchDate.AddMonths(1));
                    db.AddParameter("userId", userId);
                    //db.ReaderModelList<pm_TaskDetail>(sql,);
                    db.ExecuteReaderModelList(sql, list);
                }
                catch (Exception)
                {
                    throw;
                }
                return list;
            }
        }


        public List<pm_TaskDetail> GetUserDateTask(DateTime taskDate, string userId)
        {
            var list = new List<pm_TaskDetail>();
            var sql = @"
            SELECT tsk.[TaskId]
                  ,tsk.[ProjectId]
                  ,tsk.[TaskName] 
	              ,tskDtl.*
                  ,CreateUser.[RealName] as [CreateUserName]
                  ,UpdateUser.[RealName] as [UpdateUserName]
                  ,[pm_TaskAssign].AssignTypeId
	              ,SysItem.ItemName as AssignTypeName
              FROM [dbo].[pm_Task] tsk
              inner join [dbo].[pm_TaskDetail] tskDtl on tskDtl.TaskId=tsk.TaskId
              left join [dbo].[SysUser] as CreateUser on tskDtl.CreateUserId=CreateUser.Id
              left join [dbo].[SysUser] as UpdateUser on tskDtl.UpdateUserId=UpdateUser.Id
              left join [dbo].[pm_TaskAssign] on [pm_TaskAssign].TaskAssignId = tskDtl.[TaskAssignId]
              left join [dbo].[SysItem] on SysItem.Id = [pm_TaskAssign].AssignTypeId
              where isnull(tsk.DeleteFlag,0)=0
              and tskDtl.[Date]=@Date
              and tskDtl.CreateUserId=@UserId
              and tskDtl.ActualHours>0
            ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    db.AddParameter("Date", taskDate);
                    db.AddParameter("userId", userId);
                    //db.ReaderModelList<pm_TaskDetail>(sql,);
                    db.ExecuteReaderModelList(sql, list);
                }
                catch (Exception)
                {
                    throw;
                }
                return list;
            }
        }


        /// <summary>
        /// 根据TaskDetailId获取任务的详细信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public pm_TaskDetail GetModel(string TaskDetailId)
        {
            const string sql = @"
                SELECT top 1
                     ptd.[TaskDetailId]
                    ,ptd.[TaskId]
                    ,ptd.[LoopCount]
                    ,ptd.[TaskStatus]
                    --,ptd.[TaskPercent]
                    ,ptd.[ProcessType]
                    ,ptd.[Date]
                    ,ptd.[StartDate]
                    ,ptd.[EndDate]
                    ,ptd.[ActualHours]
                    ,ptd.[TaskContent]
                    ,ptd.[ProcessResult]
                    ,ptd.[CreateUserId]
                    ,ptd.[CreateDateTime]
                    ,ptd.[UpdateUserId]
                    ,ptd.[UpdateDateTime]
	                ,pta.[AssignPercent] as [TaskPercent]
	                ,pta.[TaskAssignId] 
                    ,pta.[AssignTypeId]
                    ,isnull(ptd.[CreateUserId],pta.[AssignUserId]) as [AssignUserId]
                FROM [dbo].[pm_TaskDetail] ptd
                left join [dbo].[pm_TaskAssign] pta on pta.[TaskAssignId] = ptd.[TaskAssignId]
                Where TaskDetailId=@TaskDetailId
            ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ReaderModel<pm_TaskDetail>(sql, new { TaskDetailId = TaskDetailId });
                    return res;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public List<pm_TaskFile> GetModelFile(string TaskId, string TaskDetailId)
        {
            var sql = @"Select 
                               [Id]
                              ,[TaskId]
                              ,[TaskDetailId]
                              ,[FileName]
                              ,[FilePath]
                              ,[DeleteFlag]
                              ,[CreateUserId]
                              ,[CreateDateTime]
                        From [dbo].[pm_TaskFile]
                        Where TaskId=@TaskId  and TaskDetailId=@TaskDetailId
                              and DeleteFlag=0
                        ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ReaderModelList<pm_TaskFile>(sql, new { TaskId = TaskId, TaskDetailId = TaskDetailId });
                    return res.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 提交    增加  或  修改（TaskDetailId=null  增加）
        /// </summary>
        /// <param name="pmTaskDetail"></param>
        /// <param name="TaskDetailId"></param>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public bool SubmitForm(pm_TaskDetail pmTaskDetail, string TaskDetailId, string TaskId, string UserId)
        {
            var assign = new PmTaskRep().GetAssign(pmTaskDetail.TaskAssignId);

            pmTaskDetail.UpdateUserId = UserId;
            pmTaskDetail.UpdateDateTime = DateTime.Now;
            pmTaskDetail.CreateUserId = UserId;
            pmTaskDetail.CreateDateTime = DateTime.Now;
            if (!string.IsNullOrEmpty(pmTaskDetail.AssignUserId))
            {
                pmTaskDetail.CreateUserId = pmTaskDetail.AssignUserId;
            }
            var sql = string.Empty;
            if (string.IsNullOrEmpty(pmTaskDetail.TaskDetailId))//增加
            {
                var taskModel = new PmTaskRep().GetTaskData(TaskId);

                //初始化数据
                pmTaskDetail.TaskId = TaskId;
                pmTaskDetail.TaskDetailId = Guid.NewGuid().ToString();
                //pmTaskDetail.CreateUserId = UserId;// OperatorProvider.Provider.GetCurrent().UserId;
                //pmTaskDetail.CreateDateTime = DateTime.Now;
                pmTaskDetail.LoopCount = taskModel.LoopCount;
                //pmTaskDetail.Date = DateTime.Now;
                //结束时间 = 开始时间 + 工时     
                //工时/8  得到天数的整数部分  
                //pmTaskDetail.EndDate = pmTaskDetail.StartDate.AddDays((int)pmTaskDetail.ActualHours / 8);
                sql = @"
                         Insert Into [dbo].[pm_TaskDetail]
                        (
                              [TaskDetailId]
                             ,[TaskId]
                             ,[TaskAssignId]
                             ,[LoopCount]
                             ,[TaskStatus]
                             ,[TaskPercent]
                             ,[ProcessType]
                             ,[Date]
                             ,[StartDate]
                             ,[EndDate]
                             ,[ActualHours]
                             ,[TaskContent]
                             ,[ProcessResult]
                             ,[CreateUserId]
                             ,[CreateDateTime]
                             ,[UpdateUserId]
                             ,[UpdateDateTime]
                            )
                            Values(
                              @TaskDetailId  
                             ,@TaskId  
                             ,@TaskAssignId      
                             ,@LoopCount  
                             ,@TaskStatus
                             ,@TaskPercent   
                             ,@ProcessType  
                             ,@Date         
                             ,@StartDate     
                             ,@EndDate      
                             ,@ActualHours   
                             ,@TaskContent   
                             ,@ProcessResult 
                             ,@CreateUserId 
                             ,@CreateDateTime
                             ,@UpdateUserId 
                             ,@UpdateDateTime 
                            )
                        ";
            }
            else
            {//修改
                sql = @" 
                    UPDATE [dbo].[pm_TaskDetail]
                       SET 
                          [TaskAssignId] = @TaskAssignId
                          ,[Date] = @Date
                          ,[TaskStatus] = @TaskStatus
                          ,[TaskPercent] = @TaskPercent
                          ,[ActualHours] = @ActualHours
                          ,[TaskContent] = @TaskContent
                          ,[ProcessResult] = @ProcessResult
                          ,[CreateUserId] = @CreateUserId
                          ,[UpdateUserId] = @UpdateUserId
                          ,[UpdateDateTime] = @UpdateDateTime
                     WHERE [TaskDetailId]= @TaskDetailId 
                                            ";
            }
            var sql2 = @"
                    UPDATE [dbo].[pm_Task]
                       SET    
                          [TaskPercent] = @TaskPercentAve
                          ,[TaskStatus] = @TaskStatus 
                          ,[UpdateUserId] = @UpdateUserId
                          ,[UpdateDateTime] = getdate()
                          ,[ActualHours]=(select isnull(sum([ActualHours]),0) from [dbo].[pm_TaskDetail] where [TaskId]=@TaskId)
                     WHERE [TaskId] = @TaskId
                    ";

            var sql3 = @"
                    UPDATE [dbo].[pm_Task]
                       SET [LoopCount]=[LoopCount]+1
                     WHERE [TaskId] = @TaskId
                    ";

            var sql4 = @"
                    UPDATE [dbo].[pm_TaskAssign]
                       SET 
                          [AssignPercent] = @TaskPercent
                          ,[UpdateUserId] = @UpdateUserId
                          ,[UpdateDateTime] = getdate()
    
                     WHERE [TaskAssignId] = @TaskAssignId
                ";

            //var sql5 = @"
            //    UPDATE [dbo].[pm_Task]
            //       SET [TaskPercent] = @TaskPercent
            //     WHERE [TaskId] = @TaskId
            //    ";

            var sql5 = @"
                    declare @nextTaskAssign varchar(50)
                    declare @AssignOrder int

                    select @AssignOrder = [AssignOrder] from [dbo].[pm_TaskAssign]
                    where [TaskAssignId] = @TaskAssignId

                    select top 1 @nextTaskAssign = [TaskAssignId]
                    from [dbo].[pm_TaskAssign]
                    where [TaskId] = @TaskId
                    and [AssignOrder] > @AssignOrder
                    order by [AssignOrder]

                    update [dbo].[pm_TaskAssign] set [pm_IsRemind] = 1
                    where [TaskAssignId] = @nextTaskAssign
                ";

            using (var db = DbUtility.GetInstance())
            {
                db.BeginTransaction();
                try
                {
                    var data = db.ExecuteNonQuery(sql, pmTaskDetail);
                    db.ExecuteNonQuery(sql2, pmTaskDetail);
                    //db.ExecuteNonQuery();
                    db.ExecuteNonQuery(sql4, pmTaskDetail);
                    if (pmTaskDetail.TaskPercent == 100)
                    {
                        db.ExecuteNonQuery(sql5, pmTaskDetail);
                    }
                    if (assign != null && assign.AssignTypeCode == TaskDetailProcessTypeEnum.Redo.ToString())
                    {
                        db.ExecuteNonQuery(sql3, pmTaskDetail);
                    }
                    db.Commit();
                    return data == 1;
                }
                catch (Exception)
                {
                    db.Rollback();
                    throw;
                }
            }
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="ptf"></param>
        /// <returns></returns>
        public pm_TaskFile UpFile(pm_TaskFile ptf)
        {
            ptf.Id = Guid.NewGuid().ToString();
            ptf.DeleteFlag = false;
            var sb = new StringBuilder();
            sb.AppendFormat(@"select [RealName] as CreateUserName from [SysUser] where Id=@CreateUserId");
            using (var utility = DbUtility.GetInstance())
            {
                try
                {
                    utility.AddParameter("CreateUserId", ptf.CreateUserId);
                    var model = new pm_TaskFile();
                    utility.ExecuteReaderModel(sb.ToString(), model);
                    ptf.CreateUserName = model.CreateUserName;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            var sql = @"Insert  into [pm_TaskFile]
                        (
                         [Id]             
                        ,[TaskId]         
                        ,[TaskDetailId]   
                        ,[FileName]       
                        ,[FilePath]       
                        ,[DeleteFlag]     
                        ,[CreateUserId]   
                        ,[CreateDateTime] 
                        )
                        Values(
                        @Id            
                       ,@TaskId        
                       ,@TaskDetailId  
                       ,@FileName      
                       ,@FilePath      
                       ,@DeleteFlag    
                       ,@CreateUserId  
                       ,@CreateDateTime
                        )
                        ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var res = db.ExecuteNonQuery(sql, ptf);
                    return ptf;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
