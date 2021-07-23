using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.Web
{
    public interface IApiResponseMessageValue { }

    public class ApiMessageValue : IApiResponseMessageValue
    {
        public string Message { get; set; }

    }

    public static class JsonExtent
    {
        public static object ToJson(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json);
        }
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
    }

    public class JsonHandler
    {

        public static JsonMessage CreateMessage(int ptype, string pmessage, string pvalue)
        {
            JsonMessage json = new JsonMessage()
            {
                type = ptype,
                message = pmessage,
                value = pvalue
            };
            return json;
        }
        public static JsonMessage CreateMessage(int ptype, string pmessage)
        {
            JsonMessage json = new JsonMessage()
            {
                type = ptype,
                message = pmessage,
            };
            return json;
        }

        public static ApiResponseMessage CreateApiResponseMessage(int Code, string Message, IApiResponseMessageValue MessageValue)
        {
            var json = new ApiResponseMessage()
            {
                Code = Code,
                message = Message,
                messageValue = MessageValue
            };
            return json;
        }

        public static ApiResponseMessage CreateApiResponseMessage(int Code, string Message)
        {
            var json = new ApiResponseMessage()
            {
                Code = Code,
                message = Message,
            };
            return json;
        }

        public static ApiResponseMessage CreateApiResponseMessage(bool status, string Message)
        {
            var json = new ApiResponseMessage()
            {
                Code = status ? 0 : 1,
                message = Message,
            };
            return json;
        }

        public static ApiResponseMessage CreateSuccessApiMessage(object MessageValue)
        {
            var json = new ApiResponseMessage()
            {
                Code = 0,
                message = "OK",
                messageValue = MessageValue
            };
            return json;
        }

        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }

        public static T Deserialize<T>(string json)
        {
            Newtonsoft.Json.JsonSerializer m_json = new Newtonsoft.Json.JsonSerializer();
            m_json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            m_json.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
            m_json.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            m_json.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            StringReader sr = new StringReader(json);
            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
            object result = m_json.Deserialize(reader, typeof(T));
            reader.Close();
            return (T)result;
        }

    }

    public class JsonMessage
    {
        public int type { get; set; }
        public string message { get; set; }
        public string value { get; set; }
    }
    public class ApiResponseMessage
    {
        public ApiResponseMessage() { }

        public int Code { get; set; }
        /// <summary>
        /// 操作结果类型
        /// </summary>
        public object state { get; set; }
        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public object messageValue { get; set; }
    }
    public enum ResultType
    {
        /// <summary>
        /// 消息结果类型
        /// </summary>
        info,
        /// <summary>
        /// 成功结果类型
        /// </summary>
        success,
        /// <summary>
        /// 警告结果类型
        /// </summary>
        warning,
        /// <summary>
        /// 异常结果类型
        /// </summary>
        error
    }


}
