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
        #region Fields

        /// <summary>
        /// Get server uploaded folder files path
        /// </summary>
        private string ServerFilesFolderPath
        {
            get
            {
                return string.Format("{0}\\{1}\\", AppDomain.CurrentDomain.BaseDirectory, Constants.ServerUploadFilesFolderName);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validate if newshore user exists in list
        /// </summary>
        /// <param name="files">Array of files uploaded</param>
        /// <returns>ActionResultReturnDTO (T as FileInfo) to download file</returns>
        public ActionResultReturnDTO<FileInfo> ValidateUsersList(HttpPostedFileBase[] files)
        {
            ActionResultReturnDTO<FileInfo> actionResultReturnDTO = new ActionResultReturnDTO<FileInfo>();

            try
            {
                ActionResultDTO actionResultDTO = UploadFilesToServer(files);
                if (actionResultDTO.HasErrors)
                {
                    actionResultReturnDTO.SetError(actionResultDTO.UserMessage, actionResultDTO.StackTraceMessage);
                    return actionResultReturnDTO;
                }

                actionResultReturnDTO = GetUsersList();
            }
            catch (Exception ex)
            {
                actionResultReturnDTO.SetError("Ocurrió un error tratanto de validar los usuarios", ex.ToString());
            }

            return actionResultReturnDTO;
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
                        var ServerSavePath = Path.Combine(string.Format("{0}{1}",
                                                                        ServerFilesFolderPath,
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

                int count = 0;

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

                    count++;
                }

                if(count < 2)
                {
                    actionResultDTO.SetError("Faltan archivos para procesar la información");
                    return actionResultDTO;
                }
            }
            catch (Exception ex)
            {
                actionResultDTO.SetError("Ocurrió un erro tratanto de validar los archivos cargados", ex.ToString());
            }

            return actionResultDTO;
        }

        /// <summary>
        /// Get report from users searched in files
        /// </summary>
        /// <returns>ActionResultReturnDTO (T as FileInfo) to download file</returns>
        private ActionResultReturnDTO<FileInfo> GetUsersList()
        {
            ActionResultReturnDTO<FileInfo> actionResultReturnDTO = new ActionResultReturnDTO<FileInfo>();

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(ServerFilesFolderPath);
                FileInfo[] filesFromFolder = directoryInfo.GetFiles();

                IList<string> fileDataList = new List<string>();
                IList<string> fileUsersList = new List<string>();

                foreach (FileInfo currentFileInfo in filesFromFolder)
                {
                    string filename = currentFileInfo.Name;

                    using (StreamReader currentFileStream = new StreamReader(ServerFilesFolderPath + filename))
                    {
                        string fileContent = currentFileStream.ReadToEnd();
                        fileContent = fileContent.Replace("\r", "");

                        if (filename.Equals(Constants.DataListFileName))
                        {
                            fileDataList = fileContent.Split('\n').ToList();
                        }
                        else if (filename.Equals(Constants.UsersListFileName))
                        {
                            fileUsersList = fileContent.Split('\n').ToList();
                        }
                    }
                }
                
                actionResultReturnDTO = EvalUsersList(fileUsersList, fileDataList);
            }
            catch (Exception ex)
            {
                actionResultReturnDTO.SetError("Ocurrió un error validando los usuarios", ex.ToString());
            }

            return actionResultReturnDTO;
        }

        /// <summary>
        /// Eval users list with contents list to find current users
        /// </summary>
        /// <param name="usersList">Users list</param>
        /// <param name="listData">List data</param>
        /// <returns>ActionResultReturnDTO (T as FileInfo) to download file</returns>
        private ActionResultReturnDTO<FileInfo> EvalUsersList(IList<string> usersList,
                                                              IList<string> listData)
        {
            ActionResultReturnDTO<FileInfo> actionResultReturnDTO = new ActionResultReturnDTO<FileInfo>();

            try
            {
                string path = ServerFilesFolderPath + Constants.ResultsFileName;
                DirectoryInfo directoryInfo = new DirectoryInfo(ServerFilesFolderPath);
                File.Create(path).Dispose();

                using (StreamWriter fileStream = new StreamWriter(path, true))
                {
                    for (int i = 0; i < usersList.Count; i++)
                    {
                        string currentUser = usersList[i];

                        if (string.IsNullOrEmpty(currentUser))
                        {
                            continue;
                        }

                        fileStream.Write(string.Format("{0}-->", currentUser));

                        string userFounded = string.Empty;

                        for (int j = 0; j < currentUser.Length; j++)
                        {
                            string currentUserLetter = currentUser[j].ToString();

                            for (int k = 0; k < listData.Count; k++)
                            {
                                if (currentUserLetter.Equals(listData[k]))
                                {
                                    userFounded += listData[k];
                                    break;
                                }
                            }
                        }

                        if (currentUser.Equals(userFounded))
                        {
                            fileStream.Write("Si Existe");
                            ActionResultReturnDTO<IList<string>> actionResultReturnDTORemoveLetters = RemoveLettersFromListData(listData, userFounded);
                            if (actionResultReturnDTORemoveLetters.HasErrors)
                            {
                                actionResultReturnDTO.SetError(actionResultReturnDTORemoveLetters.UserMessage, actionResultReturnDTORemoveLetters.StackTraceMessage);
                                return actionResultReturnDTO;
                            }

                            listData = actionResultReturnDTORemoveLetters.ResultData;
                        }
                        else
                        {
                            fileStream.Write("No Existe");
                        }

                        fileStream.WriteLine();
                    }
                }

                FileInfo resultsFile = directoryInfo.GetFiles()
                                                    .Where(x => x.Name.Equals(Constants.ResultsFileName))
                                                    .FirstOrDefault();

                actionResultReturnDTO.ResultData = resultsFile;
            }
            catch (Exception ex)
            {
                actionResultReturnDTO.SetError("Ocurrió un error buscando los usuarios en la lista", ex.ToString());
            }

            return actionResultReturnDTO;
        }

        /// <summary>
        /// Remove letters from list letting read a letter once
        /// </summary>
        /// <param name="listData">List contents data</param>
        /// <param name="currentUser">Current user</param>
        /// <returns>ActionResultReturnDTO (T as IList (T as string)) with new list content data</returns>
        private ActionResultReturnDTO<IList<string>> RemoveLettersFromListData(IList<string> listData, string currentUser)
        {
            ActionResultReturnDTO<IList<string>> actionResultReturnDTO = new ActionResultReturnDTO<IList<string>>();

            try
            {
                if(listData.Count == 0)
                {
                    actionResultReturnDTO.ResultData = listData;
                    return actionResultReturnDTO;
                }

                for (int i = 0; i < currentUser.Length; i++)
                {
                    listData.RemoveAt(listData.IndexOf(currentUser[i].ToString()));
                }

                actionResultReturnDTO.ResultData = listData;
            }
            catch (Exception ex)
            {
                actionResultReturnDTO.SetError("Ocurrió un error buscando los usuarios en la lista", ex.ToString());
            }

            return actionResultReturnDTO;
        }
        #endregion
    }
}