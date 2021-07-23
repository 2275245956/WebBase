using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2.Core.DataBases;
using Z2.Core.Exceptions;
using Z2.Core.Utility;
using Z2.Core.WebApp.Model;

namespace Z2.Core.WebApp.Repository
{
    public class SortModelRep<T> where T : ISort
    {
        public virtual bool Sort(string[] ids)
        {
            var onConditions = ids.Select(m => m.ToString()).Aggregate((c, n) => { return c + "," + n; });
            var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;
            var list = new List<sortItem>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.AppendFormat(@"
                select
                 [{0}] as SettingID
                 ,[DisplayNo]
                from [dbo].[{2}]
                where [{0}]
                ", p.KeyField, p.NameField, p.TableName);
                sql.AppendFormat(@"  in ({3}) ", onConditions);
                utility.ExecuteReaderModelList(sql.ToString(), list);
            }

            var minDisplayNo = list.Select(m => m.DisplayNo).Min();

            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();

                var sql = new StringBuilder();

                sql.AppendFormat(@"
                update [dbo].[{2}] 
                set 
                 [DisplayNo] = @DisplayNo 
                where [{0}] = @SettingID 
                 ", p.KeyField, p.NameField, p.TableName);

                try
                {
                    for (var i = 0; i < ids.Length; i++)
                    {
                        utility.AddParameter("DisplayNo", minDisplayNo + i);
                        utility.AddParameter("SettingID", ids[i]);
                        utility.ExecuteNonQuery(sql.ToString());
                    }

                    utility.Commit();
                    return true;
                }
                catch (Exception)
                {
                    utility.Rollback();
                    return false;
                }
            }
        }

        public virtual int GetMaxDisplayNo()
        {
            var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;
            var sql = new StringBuilder();

            sql.AppendFormat(@"
                select isnull(max([DisplayNo]), 0) + 1  from [{2}]) ",
                p.KeyField, p.NameField, p.TableName, p.CreaterTimeFieldName, p.CreaterUserIdFieldName, p.LastUpdateTimeFieldName, p.LastUpdateUserIdFieldName, p.DeleteTimeFieldName, p.DeleteUserIdFieldName);
            using (var utility = DbUtility.GetInstance())
            {
                return Converts.ToTryInt(utility.ExecuteScalar(sql.ToString()));
            }
        }


        class sortItem
        {
            public int SettingID { get; set; }

            public int DisplayNo { get; set; }
        }

    }

    public class MasterModelRep<T,K> where T : MasterModelBase<K>, new()
    {
        public virtual List<SelectListItem> GetSelectItems(bool includeEmptyItem)
        {
            var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;

            if (p == null || string.IsNullOrEmpty(p.TableName) || string.IsNullOrEmpty(p.KeyField) || string.IsNullOrEmpty(p.NameField))
            {
                return null;
            }

            var result = new List<SelectListItem>();

            var sql = new StringBuilder();

            sql.AppendFormat(@"
            select 
             [{0}] as Value 
             ,[{1}] as Text 
            from [dbo].[{2}] 
            where [EnabledFlag] = @EnabledFlag and [DeleteFlag] = @DeleteFlag 
            order by 
             DisplayNo,[{0}],[{3}]  
            ", p.KeyField, p.NameField, p.TableName, p.LastUpdateUserIdFieldName);

            using (var utility = DbUtility.GetInstance())
            {
                result = utility.ReaderModelList<SelectListItem>(sql.ToString(), new { EnabledFlag=true, DeleteFlag =false}).ToList();
            }

            if (includeEmptyItem)
            {
                result.Insert(0, new SelectListItem() { Value = "", Text = "" });
            }

            return result;
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool AddModel(T model)
        {
            var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;

            var sql = new StringBuilder();

            if (p.AutoID)
            {
                sql.AppendFormat(@"
                insert into [dbo].[{2}] 
                (
                  [{1}] 
                 ,[EnabledFlag] 
                 ,[DeleteFlag] 
                 ,[DisplayNo] 
                 ,[{3}] 
                 ,[{4}]  
                )
                values 
                (
                 @SettingName 
                 ,@EnabledFlag 
                 ,@DeleteFlag 
                 ,@DisplayNo
                 ,@CreaterTime 
                 ,@CreaterUserId 
                )
                ", p.KeyField, p.NameField, p.TableName, p.CreaterTimeFieldName, p.CreaterUserIdFieldName, p.LastUpdateTimeFieldName, p.LastUpdateUserIdFieldName, p.DeleteTimeFieldName, p.DeleteUserIdFieldName);
            }
            else
            {
                sql.AppendFormat(@"
                insert into [dbo].[{2}] 
                (
                  [{0}] 
                 ,[{1}] 
                 ,[EnabledFlag] 
                 ,[DeleteFlag] 
                 ,[DisplayNo] 
                 ,[{3}] 
                 ,[{4}] 
                )
                values 
                (
                 (@SettingID 
                 ,@SettingName 
                 ,@EnabledFlag 
                 ,@DeleteFlag 
                 ,@DisplayNo 
                 ,@CreaterTime 
                 ,@CreaterUserId 
                )
                ", p.KeyField, p.NameField, p.TableName, p.CreaterTimeFieldName, p.CreaterUserIdFieldName, p.LastUpdateTimeFieldName, p.LastUpdateUserIdFieldName, p.DeleteTimeFieldName, p.DeleteUserIdFieldName);
            }
            model.DisplayNo = new SortModelRep<T>().GetMaxDisplayNo();
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();

                try
                {
                    utility.AddParameter("SettingID", model.MasterId);
                    utility.AddParameter("SettingName", model.MasterName);
                    utility.AddParameter("EnabledFlag", model.EnabledFlag);
                    utility.AddParameter("DeleteFlag", model.DeleteFlag);
                    utility.AddParameter("DisplayNo", model.DisplayNo);
                    utility.AddParameter("CreaterTime", DateTime.Now);
                    utility.AddParameter("CreaterUserId", model.CreaterUserId);

                    utility.ExecuteNonQuery(sql.ToString());

                    utility.Commit();

                    return true;
                }
                catch (Exception)
                {
                    utility.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool EditModel(T model)
        {
            var bl = EditCheck(model);
            if (!bl) return false;

            var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;

            var sql = new StringBuilder();

            sql.AppendFormat(@"
            update [dbo].[{2}] 
            set 
             [{1}] = @SettingName 
             ,[EnabledFlag] = @EnabledFlag 
             ,[{5}] = @UpdateDate 
             ,[{6}] = @UpdateUserID 
            where [{0}] = @SettingID 
            ", p.KeyField, p.NameField, p.TableName, p.CreaterTimeFieldName, p.CreaterUserIdFieldName, p.LastUpdateTimeFieldName, p.LastUpdateUserIdFieldName, p.DeleteTimeFieldName, p.DeleteUserIdFieldName);

            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();

                try
                {
                    utility.AddParameter("SettingID", model.MasterId);
                    utility.AddParameter("SettingName", model.MasterName);
                    utility.AddParameter("EnabledFlag", model.EnabledFlag);
                    utility.AddParameter("DisplayNo", model.DisplayNo);
                    utility.AddParameter("UpdateDate", DateTime.Now);
                    utility.AddParameter("UpdateUserID", model.LastUpdateUserId);

                    utility.ExecuteNonQuery(sql.ToString());

                    utility.Commit();

                    return true;
                }
                catch (Exception)
                {
                    utility.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// modelリスト取得
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public virtual List<T> GetModels(
            string searchText,
            int pageIndex,
            int pageSize,
            out int rowCount)
        {
            var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;
            var list = new List<T>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.AppendFormat(@"
                SELECT  ");
                sql.AppendFormat(@"
                    Top  {0}", (pageIndex + 1) * pageSize);

                sql.AppendFormat(@"
                  [{0}] as SettingID 
                 ,[{1}] as SettingName 
                 ,[EnabledFlag] 
                 ,[DisplayNo] 
                 ,[{3}] as [CreaterTime] 
                 ,[{4}] as [CreaterUserId] 
                 ,[{5}] as [LastUpdateTime] 
                 ,[{6}] as [LastUpdateUserID] 
                from [dbo].[{2}] 
                where DeleteFlag = @DeleteFlag 
                ", p.KeyField, p.NameField, p.TableName, p.CreaterTimeFieldName, p.CreaterUserIdFieldName, p.LastUpdateTimeFieldName, p.LastUpdateUserIdFieldName, p.DeleteTimeFieldName, p.DeleteUserIdFieldName);

                if (!string.IsNullOrEmpty(searchText))
                {
                    sql.AppendFormat(@"and {0} like @SearchText ", p.NameField);
                }

                sql.AppendFormat(@" 
                order by 
                 [DisplayNo],[{0}],[{1}] 
                ", p.KeyField, p.LastUpdateTimeFieldName);

                list = utility.ReaderModelList<T>(sql.ToString(), new { DeleteFlag = false, SearchText = string.Format("%{0}%", searchText) }).Skip(pageIndex * pageSize).ToList();
            }
            rowCount = GetRowCount(searchText);
            return list;
        }

        protected virtual int GetRowCount(string SearchText)
        {
            var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;

            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.AppendFormat(@"
                select 
                 count([{0}]) 
                from [dbo].[{2}] 
                where DeleteFlag = @DeleteFlag 
                ", p.KeyField, p.NameField, p.TableName);
                if (!string.IsNullOrEmpty(SearchText))
                {
                    sql.AppendFormat(@"and {0} like @SearchText ", p.NameField);
                    utility.AddParameter("SearchText", string.Format("%{0}%", SearchText));
                }
                utility.AddParameter("DeleteFlag", false);
                return Converts.ToTryInt(utility.ExecuteScalar(sql.ToString()));
            }
        }

        public virtual T GetModel(K KeyID)
        {
            var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;

            var model = new T();

            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.AppendFormat(@"
                select 
                  [{0}] as SettingID 
                 ,[{1}] as SettingName 
                 ,[EnabledFlag] 
                 ,[DisplayNo] 
                 ,[{3}] as [CreaterTime] 
                 ,[{4}] as [CreaterUserId] 
                 ,[{5}] as [LastUpdateTime] 
                 ,[{6}] as [LastUpdateUserID] 
                from [dbo].[{2}] 
                where [{0}] = @SettingID 
                ", p.KeyField, p.NameField, p.TableName, p.CreaterTimeFieldName, p.CreaterUserIdFieldName, p.LastUpdateTimeFieldName, p.LastUpdateUserIdFieldName, p.DeleteTimeFieldName, p.DeleteUserIdFieldName);
                model = utility.ReaderModel<T>(sql.ToString(), new { SettingID = KeyID });
            }

            return model;
        }

        public virtual bool DelModel(K KeyID)
        {
            var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;

            var model = GetModel(KeyID);
            var d = default(K);
            if (model == null || model.MasterId .Equals(d)) return false;

            var bl = DeleteCheck(model);
            if (!bl)
            {
                throw new IsUseingException();
                //return false;
            }

            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();

                sql.AppendFormat(@"
                update [dbo].[{2}] set 
                     [DeleteFlag]=@DeleteFlag  
                    ,[{3}] = @DeleteTime 
                    ,[{4}] = @DeleteUserId 
                where {0} = @SettingID "
                , p.KeyField, p.NameField, p.TableName, p.DeleteTimeFieldName, p.DeleteUserIdFieldName);

                utility.BeginTransaction();

                try
                {
                    utility.AddParameter("SettingID", KeyID);
                    utility.AddParameter("DeleteFlag", true);
                    utility.AddParameter("DeleteTime", DateTime.Now);
                    utility.AddParameter("DeleteUserId", model.DeleteUserId);

                    utility.ExecuteNonQuery(sql.ToString());

                    utility.Commit();
                    return true;
                }
                catch (Exception)
                {
                    utility.Rollback();
                    return false;
                }
            }
        }

        protected virtual bool EditCheck(T model)
        {
            return true;
        }

        protected virtual bool DeleteCheck(T model)
        {
            var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;
            if (!string.IsNullOrEmpty(p.LinkTableName))
            {
                var LinkTableField = p.LinkTableField;
                if (string.IsNullOrEmpty(LinkTableField))
                {
                    LinkTableField = p.KeyField;
                }

                var LinkTableFields = LinkTableField.Split(new[] { ',' });
                var LinkTableNames = p.LinkTableName.Split(new[] { ',' });
                for (int i = 0; i < LinkTableNames.Length; i++)
                {
                    var linkTableName = LinkTableNames[i];
                    var linkTableField = LinkTableFields[i];
                    using (var utility = DbUtility.GetInstance())
                    {
                        var sql = new StringBuilder();
                        sql.AppendFormat(@"
                            select 
                             sum(t.cnt) as cnt 
                            from
                            ( 
                                 select 
                                  count(*) as cnt 
                                 from [dbo].[{2}] as linkTb 
                                 where 1 = 1 
                                 and linkTb.[DeleteFlag] = @DeleteFlag 
                                 and linkTb.[{0}] =@SettingID 
                            ) as t 
                        ", linkTableField, p.NameField, linkTableName);

                        utility.AddParameter("SettingID", model.MasterId);
                        utility.AddParameter("DeleteFlag", false);
                        var cnt = Convert.ToInt32(utility.ExecuteScalar(sql.ToString()));
                        if (cnt != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public virtual bool Sort(string[] ids)
        {
            SortModelRep<T> sortRep = new SortModelRep<T>();
            return sortRep.Sort(ids);

            //var onConditions = ids.Select(m => m.ToString()).Aggregate((c, n) => { return c + "," + n; });

            //var p = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true).FirstOrDefault() as TableNameAttribute;

            //var list = new List<sortItem>();

            //using (var utility = DbUtility.GetInstance())
            //{
            //    var sql = new StringBuilder();

            //    sql.AppendFormat(@"
            //    select
            //     [{0}] as SettingID
            //     ,[DisplayNo]
            //    from [dbo].[{2}]
            //    where [{0}] in ({3})
            //    ", p.KeyField, p.NameField, p.TableName, onConditions);

            //    utility.ExecuteReaderModelList(sql.ToString(), list);
            //}

            //var minDisplayNo = list.Select(m => m.DisplayNo).Min();

            //using (var utility = DbUtility.GetInstance())
            //{
            //    utility.BeginTransaction();

            //    var sql = new StringBuilder();

            //    sql.AppendFormat(@"
            //    update [dbo].[{2}] 
            //    set 
            //     [DisplayNo] = @DisplayNo 
            //    where [{0}] = @SettingID 
            //     ", p.KeyField, p.NameField, p.TableName);

            //    try
            //    {
            //        for (var i = 0; i < ids.Length; i++)
            //        {
            //            utility.AddParameter("DisplayNo", minDisplayNo + i);
            //            utility.AddParameter("SettingID", ids[i]);
            //            utility.ExecuteNonQuery(sql.ToString());
            //        }

            //        utility.Commit();
            //        return true;
            //    }
            //    catch (Exception)
            //    {
            //        utility.Rollback();
            //        return false;
            //    }
            //}
        }

        //class sortItem
        //{
        //    public int SettingID { get; set; }

        //    public int DisplayNo { get; set; }
        //}
    }
}
