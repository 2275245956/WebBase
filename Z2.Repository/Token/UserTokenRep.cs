using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Model.Token;

namespace Z2.Repository.Token
{
    public class UserTokenRep
    {
        public UserTokenModel FindByToken(string token)
        {
            UserTokenModel model = new UserTokenModel();
            var sql = @"
                SELECT [ID]
                      ,[Token]
                      ,[UserId]
                      ,[LoginId]
                      ,[SystemAuthority]
                      ,[Timeout]
                      ,[RoleId]
                FROM [dbo].[UserToken]
                Where [Token]=@token
                ";
            using (var utility = DbUtility.GetInstance())//MedicalFaclilty
            {
                try
                {
                    utility.AddParameter("token", token);
                    utility.ExecuteReaderModel(sql, model);
                    return model;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Remove(UserTokenModel tt)
        {
            var sql = @"
                Delete FROM [dbo].[UserToken]
                Where [Token]=@token
                ";
            using (var utility = DbUtility.GetInstance())//MedicalFaclilty
            {
                try
                {
                    utility.AddParameter("token", tt.Token);
                    utility.ExecuteNonQuery(sql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Update(UserTokenModel tt)
        {
            var sql = @"
                Update [dbo].[UserToken]
                Set Timeout=@Timeout
                Where [Token]=@token
                ";
            using (var utility = DbUtility.GetInstance())//MedicalFaclilty
            {
                try
                {
                    utility.AddParameter("Timeout", tt.Timeout);
                    utility.AddParameter("token", tt.Token);
                    utility.ExecuteNonQuery(sql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public List<UserTokenModel> GetAll()
        {
            var list = new List<UserTokenModel>();
            var sql1 = @"
                 delete from [dbo].[UserToken] where [Timeout]<GETDATE()
                ";
            var sql2 = @"
                SELECT [ID]
                      ,[Token]
                      ,[UserId]
                      ,[LoginId]
                      ,[SystemAuthority]
                      ,[Timeout]
                      ,[RoleId]
                  FROM [dbo].[UserToken]
                ";
            using (var utility = DbUtility.GetInstance())//
            {
                try
                {
                    utility.ExecuteNonQuery(sql1);
                    utility.ExecuteReaderModelList(sql2, list);
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Add(UserTokenModel token)
        {
            var sql = @"
                INSERT INTO [dbo].[UserToken]
                           ([Token]
                           ,[UserId]
                           ,[LoginId]
                           ,[SystemAuthority]
                           ,[Timeout]
                           ,[RoleId])
                     VALUES
                           (@Token
                           ,@UserId
                           ,@LoginId
                           ,@SystemAuthority
                           ,@Timeout
                           ,@RoleId)
                ";
            using (var utility = DbUtility.GetInstance())//MedicalFaclilty
            {
                try
                {
                    utility.AddParameter("token", token.Token);
                    utility.AddParameter("UserId", token.UserId);
                    utility.AddParameter("LoginId", token.LoginId);
                    utility.AddParameter("SystemAuthority", token.DutyId);
                    utility.AddParameter("Timeout", token.Timeout);
                    utility.AddParameter("RoleId", token.DepartmentId);
                    utility.ExecuteNonQuery(sql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void RemoveTimeOut()
        {
            using (var utility = DbUtility.GetInstance())//MedicalFaclilty
            {
                utility.ExecuteNonQuery("Delete from [dbo].[UserToken] where Timeout<GETDATE()");
            }
        }
    }
}
