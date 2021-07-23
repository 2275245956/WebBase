using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Model.Mall
{
    /// <summary>
    /// 商品种类
    /// </summary>
    public class ProductCategory
    {
        public ProductCategory() { }

        private string _Id;
        private string _ParentId;
        private string _ItemCode;
        private string _ItemName;
        private string _SimpleSpelling;
        private int? _Layers;
        private string _PriceRange;
        private string _TagText;
        private string _SubTitle;
        private string _CategoryImg;
        private string _Path;
        private int? _DisplayNo;
        private bool _DeleteFlag;
        private bool _EnabledFlag;
        private string _Description;
        private DateTime? _CreaterTime;
        private string _CreaterUserId;
        private DateTime? _LastUpdateTime;
        private string _LastUpdateUserId;
        private DateTime? _DeleteTime;
        private string _DeleteUserId;

        public string Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = value;
            }
        }

        public string ParentId
        {
            get
            {
                return _ParentId;
            }

            set
            {
                _ParentId = value;
            }
        }

        public string ItemCode
        {
            get
            {
                return _ItemCode;
            }

            set
            {
                _ItemCode = value;
            }
        }

        public string ItemName
        {
            get
            {
                return _ItemName;
            }

            set
            {
                _ItemName = value;
            }
        }

        public string SimpleSpelling
        {
            get
            {
                return _SimpleSpelling;
            }

            set
            {
                _SimpleSpelling = value;
            }
        }

        public int? Layers
        {
            get
            {
                return _Layers;
            }

            set
            {
                _Layers = value;
            }
        }

        public string PriceRange
        {
            get
            {
                return _PriceRange;
            }

            set
            {
                _PriceRange = value;
            }
        }

        public string TagText
        {
            get
            {
                return _TagText;
            }

            set
            {
                _TagText = value;
            }
        }

        public string SubTitle
        {
            get
            {
                return _SubTitle;
            }

            set
            {
                _SubTitle = value;
            }
        }

        public string CategoryImg
        {
            get
            {
                return _CategoryImg;
            }

            set
            {
                _CategoryImg = value;
            }
        }

        public string Path
        {
            get
            {
                return _Path;
            }

            set
            {
                _Path = value;
            }
        }

        public int? DisplayNo
        {
            get
            {
                return _DisplayNo;
            }

            set
            {
                _DisplayNo = value;
            }
        }

        public bool DeleteFlag
        {
            get
            {
                return _DeleteFlag;
            }

            set
            {
                _DeleteFlag = value;
            }
        }

        public bool EnabledFlag
        {
            get
            {
                return _EnabledFlag;
            }

            set
            {
                _EnabledFlag = value;
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        public DateTime? CreaterTime
        {
            get
            {
                return _CreaterTime;
            }

            set
            {
                _CreaterTime = value;
            }
        }

        public string CreaterUserId
        {
            get
            {
                return _CreaterUserId;
            }

            set
            {
                _CreaterUserId = value;
            }
        }

        public DateTime? LastUpdateTime
        {
            get
            {
                return _LastUpdateTime;
            }

            set
            {
                _LastUpdateTime = value;
            }
        }

        public string LastUpdateUserId
        {
            get
            {
                return _LastUpdateUserId;
            }

            set
            {
                _LastUpdateUserId = value;
            }
        }

        public DateTime? DeleteTime
        {
            get
            {
                return _DeleteTime;
            }

            set
            {
                _DeleteTime = value;
            }
        }

        public string DeleteUserId
        {
            get
            {
                return _DeleteUserId;
            }

            set
            {
                _DeleteUserId = value;
            }
        }
    }
}
