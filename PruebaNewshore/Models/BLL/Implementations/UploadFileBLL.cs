using PruebaNewshore.Models.BLL.Contracts;
using PruebaNewshore.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PruebaNewshore.Models.BLL.Implementations
{
    /// <summary>
    /// UploadFileBLL class to implementate IUploadFileBLL methods
    /// </summary>
    public class UploadFileBLL : IUploadFileBLL
    {
        #region Public Methods

        public ActionResultDTO ValidateUsersList(HttpPostedFileBase[] files)
        {
            ActionResultDTO actionResultDTO = new ActionResultDTO();

            try
            {
                actionResultDTO = UploadFilesToServer(files);
                if (actionResultDTO.HasErrors)
                {
                    return actionResultDTO;
                }
            }
            catch (Exception ex)
            {
                actionResultDTO.SetError("Ocurrió un error tratanto de validar los usuarios", ex.ToString());
            }

            return actionResultDTO;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Upload current files loaded from view to server folder
        /// </summary>
        /// <param name="files">Array of files uploaded</param>
        /// <returns>ActionResultDTO with process result</returns>
        private ActionResultDTO UploadFilesToServer(HttpPostedFileBase[] files)
        {
            ActionResultDTO actionResultDTO = new ActionResultDTO();

            try
            {
                actionResultDTO = ValidateCurrentData(files);
                if (actionResultDTO.HasErrors)
                {
                    return actionResultDTO;
                }

                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(string.Format("{0}\\{1}\\{2}", 
                                AppDomain.CurrentDomain.BaseDirectory, 
                                Constants.ServerUploadFilesFolderName, 
                                InputFileName));
                        
                        file.SaveAs(ServerSavePath);
                        
                        actionResultDTO.UserMessage = string.Format("{0} archivos cargados con éxito", files.Count());
                    }

                }
            }
            catch (Exception ex)
            {
                actionResultDTO.SetError("Ocurrió un error tratanto de cargar los archivos.", ex.ToString());
            }

            return actionResultDTO;
        }

        /// <summary>
        /// Validate current files uploaded
        /// </summary>
        /// <param name="files">Array of files uploaded</param>
        /// <returns>ActionResultDTO with process result</returns>
        private ActionResultDTO ValidateCurrentData(HttpPostedFileBase[] files)
        {
            ActionResultDTO actionResultDTO = new ActionResultDTO();

            try
            {
                if (files == null)
                {
                    actionResultDTO.SetError("No hay archivos seleccionados para cargar");
                    return actionResultDTO;
                }

                if (files.Length == 0)
                {
                    actionResultDTO.SetError("No hay archivos seleccionados para cargar");
                    return actionResultDTO;
                }

                foreach (HttpPostedFileBase file in files)
                {
                    if (file == null)
                    {
                        actionResultDTO.SetError("El archivo está vacío o no existe");
                        return actionResultDTO;
                    }

                    if (!file.FileName.EndsWith(".txt"))
                    {
                        actionResultDTO.SetError("Extensión de archivos no válida, solo se permiten archivos de extensión .txt");
                        return actionResultDTO;
                    }

                    if(file.ContentLength == 0)
                    {
                        actionResultDTO.SetError("El archivo está vacío");
                        return actionResultDTO;
                    }
                }
            }
            catch (Exception ex)
            {
                actionResultDTO.SetError("Ocurrió un erro tratanto de validar los archivos cargados", ex.ToString());
            }

            return actionResultDTO;
        }

        #endregion
    }
}