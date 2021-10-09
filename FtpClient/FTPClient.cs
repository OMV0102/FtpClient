using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseDT.Net.Ftp;

namespace FtpClient
{
    public class FTPClient
    {
        private FTPConnection ftpConnection;

        public FTPClient(string host, string user, string password)
        {
            this.ftpConnection = new FTPConnection();
            this.ftpConnection.ServerAddress = host;
            this.ftpConnection.UserName = user;
            this.ftpConnection.Password = password;
            ftpConnection.AutoLogin = true;
        }

        public string ServerAddress
        {
            get
            {
                return this.ftpConnection.ServerAddress;
            }
        }

        public string UserName
        {
            get
            {
                return this.ftpConnection.UserName;
            }
        }

        public string Password
        {
            get
            {
                return this.ftpConnection.Password;
            }
        }

        // открыть соединение
        public void openConnection()
        {
            this.ftpConnection.Connect();
        }

        // закрыть соединение
        public void closeConnection()
        {
            this.ftpConnection.Close();
        }

        public bool getStatusConnection()
        {
            return this.ftpConnection.IsConnected;
        }

        // получить текущий каталог
        public string getCurrentDirectory()
        {
            return this.ftpConnection.ServerDirectory;
        }

        // получить список файлов в текущем каталоге
        public string[] getListFiles()
        {
            return this.ftpConnection.GetFiles(this.getCurrentDirectory(), true);
        }

        // получить список файлов в указанном каталоге
        public string[] getListFiles(string Directory)
        {
            if (Directory.Length < 1)
                return new string[0];

            return this.ftpConnection.GetFiles(Directory, true);
        }

        // сменить текущий каталог
        public bool changeDirectory(string newDirectory)
        {
            if (!newDirectory.Contains("./")) 
            {
                newDirectory = this.getCurrentDirectory() + "/" + newDirectory;
            }
            return this.ftpConnection.ChangeWorkingDirectory(newDirectory);
        }

        //сменить текущий каталог на родительский
        public bool changeDirrectoryOnParent()
        {
            return this.ftpConnection.ChangeWorkingDirectoryUp();
        }

        // создать каталог в текущем каталоге
        public bool createDirectory(string nameNewDirectory)
        {
            if (nameNewDirectory.Length < 1)
                return false;
            this.ftpConnection.CreateDirectory(nameNewDirectory);
            return true;
        }

        //удалить указанный каталог
        public bool deleteDirectory(string Directory)
        {
            if (Directory.Length < 1)
                return false;

            this.ftpConnection.DeleteDirectory(Directory);

            return true;
        }

        //удалить указанный файл
        public bool deleteFile(string remoteFile)
        {
            if (remoteFile.Length < 1)
                return false;

            return this.ftpConnection.DeleteFile(remoteFile);
        }

        //загрузка файла на сервер в текущую директорию
        public bool UploadFile(string localFile)
        {
            if (localFile.Length < 1)
                return false;

            string CurrentDirectory = this.getCurrentDirectory();
            this.ftpConnection.UploadFile(localFile, CurrentDirectory, false); // false - перезаписать файл

            return true;
        }

        //загрузка файла на сервер по указаному пути
        public bool UploadFile(string localFile, string remoteFile)
        {
            if (localFile.Length < 1 || remoteFile.Length < 1)
                return false;

            //if (!remoteFile.Contains("./"))
            //    remoteFile = this.getCurrentDirectory() + "/"+ remoteFile;

            this.ftpConnection.UploadFile(localFile, remoteFile); // перезаписать файл
            return true;
        }

        //загрузка байтов файла на сервер по указанному пути
        public bool UploadFile(byte[] localFileByte, string remoteFile)
        {
            if (localFileByte.Length < 1 || remoteFile.Length < 1)
                return false;

            //if (!remoteFile.Contains("./"))
            //    remoteFile = this.getCurrentDirectory() + "/" + remoteFile;

            this.ftpConnection.UploadByteArray(localFileByte, remoteFile); // перезаписать файл
            return true;
        }

        //получение указанного файла с сервера
        public bool DownloadFile(string localFile, string remoteFile)
        {
            if (localFile.Length < 1 || remoteFile.Length < 1)
                return false;

            if (!remoteFile.Contains("./"))
                remoteFile = this.getCurrentDirectory() + "/" + remoteFile;

            this.ftpConnection.DownloadFile(localFile, remoteFile);
            return true;
        } 

        //переименовать файл
        public bool RenameFile(string oldName, string newName)
        {
            if (oldName.Length < 1 || newName.Length < 1)
                return false;
            if (!oldName.Contains("./"))
            {
                oldName = this.getCurrentDirectory() + "/" + oldName;
            }
            if (!newName.Contains("./"))
            {
                newName = this.getCurrentDirectory() + "/" + newName;
            }
            this.ftpConnection.RenameFile(oldName, newName);
            return true;
        }

        // проверить существование файла
        public bool CheckExistFile(string remoteFile)
        {
            if (remoteFile.Length < 1)
                return false;

            return this.ftpConnection.Exists(remoteFile);
        }

        // проверить существование каталога
        public bool CheckExistDirectory(string remoteDirectory)
        {
            if (remoteDirectory.Length < 1)
                return false;

            return this.ftpConnection.DirectoryExists(remoteDirectory);
        }
    }
}
