using Microsoft.AspNetCore.Mvc;
using System;

namespace CentennialTalk.Models.DTOModels
{
    [Serializable]
    public class ResponseDTO
    {
        public ResponseCode code;
        public object data;

        public ResponseDTO(ResponseCode code, object data)
        {
            this.code = code;
            this.data = data;
        }

        public JsonResult Json
        {
            get
            {
                return new JsonResult(this);
            }
        }
    }

    public enum ResponseCode
    {
        OK = 200,
        ERROR = 500,
    }
}