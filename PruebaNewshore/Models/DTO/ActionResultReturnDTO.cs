using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaNewshore.Models.DTO
{
    public class ActionResultReturnDTO<T>: ActionResultDTO
    {
        public ActionResultReturnDTO(): base()
        {
            ResultData = default(T);
        }

        public ActionResultReturnDTO(T resultInstance): base()
        {
            ResultData = resultInstance;
        }

        public new ActionResultReturnDTO<T> SetError(string errorMessage)
        {
            base.SetError(errorMessage);

            return this;
        }

        public new ActionResultReturnDTO<T> SetError(string errorMessage, string stacktrace)
        {
            base.SetError(errorMessage, stacktrace);

            return this;
        }

        public T ResultData { get; set; }
    }
}