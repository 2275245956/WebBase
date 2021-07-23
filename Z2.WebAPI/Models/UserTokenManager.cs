using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Z2.Model.Token;
using Z2.Repository.Token;

namespace Z2.WebAPI
{
    public class UserTokenManager
    {
        public const string TOKENNAME = "token";

        public const string MS_HttpContext = "MS_HttpContext";

        private static UserTokenManager _instance = new UserTokenManager();
        public static UserTokenManager Instance
        {
            get
            {
                return _instance;
            }
        }

        // private static readonly IUserTokenBLL _tokenRep;
        private System.Collections.Concurrent.ConcurrentDictionary<string, UserTokenModel> _tokens;
        private int _removeTimeoutFlag = 0;
        private int _existTokenCheckCount = 0;

        public UserTokenRep _tokenRep { get; set; }

        private UserTokenManager()
        {
            _tokenRep = new UserTokenRep();
            InitCache();
        }
        /// <summary>
        /// 初始化缓存
        /// </summary>
        private void InitCache()
        {
            var tokens = _tokenRep.GetAll().Select(m => new KeyValuePair<string, UserTokenModel>(m.Token, m));
            _tokens = new System.Collections.Concurrent.ConcurrentDictionary<string, UserTokenModel>(tokens);

        }


        public string GetUId(string token)
        {
            var tokens = _tokens;
            var result = string.Empty;
            if (tokens.ContainsKey(token))
            {
                var id = tokens[token].UserId;
                result = id;
            }
            return result;
        }


        public string GetPermission(string token)
        {
            var tokens = _tokens;
            if (tokens.ContainsKey(token))
                return tokens[token].RoleId;
            else
                return "NoAuthorize";
        }

        //public string GetUserType(string token)
        //{
        //    var tokens = InitCache();
        //    if (tokens.Count == 0)
        //        return "";
        //    else
        //        return tokens.Where(c => c.Token == token).Select(c => c.UserType).FirstOrDefault();
        //}

        /// <summary>
        /// 判断令牌是否存在
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsExistToken(string token)
        {
            System.Threading.Interlocked.Increment(ref _existTokenCheckCount);
            var tokens = _tokens;
            if (tokens == null || tokens.Count == 0) return false;
            if (!tokens.ContainsKey(token))
                return false;

            var t = tokens[token];
            if (t.Timeout < DateTime.Now)
            {
                RemoveToken(t);
                try
                {
                    RemoveTimeout();
                }
                catch (Exception) { }
                return false;
            }
            else
            {
                // 小于8小时 更新过期时间                    
                t.Timeout = DateTime.Now.AddHours(8);
                UpdateToken(t);
                try
                {
                    RemoveTimeout();
                }
                catch (Exception) { }
                return true;
            }
        }

        private void RemoveTimeout()
        {
            if (_existTokenCheckCount > 1000)
            {
                if (System.Threading.Interlocked.Increment(ref _removeTimeoutFlag) == 1)
                {
                    Task.Factory.StartNew(() =>
                    {
                        var tokens = _tokens;
                        var removeList = tokens.Where(m => m.Value.Timeout < DateTime.Now).ToList();
                        if (removeList != null && removeList.Any())
                        {
                            removeList.ForEach(m =>
                            {
                                UserTokenModel tkm;
                                if (tokens.TryRemove(m.Key, out tkm))
                                {
                                }
                            });
                        }
                        _tokenRep.RemoveTimeOut();
                        System.Threading.Interlocked.Exchange(ref _removeTimeoutFlag, 0);
                        System.Threading.Interlocked.Exchange(ref _existTokenCheckCount, 0);
                    });
                }
            }
        }
        /// <summary>
        /// 添加令牌， 没有则添加，有则更新
        /// </summary>
        /// <param name="token"></param>
        public void AddToken(UserTokenModel token)
        {
            var tokens = _tokens;
            // 不存在  怎增加
            if (!IsExistToken(token.Token))
            {
                token.ID = 0;
                tokens.TryAdd(token.Token, token);
                // 插入数据库
                _tokenRep.Add(token);
            }
            else  // 有则更新
            {
                UpdateToken(token);
            }
        }

        public bool UpdateToken(UserTokenModel token)
        {
            var tokens = _tokens;
            if (tokens == null || tokens.Count == 0)
            {
                return false;
            }
            else
            {
                if (!tokens.ContainsKey(token.Token))
                    return false;
                var t = tokens[token.Token];
                t.Timeout = token.Timeout;
                _tokenRep.Update(t);
                // 更新数据库
                //var tt = _tokenRep.FindByToken(token.Token);
                //if (tt != null)
                //{
                //    tt.UId = token.UId;
                //    tt.Role = token.Role;
                //    tt.Timeout = token.Timeout;
                //    _tokenRep.Update(tt);
                //}
                return true;
            }
        }
        /// <summary>
        /// 移除指定令牌
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public void RemoveToken(UserTokenModel token)
        {
            var tokens = _tokens;
            if (tokens == null || tokens.Count == 0) return;
            UserTokenModel tkm;
            if (tokens.TryRemove(token.Token, out tkm))
            {
                _tokenRep.Remove(tkm);
            }
        }

        public void RemoveToken(string token)
        {
            var tokens = _tokens;
            if (tokens == null || tokens.Count == 0) return;

            UserTokenModel tkm;
            if (tokens.TryRemove(token, out tkm))
            {
                _tokenRep.Remove(tkm);
            }
        }

        public void RemoveTokenByUid(string uid)
        {
            var tokens = _tokens;
            if (tokens == null || tokens.Count == 0) return;

            var ts = tokens.Where(c => c.Value.UserId == uid).ToList();
            foreach (var t in ts)
            {
                UserTokenModel tkm;
                if (tokens.TryRemove(t.Key, out tkm))
                {
                    _tokenRep.Remove(tkm);
                }
            }
        }

    }
}