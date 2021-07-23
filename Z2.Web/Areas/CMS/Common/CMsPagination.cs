namespace Z2.Web.Areas.CMS.Common
{
    /// <summary>
    /// 分页
    /// </summary>
    public class CmsPagination
    {

        public CmsPagination()
        {
            this.PageIndex = 0;
            this.PageSize = 20;
        }
        /// <summary>
        /// 当前页，索引从0开始。
        /// </summary>
        public int PageIndex { get; set; }
        public int PageIndexReal => PageIndex + 1;

        private int _pageSize = 0;
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                if (_pageSize <= 0)
                {
                    _pageSize = 20;
                }
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int AllPage
        {
            get
            {
                var num = RecordCount / PageSize;
                if (RecordCount % PageSize > 0)
                {
                    num++;
                }
                return (int)num;
            }
        }
        /// <summary>
        /// 总数据量
        /// </summary>
        public long RecordCount { get; set; }
    }
}
