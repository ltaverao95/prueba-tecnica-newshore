using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaNewshore.Models
{
    public class Constants
    {
        /// <summary>
        /// Result of actions
        /// </summary>
        public enum EnumResult
        {
            ERROR = 1,
            SUCCESS = 2
        }

        /// <summary>
        /// Folder name where files are uploaded
        /// </summary>
        public const string ServerUploadFilesFolderName = "UploadedFiles";

        public const string DataListFileName = "CONTENIDO.txt";
        public const string UsersListFileName = "REGISTRADOS.txt";
        public const string ResultsFileName = "RESULTADOS.txt";
    }
}