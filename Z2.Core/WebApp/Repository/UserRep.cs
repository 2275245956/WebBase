using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.Exceptions;
using Z2.Core.Security;
using Z2.Core.WebApp.Model;
using Z2.Cores.Common;

namespace Z2.Core.WebApp.Repository
{
    public class UserRep
    {
        public IEnumerable<SysUser> GetList(string keyword)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                  SELECT  [SysUser].[Id]
                  ,[SysUser].[Account]
                  ,[SysUser].[RealName]
                  ,[SysUser].[NickName]
                  ,[SysUser].[Avatar]
                  ,[SysUser].[Gender]
                  ,[SysUser].[Birthday]
                  ,[SysUser].[MobilePhone]
                  ,[SysUser].[Email]
                  ,[SysUser].[WeChat]
                  ,[SysUser].[ManagerId]
                  ,[SysUser].[SecurityLevel]
                  ,[SysUser].[Signature]
                  ,[SysUser].[OrganizeId]
                  ,[SysUser].[DepartmentId]
                  ,[SysUser].[RoleId]
                  ,[SysUser].[DutyId]
                  ,[SysUser].[IsAdministrator]
                  ,[SysUser].[DisplayNo]
                  ,[SysUser].[DeleteFlag]
                  ,[SysUser].[EnabledFlag]
                  ,[SysUser].[Description]
                  ,[SysUser].[CreaterTime]
                  ,[SysUser].[CreaterUserId]
                  ,[SysUser].[LastUpdateTime]
                  ,[SysUser].[LastUpdateUserId]
                  ,[SysUser].[DeleteTime]
                  ,[SysUser].[DeleteUserId]
				  ,(select [Name] from [SysOrganize] where [SysOrganize].Id=[SysUser].OrganizeId) as OrganizeName
				  ,(select [Name] from [SysOrganize] where [SysOrganize].Id=[SysUser].DepartmentId) as DepartmentName
                  ,(select [Name] from [SysRole] where [SysRole].Id=[SysUser].DutyId and [SysRole].[DeleteFlag]=0 and [Type]=2) as DutyName
                FROM [dbo].[SysUser]
                  where [DeleteFlag]=@DeleteFlag";
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql = sql + @" and [RealName] like @keyword";
                }
                sql = sql + @" order by [DisplayNo]";
                try
                {
                    var rst = db.ReaderModelList<SysUser>(sql, new { DeleteFlag = 0, EnabledFlag = true, keyword = "%" + keyword + "%" });
                    return rst;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public bool DeleteForm(string keyValue, string userId)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    update [dbo].[SysUser] 
                    set [DeleteFlag]=1
                    ,[DeleteTime]=@DeleteTime
                    ,[DeleteUserId]=@DeleteUserId
                    where [Id]=@keyValue";
                var bl = db.ExecuteNonQuery(sql, new { keyValue = keyValue, DeleteTime = DateTime.Now, DeleteUserId = userId });
                return bl == 1;
            }
        }
        public bool UpdateForm(SysUser userEntity, string userId)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    update [dbo].[SysUser] 
                    set [EnabledFlag]=@EnabledFlag
                    ,[LastUpdateTime]=@LastUpdateTime
                    ,[LastUpdateUserId]=@LastUpdateUserId
                    where [Id]=@keyValue";
                var bl = db.ExecuteNonQuery(sql, new { EnabledFlag = userEntity.EnabledFlag, LastUpdateTime = DateTime.Now, LastUpdateUserId = userId, keyValue = userEntity.Id });
                return bl == 1;
            }
        }
        public SysUser GetForm(string keyword)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                  SELECT  [Id]
                  ,[Account]
                  ,[RealName]
                  ,[NickName]
                  ,[Avatar]
                  ,[Gender]
                  ,[Birthday]
                  ,[MobilePhone]
                  ,[Email]
                  ,[WeChat]
                  ,[ManagerId]
                  ,[SecurityLevel]
                  ,[Signature]
                  ,[OrganizeId]
                  ,[DepartmentId]
                  ,[RoleId]
                  ,[DutyId]
                  ,[IsAdministrator]
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
                  FROM [dbo].[SysUser]
                  where [DeleteFlag]=@DeleteFlag
                  and [Id]=@Id
                  order by [DisplayNo]";
                try
                {
                    var rst = db.ReaderModel<SysUser>(sql, new { DeleteFlag = 0, EnabledFlag = true, Id = keyword });
                    return rst;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        public bool SubmitForm(SysUser roleEntity)
        {
            roleEntity.CreaterTime = DateTime.Now;
            roleEntity.LastUpdateTime = DateTime.Now;

            var userSecretkey = Md5Helper.md5(DESEncryptHelper.Encrypt(Common.CreateNo().ToLower()).ToLower(), 16).ToLower();
            UserLogOn uloMdl = new UserLogOn()
            {
                UserId = roleEntity.Id,
                UserPassword = Md5Helper.md5(DESEncryptHelper.Encrypt(roleEntity.Password.ToLower(), userSecretkey).ToLower(), 32).ToLower(),
                UserSecretkey = userSecretkey,
            };

            var sql = string.Empty;
            var sqlSysUserLogOn = string.Empty;
            var sqlSysRoleUser = string.Empty;
            if (string.IsNullOrEmpty(roleEntity.Id))
            {
                roleEntity.Id = Guid.NewGuid().ToString();
                uloMdl.UserId = roleEntity.Id;
                #region sql
                sql = @"
                    insert into [dbo].[SysUser]
                    ([Id]
                  ,[Account]
                  ,[RealName]
                  ,[NickName]
                  ,[Avatar]
                  ,[Gender]
                  ,[Birthday]
                  ,[MobilePhone]
                  ,[Email]
                  ,[WeChat]
                  ,[ManagerId]
                  ,[SecurityLevel]
                  ,[Signature]
                  ,[OrganizeId]
                  ,[DepartmentId]
                  ,[RoleId]
                  ,[DutyId]
                  ,[IsAdministrator]
                  ,[DisplayNo]
                  ,[DeleteFlag]
                  ,[EnabledFlag]
                  ,[Description]
                  ,[CreaterTime]
                  ,[CreaterUserId]
                  ,[LastUpdateTime]
                  ,[LastUpdateUserId]
                  ,[DeleteTime]
                  ,[DeleteUserId])
                    values
                    (
                    @Id
                    ,@Account
                    ,@RealName
                    ,@NickName
                    ,@Avatar
                    ,@Gender
                    ,@Birthday
                    ,@MobilePhone
                    ,@Email
                    ,@WeChat
                    ,@ManagerId
                    ,@SecurityLevel
                    ,@Signature
                    ,@OrganizeId
                    ,@DepartmentId
                    ,@RoleId
                    ,@DutyId
                    ,@IsAdministrator
                    ,@DisplayNo
                    ,@DeleteFlag
                    ,@EnabledFlag
                    ,@Description
                    ,@CreaterTime
                    ,@CreaterUserId
                    ,@LastUpdateTime
                    ,@LastUpdateUserId
                    ,@DeleteTime
                    ,@DeleteUserId)";
                #endregion

                #region sqlSysRoleUser
                sqlSysRoleUser = @"INSERT INTO [dbo].[SysRoleUser]
                                           ([Id]
                                           ,[RoleId]
                                           ,[UserId])
                                   VALUES
                                           (newid()
                                           ,@RoleId
                                           ,@Id)";
                #endregion

                #region sqlSysUserLogOn
                sqlSysUserLogOn = @"INSERT INTO [dbo].[SysUserLogOn]
                                           (id
                                           ,[UserId]
                                           ,[UserPassword]
                                           ,[UserSecretkey]
                                           ,[AllowStartTime]
                                           ,[AllowEndTime]
                                           ,[LockStartDate]
                                           ,[LockEndDate]
                                           ,[FirstVisitTime]
                                           ,[PreviousVisitTime]
                                           ,[LastVisitTime]
                                           ,[ChangePasswordDate]
                                           ,[MultiUserLogin]
                                           ,[LogOnCount]
                                           ,[UserOnLine]
                                           ,[Question]
                                           ,[AnswerQuestion]
                                           ,[CheckIPAddress]
                                           ,[Language]
                                           ,[Theme])
                                     VALUES
                                           (newid()
                                           ,@UserId
                                           ,@UserPassword
                                           ,@UserSecretkey
                                           ,@AllowStartTime
                                           ,@AllowEndTime
                                           ,@LockStartDate
                                           ,@LockEndDate
                                           ,@FirstVisitTime
                                           ,@PreviousVisitTime
                                           ,@LastVisitTime
                                           ,@ChangePasswordDate
                                           ,@MultiUserLogin
                                           ,@LogOnCount
                                           ,@UserOnLine
                                           ,@Question
                                           ,@AnswerQuestion
                                           ,@CheckIPAddress
                                           ,@Language
                                           ,@Theme)";
                #endregion

            }
            else
            {
                #region sql
                sql = @"
                    update [dbo].[SysUser] set
                   [Account]=@Account
                  ,[RealName]=@RealName
                  ,[NickName]=@NickName
                  ,[Avatar]=@Avatar
                  ,[Gender]=@Gender
                  ,[Birthday]=@Birthday
                  ,[MobilePhone]=@MobilePhone
                  ,[Email]=@Email
                  ,[WeChat]=@WeChat
                  ,[ManagerId]=@ManagerId
                  ,[SecurityLevel]=@SecurityLevel
                  ,[Signature]=@Signature
                  ,[OrganizeId]=@OrganizeId
                  ,[DepartmentId]=@DepartmentId
                  ,[RoleId]=@RoleId
                  ,[DutyId]=@DutyId
                  ,[IsAdministrator]=@IsAdministrator
                  ,[DisplayNo]=@DisplayNo
                  ,[DeleteFlag]=@DeleteFlag
                  ,[EnabledFlag]=@EnabledFlag
                  ,[Description]=@Description
                  ,[CreaterTime]=@CreaterTime
                  ,[CreaterUserId]=@CreaterUserId
                  ,[LastUpdateTime]=@LastUpdateTime
                  ,[LastUpdateUserId]=@LastUpdateUserId
                  ,[DeleteTime]=@DeleteTime
                  ,[DeleteUserId]=@DeleteUserId
                  where [Id]=@Id";
                #endregion

                #region sqlSysRoleUser
                sqlSysRoleUser = @"UPDATE [dbo].[SysRoleUser]
                                       SET
                                          [RoleId] = @RoleId
                                     WHERE [UserId] = @Id";
                #endregion

                #region sqlSysUserLogOn
                sqlSysUserLogOn = @"";
                #endregion

            }
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var effRow = db.ExecuteNonQuery(sql, roleEntity);
                    if (effRow > 0 && !string.IsNullOrEmpty(sqlSysRoleUser))
                    {
                        var roleRow = db.ExecuteNonQuery(sqlSysRoleUser, roleEntity);
                        if (roleRow > 0 && !string.IsNullOrEmpty(sqlSysUserLogOn))
                        {
                            var logonRow = db.ExecuteNonQuery(sqlSysUserLogOn, uloMdl);
                        }
                    }
                    return effRow == 1;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 获取用户已经分配的角色
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<SysRoleUser> GerRoleIdListByUserId(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = " select  RoleId  from SysRoleUser where UserId=@UserId";

                try
                {
                    var data = db.ReaderModelList<SysRoleUser>(sql, new { UserId = keyValue });
                    return data.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }



        public bool RevisePassword(string userPassword, string keyValue)
        {
            var userSecretkey = Md5Helper.md5(DESEncryptHelper.Encrypt(Common.CreateNo().ToLower()).ToLower(), 16).ToLower();

            userPassword = Md5Helper.md5(DESEncryptHelper.Encrypt(userPassword.ToLower(), userSecretkey).ToLower(), 32).ToLower();
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                    update [dbo].[SysUserLogOn] set
                   
                     [UserPassword]=@userPassword
                    ,[ChangePasswordDate]=@ChangePasswordDate
                    ,[UserSecretkey]=@UserSecretkey
                    
                    where [Id]=@keyValue";
                var bl = db.ExecuteNonQuery(sql, new { userPassword = userPassword, ChangePasswordDate = DateTime.Now, keyValue = keyValue, UserSecretkey=userSecretkey });
                return bl == 1;
            }
        }
        public List<SysRole> GetFormGridData(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"SELECT 
	               [SysRoleUser].Id
	              ,[SysRole].[Id] as RoleId
	              ,[SysOrganize].Name as OrganizeName 
                  ,[SysRole].[Category]
                  ,[SysRole].[EnCode]
                  ,[SysRole].[Name]
                  ,[SysRole].[Type]
                  FROM [dbo].[SysRole] 
                  inner join [dbo].[SysOrganize] on [SysOrganize].Id=[SysRole].[OrganizeId]
                  left join [dbo].[SysRoleUser] on [SysRoleUser].RoleId=[SysRole].Id and [SysRoleUser].UserId=@UserId
                  where [SysRole].DeleteFlag=0 and [SysRole].EnabledFlag=1 and [SysRole].[Type]=1";
                //order by [SysRole].DisplayNo";
                try
                {
                    var result = db.ReaderModelList<SysRole>(sql, new { UserId = keyValue });
                    return result.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public bool SubmitAssignRoles(List<SysUser> dataList, string keyValue)
        {
            var bl = 0;
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"DELETE FROM [dbo].[SysRoleUser]";
                var ids = "";
                if (dataList != null)
                {
                   
                    for (int i = 0; i < dataList.Count(); i++)
                    {
                        if (i < dataList.Count() - 1)
                        {
                            ids = ids + "'" + dataList[i].RoleId + "',";
                        }
                        else
                        {
                            ids = ids + "'" + dataList[i].RoleId + "'";
                        }
                    }

                }

                sql += "    WHERE [UserId]=@UserId  ";
                //var ids = dataList.Select(m => m.RoleId.ToString()).Aggregate((s, s1) => s + "," + s1);
                if (!string.IsNullOrEmpty(ids))
                {
                    sql = sql + @"and [RoleId] not in (" + ids + ")";
                }

                try
                {
                    bl = db.ExecuteNonQuery(sql, new { UserId = keyValue });
                    if (dataList != null)
                    {
                        for (int i = 0; i < dataList.Count(); i++)
                        {
                            sql = @"  if not exists(select top 1 * from [dbo].[SysRoleUser] where [RoleId]=@RoleId and [UserId]=@UserId)
                            begin
                            insert into [dbo].[SysRoleUser]([Id],[RoleId],[UserId])
                            values
                            (newID(),@RoleId,@UserId)
                            end";
                            bl = db.ExecuteNonQuery(sql, new { RoleId = dataList[i].RoleId, UserId = keyValue });

                        }
                    }

                   
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            return bl == 1;
        }

        public SysUser CheckLogin(string account, string password)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                 SELECT TOP 1  
	                   logon.[Id]
                      ,logon.[UserId]
                      ,logon.[UserPassword]
                      ,logon.[UserSecretkey]
                      ,logon.[AllowStartTime]
                      ,logon.[AllowEndTime]
                      ,logon.[LockStartDate]
                      ,logon.[LockEndDate]
                      ,logon.[FirstVisitTime]
                      ,logon.[PreviousVisitTime]
                      ,logon.[LastVisitTime]
                      ,logon.[ChangePasswordDate]
                      ,logon.[MultiUserLogin]
                      ,logon.[LogOnCount]
                      ,logon.[UserOnLine]
                      ,logon.[Question]
                      ,logon.[AnswerQuestion]
                      ,logon.[CheckIPAddress]
                      ,logon.[Language]
                      ,logon.[Theme]
                  FROM [dbo].[SysUserLogOn] logon
                  inner join [dbo].[SysUser] sysuser on sysuser.Id= logon.UserId
                  where sysuser.Account=@account
                ";

                var logOnEntity = db.ReaderModel<UserLogOn>(sql, new { account = account, EnabledFlag = true });
                if (logOnEntity == null)
                {
                    throw new AppException("账户不存在，请重新输入");
                }
                string dbPassword = Md5Helper.md5(DESEncryptHelper.Encrypt(password.ToLower(), logOnEntity.UserSecretkey).ToLower(), 32).ToLower();
                if (dbPassword != logOnEntity.UserPassword)
                {
                    throw new AppException("密码不正确，请重新输入");
                }
                var entity = GetForm(logOnEntity.UserId);
                if (entity.EnabledFlag == false)
                {
                    throw new AppException("账户被系统锁定,请联系管理员");
                }

                sql = @"
                 UPDATE [dbo].[SysUserLogOn]
                   SET 
                       [PreviousVisitTime] =[LastVisitTime]
                      ,[LastVisitTime] = @LastVisitTime
                      ,[LogOnCount] = isnull([LogOnCount],0)+1      
                 WHERE [Id] = @id
                ";
                db.ExecuteNonQuery(sql, new { LastVisitTime = DateTime.Now, id = logOnEntity.Id });
                return entity;
            }
        }
        /// <summary>
        /// 下拉框值得获取
        /// </summary>
        /// <returns></returns>
        public static List<SysUser> GetUserSelectList(bool appendEmpty)
        {
            var result = new List<SysUser>();
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"
                 select Id,RealName  from SysUser where DeleteFlag= 0 and EnabledFlag=1
                ";
                db.ExecuteReaderModelList(sql, result);
                if (appendEmpty)
                {
                    result.Insert(0, new SysUser { Id = "", RealName = "未指定" });
                }
            }
            return result;
        }

    }
}
