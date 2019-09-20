using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace API.Model
{
    /// <summary>
    /// 服务端向客户端返回信息类
    /// </summary>
    [DataContract]
    public class ReturnMessage
    {
        private bool _Success = false;
        private string _Command = "";
        private string _Status = null;
        private JArray _Array = null;
        //private string _Redirect = "";
        //private List<object> _Data = null;

        /// <summary>
        /// 请求状态
        /// </summary>
        [DataMember]
        public bool Success
        {
            get { return _Success; }
            set { _Success = value; }
        }
        /// <summary>
        /// 信息类型
        /// </summary>
        //[DataMember]
        //public int TypeID
        //{
        //    get { return _TypeID; }
        //    set { _TypeID = value; }
        //}
        /// <summary>
        /// 提示信息
        /// </summary>
        [DataMember]
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        [DataMember]
        public string Command
        {
            get { return _Command; }
            set { _Command = value; }
        }


        /// <summary>
        /// 重转地址
        /// </summary>
        //[DataMember]
        //public string Redirect
        //{
        //    get { return _Redirect; }
        //    set { _Redirect = value; }
        //}
        ///// <summary>
        ///// 数据集
        ///// </summary>
        //[DataMember]
        //public List<object> Data
        //{
        //    get { return _Data; }
        //    set { _Data = value; }
        //}

        /// <summary>
        /// JArray
        /// </summary>
        [DataMember]
        public JArray Array
        {
            get { return _Array; }
            set { _Array = value; }
        }

    }
}