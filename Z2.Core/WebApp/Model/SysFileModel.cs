using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    [TableName(TableName = "SysFile", KeyField = "FileID", NameField = "FileName")]
    public class SysFileModel : MasterModelBase<string>
    {
        public override string MasterId
        {
            get
            {
                return FileID;
            }

            set
            {
                FileID = value;
            }
        }

        public override string MasterName
        {
            get
            {
                return FileName;
            }

            set
            {
                FileName = value;
            }
        }

        public string FileID { get; set; }
        public string FileName { get; set; }
        /// <summary>
        /// 原始文件路径
        /// </summary>
        public string OrgFilePath { get; set; }

        /// <summary>
        /// 原始文件名
        /// </summary>
        public string OrgFileName { get; set; }
        /// <summary>
        /// 文件保存方式
        /// 0 路径；1 数据库
        /// </summary>
        public int SaveType { get; set; }
        public string FilePath { get; set; }
        public byte[] FileData { get; set; }


        public string DownloadUrl { get; set; }

        public string Extension
        {
            get
            {
                return GetExtension(this.FileName);
            }
        }

        public static string GetExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }
            //var idx = OrgFileName.LastIndexOf(".");
            //if (idx < 1)
            //{
            //    return string.Empty;
            //}

            //return OrgFileName.Substring(idx).ToLower();
            return System.IO.Path.GetExtension(fileName);
        }
        public string Description { get; set; }
    }
}
