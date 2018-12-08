using PruebaNewshore.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PruebaNewshore.Models.BLL.Contracts
{
    /// <summary>
    /// IUploadFileBLL interface to manage all customer bussiness logic
    /// </summary>
    interface IUploadFileBLL
    {
        /// <summary>
        /// Validate if newshore user exists in list
        /// </summary>
        /// <param name="files">Array of files uploaded</param>
        /// <returns>ActionResultReturnDTO (T as FileInfo) to download file</returns>
        ActionResultReturnDTO<FileInfo> ValidateUsersList(HttpPostedFileBase[] files);
    }
}
