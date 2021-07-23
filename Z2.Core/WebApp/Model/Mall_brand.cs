using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
  public partial  class Mall_brand
    {

        public Mall_brand()
        {

        }
        #region Model
        private string _id;
        private string _itemcode;
        private string _itemname;
        private string _simplespelling;
        private string _logo;
        private int  _displayno;
        private bool _deleteflag;
        private bool _enabledflag;
        private string _description;
        private DateTime _creatertime;
        private string _createruserid;
        private DateTime _lastupdatetime;
        private string _lastupdateuserid;
        private DateTime _deletetime;
        private string _deleteuserid;
        private string _LogoPath;




        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Itemcode
        {
            get
            {
                return _itemcode;
            }

            set
            {
                _itemcode = value;
            }
        }

        public string Itemname
        {
            get
            {
                return _itemname;
            }

            set
            {
                _itemname = value;
            }
        }

        public string Simplespelling
        {
            get
            {
                return _simplespelling;
            }

            set
            {
                _simplespelling = value;
            }
        }

        public string Logo
        {
            get
            {
                return _logo;
            }

            set
            {
                _logo = value;
            }
        }

        public int Displayno
        {
            get
            {
                return _displayno;
            }

            set
            {
                _displayno = value;
            }
        }

        public bool Deleteflag
        {
            get
            {
                return _deleteflag;
            }

            set
            {
                _deleteflag = value;
            }
        }

        public bool Enabledflag
        {
            get
            {
                return _enabledflag;
            }

            set
            {
                _enabledflag = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

     

        

        public DateTime Lastupdatetime
        {
            get
            {
                return _lastupdatetime;
            }

            set
            {
                _lastupdatetime = value;
            }
        }

       

        public DateTime Deletetime
        {
            get
            {
                return _deletetime;
            }

            set
            {
                _deletetime = value;
            }
        }

        
        public DateTime Creatertime
        {
            get
            {
                return _creatertime;
            }

            set
            {
                _creatertime = value;
            }
        }

        public string Createruserid
        {
            get
            {
                return _createruserid;
            }

            set
            {
                _createruserid = value;
            }
        }

        public string Deleteuserid
        {
            get
            {
                return _deleteuserid;
            }

            set
            {
                _deleteuserid = value;
            }
        }

        public string Lastupdateuserid
        {
            get
            {
                return _lastupdateuserid;
            }

            set
            {
                _lastupdateuserid = value;
            }
        }

        public string LogoPath
        {
            get
            {
                return _LogoPath;
            }

            set
            {
                _LogoPath = value;
            }
        }
        #endregion





    }
}
