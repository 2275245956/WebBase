using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class SysFileModelReg : MasterModelRep<SysFileModel, string>
    {
        public void SaveFile(SysFileModel fileModel)
        {
            return;
        }

        public List<SysFileModel> GetFile(string fileId, string keyValue)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"select [FileID]
                          ,[FileName]
                          ,[SaveType]
                          ,[OrgFilePath]
                          ,[OrgFileName]
                          ,[FilePath]
                          ,[FileData]
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
                          FROM [dbo].[SysFile]
                          where 1=1
                          and DeleteFlag=0";
                if (!string.IsNullOrEmpty(fileId))
                {
                    sql = sql + @" and OrgFileName like @fileId";
                }
                if (!string.IsNullOrEmpty(keyValue))
                {
                    sql = sql + @" and FileID = @keyValue";
                }
                try
                {
                    var result = db.ReaderModelList<SysFileModel>(sql, new { fileId = "%" + fileId + "%", keyValue = keyValue });
                    return result.ToList();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
        public bool UpdateFileData(SysFileModel fileEntity)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"update [dbo].[SysFile] set
                        [OrgFileName]=@OrgFileName
                        ,[Description]=@Description
                        ,[LastUpdateTime]=@LastUpdateTime
                        ,[LastUpdateUserId]=LastUpdateUserId
                        where FileID=@FileID";
                try
                {
                    var bl = db.ExecuteNonQuery(sql, fileEntity);
                    return bl == 1;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        public bool AddFileData(SysFileModel fileEntity)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"insert into [dbo].[SysFile]
                          ([FileID]
                          ,[FileName]
                          ,[SaveType]
                          ,[OrgFilePath]
                          ,[OrgFileName]
                          ,[FilePath]
                          ,[FileData]
                          ,[DisplayNo]
                          ,[DeleteFlag]
                          ,[EnabledFlag]
                          ,[Description]
                          ,[CreaterTime]
                          ,[CreaterUserId])
                            values
                            (
                            @FileID
                            ,@FileName
                            ,@SaveType
                            ,@OrgFilePath
                            ,@OrgFileName
                            ,@FilePath
                            ,@FileData
                            ,@DisplayNo
                            ,0
                            ,@EnabledFlag
                            ,@Description
                            ,@CreaterTime
                            ,@CreaterUserId)";
                try
                {
                    var bl = db.ExecuteNonQuery(sql, fileEntity);
                    return bl == 1;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        public bool DelFileData(SysFileModel fileEntity)
        {
            using (var db = DbUtility.GetInstance())
            {
                var sql = @"update [dbo].[SysFile] set
                            [DeleteFlag]=1
                            ,[DeleteTime]=@DeleteTime
                            ,[DeleteUserId]=@DeleteUserId
                            where FileID=@FileID";
                try
                {
                    var bl = db.ExecuteNonQuery(sql, fileEntity);
                    return bl == 1;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
    }
}
