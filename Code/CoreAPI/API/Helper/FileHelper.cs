using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Helper
{
    public class FileHelper
    {
        private  readonly string baseFolder = string.Empty;

        
        public FileHelper(IWebHostEnvironment hostingEnvironment)
        {
            baseFolder = hostingEnvironment.ContentRootPath;
        }

        public  bool checkFolderExists(string path)
        {

            return Directory.Exists( path);

        }
        public bool checkFileExists(string path)
        {
            string tmp = path;

            return File.Exists( tmp);

        }
        public bool IsDirectoryEmpty(string path)
        {

            if (checkFolderExists( path))
            {
                DirectoryInfo dirinfo = new DirectoryInfo( path);
                long siz = dirinfo.GetFiles().Sum(file => file.Length);
                return Convert.ToBoolean(siz);
            }
            else
            {
                return false;
            }

        }

        public Int32 countFile(string path)
        {
            if (checkFolderExists( path))
            {
                int fileCount = Directory.GetFiles( path).Length;
                return fileCount;
            }
            else
            {
                return -1;
            }
        }
        public bool createDirectory(string path)
        {
            bool b = false;

            try
            {
                if (!checkFolderExists(path))
                {
                    Directory.CreateDirectory(path);
                    b = true;
                }
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;

        }

        public void deleteFile(string path)
        {
            

            string tmp = path;
            if (File.Exists(tmp))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(tmp);
               
            }

        }
        public void deleteFolder(string path)
        {
            string tmp = path;
            if (checkFolderExists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                di.Delete();
            }


        }
        public void deleteFolder(string path, bool includesubfolders)
        {
            string tmp = path;
            if (checkFolderExists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                di.Delete(includesubfolders);
            }


        }
        public string SystemGeneratedFileName()
        {

            return Guid.NewGuid().ToString().ToUpper();
        }

    }
}
