using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.Utility;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class SysSettingRep
    {
        #region
        public T GetSetting<T>(string settingKey, T defaultValue)
        {
            try
            {
                var v = GetSetting(settingKey, "0", "0", null);
                if (v != null)
                {
                    T r = (T)Convert.ChangeType(v, typeof(T));
                    return r;
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                return defaultValue;//default(T);
            }
        }

        public T GetSetting<T>(string settingKey, string oficeId, T defaultValue)
        {
            try
            {
                var v = GetSetting(settingKey, oficeId, "0", null);
                if (v != null)
                {
                    T r = (T)Convert.ChangeType(v, typeof(T));
                    return r;
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                return defaultValue;//default(T);
            }
        }

        public T GetSetting<T>(string settingKey, string oficeId, string userId, T defaultValue)
        {
            try
            {
                var v = GetSetting(settingKey, oficeId, userId, null);
                if (v != null)
                {
                    T r = (T)Convert.ChangeType(v, typeof(T));
                    return r;
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                return defaultValue;//default(T);
            }
        }

        private string GetSetting(string settingKey, string oficeId, string userId, string defaultValue)
        {
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();

                utility.AddParameter("SettingKey", settingKey);
                utility.AddParameter("OfficeID", oficeId);
                utility.AddParameter("UserID", userId);

                sql.AppendFormat(@" SELECT SettingValue  FROM SysSettings  WHERE SettingKey = @SettingKey and OfficeID=@OfficeID and UserID=@UserID ");

                return Converts.ToTryString(utility.ExecuteScalar(sql.ToString()), defaultValue);
            }
        }

        public bool SetSetting(string settingKey, object settingValue)
        {
            return SetSetting(settingKey, "0", "0", settingValue);
        }

        public bool SetSetting(string settingKey, string oficeId, object settingValue)
        {
            return SetSetting(settingKey, oficeId, "0", settingValue);
        }

        public bool SetSetting(string settingKey, string oficeId, string userId, object settingValue)
        {
            if (SettingExists(settingKey, oficeId, userId))
            {
                return UpdateSetting(settingKey, oficeId, userId, settingValue);
            }
            else
            {
                return InsertSetting(settingKey, oficeId, userId, settingValue);
            }
        }

        private bool SettingExists(string settingKey, string oficeId, string userId)
        {
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();

                utility.AddParameter("SettingKey", settingKey);
                utility.AddParameter("OfficeID", oficeId);
                utility.AddParameter("UserID", userId);

                sql.AppendFormat(@" SELECT SettingValue FROM M_Setting  WHERE SettingKey = @SettingKey and OfficeID=@OfficeID and UserID=@UserID ");

                return utility.ExecuteScalar(sql.ToString()) != null;
            }
        }

        private bool UpdateSetting(string settingKey, string officeId, string userId, object settingValue)
        {
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.AppendFormat(@"
                    update dbo.M_Setting set
                    SettingValue=@settingValue
                    where SettingKey=@settingKey and OfficeID=@officeId");
                utility.AddParameter("settingKey", settingKey);
                utility.AddParameter("officeId", officeId);
                utility.AddParameter("settingValue", settingValue);
                utility.AddParameter("userId", userId);
                return utility.ExecuteNonQuery(sql.ToString()) == 1;
            }
        }

        private bool InsertSetting(string settingKey, string officeId, string userId, object settingValue)
        {
            var DisplayNo = 0;
            using (var utility = DbUtility.GetInstance())
            {
                var sb = new StringBuilder();
                sb.AppendFormat(@"select max(DisplayNo) as DisplayNo from M_Setting ");
                var a = utility.ExecuteScalar(sb.ToString());
                DisplayNo = Converts.ToTryInt(a, 0) + 1;
            }

            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.AppendFormat(@"
                    insert into  dbo.M_Setting
                    (SettingValue
                    ,SettingKey
                    ,OfficeID
                    ,UserID
                    ,DisplayNo
                    ,LastUpdateTime
                    ,LastUserID
                    ) values (
                     @settingValue
                    ,@settingKey
                    ,@officeId
                    ,@userId
                    ,@DisplayNo
                    ,GetDate()
                    ,@userId)");
                utility.AddParameter("settingKey", settingKey);
                utility.AddParameter("officeId", officeId);
                utility.AddParameter("settingValue", settingValue);
                utility.AddParameter("DisplayNo", DisplayNo);
                utility.AddParameter("userId", userId);
                return utility.ExecuteNonQuery(sql.ToString()) == 1;
            }
        }
        #endregion
        public List<SysModuleOperate> GetOperateDataJson(string roleValue, string moduleValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"SELECT 
                    auth.Id as AuthId,
                    auth.AuthorizeId,
                           smo.[Id]
                          ,smo.[Name]
                          ,smo.[KeyCode]
                          ,smo.[ModuleId]  
                      FROM [dbo].[SysModuleOperate] smo
                       left join (
                     SELECT 
	                     srao.Id
	                    ,srao.AuthorizeId
	                    ,srao.KeyCode
                      FROM [dbo].[SysRoleAuthorize] sra
                      inner join  [dbo].[SysRoleAuthorizeOperate] srao on srao.AuthorizeId=sra.Id
                      Where ItemType=1 and ItemId=@ModuleId
                      and ObjectType=1 and ObjectId=@RoleId
                      ) auth on auth.KeyCode=smo.[KeyCode]
                      where smo.ModuleId=@ModuleId 
                      order by smo.Sort";
                try
                {
                    var result = db.ReaderModelList<SysModuleOperate>(sql, new { RoleId = roleValue, ModuleId = moduleValue });
                    return result.ToList();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public bool SaveRoleAuthorize(string roleValue,
            string moduleValue,
            List<SysModuleOperate> roleAuthorizeEntity)
        {
            var AuthorizeId = ResultHelper.NewId();
            var bl = 0;
            using (var db = DbUtility.GetInstance())
            {
                db.BeginTransaction();
                var sql = @"delete from [dbo].[SysRoleAuthorize] where [ItemId]=@moduleValue
                        and [ObjectId]=@roleValue
                        insert into [dbo].[SysRoleAuthorize]
                        (Id
                        ,ItemType
                        ,ItemId
                        ,ObjectType
                        ,ObjectId
                        ,DisplayNo
                        ,CreaterTime
                        ,CreaterUserId)
                        values
                        (@AuthorizeId
                        ,1
                        , @moduleValue
                        ,1
                        , @roleValue
                        , @DisplayNo
                        , @CreaterTime
                        , @CreaterUserId)";
                try
                {
                    bl = db.ExecuteNonQuery(sql, new { AuthorizeId = AuthorizeId, moduleValue = moduleValue, roleValue = roleValue, DisplayNo = 1, CreaterTime = DateTime.Now, CreaterUserId = "admin" });
                }
                catch (Exception e)
                {
                    throw;
                }
                for (int i = 0; i < roleAuthorizeEntity.Count(); i++)
                {
                    var sb = @"insert into [dbo].[SysRoleAuthorizeOperate]
                            (Id
                            ,AuthorizeId
                            ,KeyCode
                            ,IsValid)
                            values
                            (@Id
                            ,@AuthorizeId
                            ,@KeyCode
                            ,@IsValid)";
                    bl = db.ExecuteNonQuery(sb, new { Id = ResultHelper.NewId(), AuthorizeId = AuthorizeId, KeyCode = roleAuthorizeEntity[i].KeyCode, IsValid = "" });
                }
                db.Commit();
                return bl == 1;
            }
        }

        #region System global configuration
        /// <summary>
        ///加载配置信息
        /// </summary>
        /// <returns>list集合</returns>
        public List<SysSettings> GetSysSettings(string keywords)
        {
            using (var db = DbUtility.GetInstance())
            {
                string sql = @"SELECT [SettingID]
                          ,[SettingKey]
                          ,[OfficeID]
                          ,[UserID]
                          ,[SettingName]
                          ,[SettingNote]
                          ,[SettingValue]
                          ,[DisplayNo]
                          ,[LastUpdateTime]
                          ,[LastUpdateUserId]
                      FROM[dbo].[SysSettings]
                      Where OfficeID = '0' and UserID = '0'
                     ";


                if (!string.IsNullOrEmpty(keywords))
                {
                    sql += " and [SettingName] like   @keywords";
                }
                sql += "  Order by DisplayNo";

                try
                {
                    var res = db.ReaderModelList<SysSettings>(sql, new { keywords = "%" + keywords + "%" });
                    return res.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }


        public SysSettings GetSysSettingsFrom(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                string sql = @"SELECT [SettingID]
                          ,[SettingKey]
                          ,[OfficeID]
                          ,[UserID]
                          ,[SettingName]
                          ,[SettingNote]
                          ,[SettingValue]
                          ,[DisplayNo]
                          ,[LastUpdateTime]
                          ,[LastUpdateUserId]
                      FROM [dbo].[SysSettings]
                      Where OfficeID = '0' and UserID = '0'
                     ";


                if (!string.IsNullOrEmpty(keyValue))
                {
                    sql += " and [SettingID]= @keyValue";
                }
                try
                {
                    var res = db.ReaderModel<SysSettings>(sql, new { keyValue = keyValue });
                    return res;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }




        /// <summary>
        /// 修改 或者 添加 配置信息 根据id的是否为空来选择是添加还是修改
        /// </summary>
        /// <param name="roleEntity">objectEntity 实体对象</param>
        /// <returns>返回是否改变  为bool</returns>
        public bool SubmitForm(SysSettings sysSettingEntity)
        {
            //初始化部分属性值
            sysSettingEntity.LastUpdateTime = DateTime.Now;

            sysSettingEntity.OfficeID = "0";
            sysSettingEntity.UserID = "0";

            var sql = string.Empty;
            //如果SettingId为空    添加信息
            if (string.IsNullOrEmpty(sysSettingEntity.SettingID))
            {
                //GUID自动产生SettingId
                sysSettingEntity.SettingID = ResultHelper.NewId();
                sql = @"
                    INSERT INTO [dbo].[SysSettings]
                   ([SettingID]
                   ,[SettingKey]
                   ,[OfficeID]
                   ,[UserID]
                   ,[SettingName]
                   ,[SettingNote]
                   ,[SettingValue]
                   ,[DisplayNo]
                   ,[LastUpdateTime]
                   ,[LastUpdateUserId])
             VALUES
                   (@SettingID		
                   ,@SettingKey		
                   ,@OfficeID		
                   ,@UserID			
                   ,@SettingName	
                   ,@SettingNote	
                   ,@SettingValue	
                   ,@DisplayNo		
                   ,@LastUpdateTime	
                   ,@LastUpdateUserId)";
            }
            else   //否则就是修改 某个配置的信息
            {
                sql = @"
                    UPDATE [dbo].[SysSettings]
                    SET [SettingKey] = @SettingKey    
                      ,[SettingName] = @SettingName
                      ,[SettingNote] = @SettingNote
                      ,[SettingValue] = @SettingValue
                      ,[LastUpdateTime] = @LastUpdateTime
                      ,[LastUpdateUserId] = @LastUpdateUserId
                    WHERE	[SettingID] =@SettingID";
            }
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var effRow = db.ExecuteNonQuery(sql, sysSettingEntity);
                    return effRow == 1;
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }

        /// <summary>
        /// 删除 某个配置
        /// </summary>
        /// <param name="keyValue">删除关键字  默认为Id</param>
        /// <returns></returns>
        public bool DeleteForm(string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"delete　
                            from　[dbo].[SysSettings]
                            where　[dbo].[SysSettings].SettingID=@SettingID";
                try
                {
                    var bl = db.ExecuteNonQuery(sql, new { SettingID = keyValue });
                    return bl == 1;
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        #endregion

    }
}
