using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaNewshore.Models.DTO
{
    public class ActionResultDTO
    {
        public ActionResultDTO()
        {
            Result = Constants.EnumResult.SUCCESS;
        }

        public bool IsOk
        {
            get
            {
                return Result == Constants.EnumResult.SUCCESS;
            }
        }

        public bool HasErrors
        {
            get
            {
                return !IsOk;
            }
        }

        public virtual ActionResultDTO SetError(string errorMessage)
        {
            this.Result = Constants.EnumResult.ERROR;
            this.UserMessage = errorMessage;

            return this;
        }

        public virtual ActionResultDTO SetError(string errorMessage, string stackTrace)
        {
            this.StackTraceMessage = stackTrace;
            return this.SetError(errorMessage);
        }

        public Constants.EnumResult Result { get; set; }
        public string UserMessage { get; set; }
        public string StackTraceMessage { get; set; }
    }
}