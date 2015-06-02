


namespace StoreAppTest.Web
{
    using System;
    using System.Web;
    using System.IO;

    /// <summary>
    /// Сводное описание для UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        string FileName { get; set; }
        string FilePath { get; set; }
        int PackageCount { get; set; }
        int PackageNumber { get; set; }

        /// <summary>
        /// Разрешает обработку веб-запросов НТТР для пользовательского элемента HttpHandler, который реализует интерфейс <see cref="T:System.Web.IHttpHandler"/>.
        /// </summary>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/>, предоставляющий ссылки на внутренние серверные объекты (например, Request, Response, Session и Server), используемые для обслуживания HTTP-запросов.</param>
        public void ProcessRequest(HttpContext context)
        {
            ProcessQueryString(context);
            ProcessFile(context);
        }

        public bool IsReusable {
            get { return false; }
        }

        void ProcessQueryString(HttpContext context)
        {
            var query = context.Request.QueryString;
            FilePath = Uri.UnescapeDataString(query["filePath"]);
            FileName = Uri.UnescapeDataString(query["fileName"]);
            PackageCount = int.Parse(query["packageCount"]);
            PackageNumber = int.Parse(query["packageNumber"]);
        }

        void ProcessFile(HttpContext context)
        {
            string serverFileName = GetServerPath(context.Server, FilePath, FileName);
            FileMode fileMode = File.Exists(serverFileName) && PackageNumber > 0
                ? FileMode.Append
                : FileMode.Create;
            using (BinaryReader reader = new BinaryReader(context.Request.InputStream))
            using (BinaryWriter writer = new BinaryWriter(File.Open(serverFileName, fileMode)))
            {
                byte[] buffer = new byte[4096];
                int byteRead;
                while ((byteRead = reader.Read(buffer, 0, buffer.Length)) != 0)
                {
                   writer.Write(buffer, 0, byteRead); 
                }
            }

        }

        private string GetServerPath(HttpServerUtility server, string filePath, string fileName)
        {

            return server.MapPath(Path.Combine(filePath, Path.GetFileName(fileName)));

        }
    }
}