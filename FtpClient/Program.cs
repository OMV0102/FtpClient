using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            string host = "";
            string user = "";
            string password = "";
            Console.WriteLine("Введите адрес сервера, имя пользователя и пароль для подключения к серверу FTP");
            /*Console.Write("Сервер: ");
            host = Console.ReadLine();
            Console.Write("Пользователь: ");
            user = Console.ReadLine();
            Console.Write("Пароль: ");
            password = Console.ReadLine();*/

            host = "fpm2.ami.nstu.ru";
            user = "pmi-b6603";
            password = "BeSwulj5";
            //ftp://pmi-b6603:BeSwulj50@fpm2.ami.nstu.ru

            FTPClient client = new FTPClient(host, user, password);
            try
            {
                Console.WriteLine("Идет открытие соединения, пожалуйста подождите...");
                client.openConnection();
                if (client.getStatusConnection() == false)
                {
                    Console.WriteLine("Не удалось открыть соединение по введенным параметрам!");
                    Console.WriteLine("Завершение работы(нажмите Enter)...");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка при открытии соединения!");
                Console.WriteLine("Сообщение об ошибке:\n" + e.Message);
                Console.WriteLine("Завершение работы(нажмите Enter)...");
                Console.ReadLine();
                Environment.Exit(1);
            }
            while (true)
            {
                Program.printMenu();

                Console.WriteLine("[" + client.getCurrentDirectory() + "]");
                Console.Write("Выбор: ");
                string strNumber = Console.ReadLine();
                int Number;
                bool resultNumber = Int32.TryParse(strNumber, out Number);
                if (resultNumber == false)
                {
                    Console.WriteLine("Введен некоректный номер пункта меню!");
                }
                else
                {
                    switch (Number)
                    {
                        case 1: // Создать каталог
                        {
                            Console.Write("Введите имя нового каталога: ");
                            string nameDirectory = Console.ReadLine();
                            string fullPath = client.getCurrentDirectory() + "/" + nameDirectory;
                            bool result = true;
                            if (!client.CheckExistDirectory(fullPath))
                            {
                                result = client.createDirectory(fullPath); // создали в текущем каталоге
                                if (!result)
                                    Console.WriteLine("Произошла ошибка при создании нового каталога!");
                                else
                                    Console.WriteLine("Создан новый каталог: " + fullPath);
                            }
                            else
                            {
                                Console.WriteLine("Каталог с таким именем уже существует!");
                            }

                            break;
                        }
                        case 2: // Удалить каталог
                        {
                            Console.Write("Введите имя удаляемого каталога: ");
                            string nameDirectory = Console.ReadLine();
                            string fullPath = client.getCurrentDirectory() + "/" + nameDirectory;
                            bool result = true;
                            if (client.CheckExistDirectory(fullPath))
                            {
                                result = client.deleteDirectory(fullPath); // удалили
                                if (!result)
                                    Console.WriteLine("Произошла ошибка при удалении указаного каталога!");
                                else
                                    Console.WriteLine("Каталог [" + fullPath + "] удален.");
                            }
                            else
                            {
                                Console.WriteLine("Каталога с таким именем НЕ существует!");
                            }

                            break;
                        }
                        case 3: // Перейти в родительский каталог
                        {
                            bool result = true;
                            client.changeDirrectoryOnParent();
                            if (!result)
                                Console.WriteLine("Произошла ошибка при переходе в родительский каталог!");
                            else
                                Console.WriteLine("Вы перешли в каталог " + client.getCurrentDirectory() + ".");
                            break;
                        }
                        case 4: // Перейти в указаный каталог
                        {
                            Console.Write("Введите имя каталога для перехода: ");
                            string nameDirectory = Console.ReadLine();
                            string fullPath = client.getCurrentDirectory() + "/" + nameDirectory;
                            bool result = true;
                            if (client.CheckExistDirectory(fullPath))
                            {
                                result = client.changeDirectory(fullPath); // создали в текущем каталоге
                                if (!result)
                                    Console.WriteLine("Произошла ошибка при переходе в указанный каталог!");
                                else
                                    Console.WriteLine("Вы перешли в каталог " + client.getCurrentDirectory() + ".");
                            }
                            else
                            {
                                Console.WriteLine("Каталога с таким именем НЕ существует!");
                            }
                            break;
                        }
                        case 5: //Показать содержимое текущего каталога
                        {
                            string[] listFiles;
                            listFiles = client.getListFiles();
                            if (listFiles.Length == 0)
                            {
                                Console.WriteLine("Текущий каталог пуст!");
                            }
                            else
                            {
                                Console.WriteLine("\tСодержимое текущего каталога:");
                                foreach (string str in listFiles)
                                {
                                    Console.WriteLine(str);
                                }
                            }

                            break;
                        }
                        case 6: //Получить указаный файл
                        {
                            bool result = true;
                            string fileNameLocal = "";
                            string directoryLocal = "";
                            string fileNameRemote = "";
                            byte[] fileLocalByte;
                            Console.WriteLine("Введите имя файла или его абсолютный путь:");
                            fileNameRemote = Console.ReadLine();
                            if (fileNameRemote.Contains("./"))
                            {
                                fileNameRemote = client.getCurrentDirectory() + "/" + fileNameRemote.Substring(fileNameRemote.LastIndexOf("./") + 1);
                            }
                            else if (!fileNameRemote.Contains("/"))
                            {
                                fileNameRemote = client.getCurrentDirectory() + "/" + fileNameRemote;
                            }

                            if (client.CheckExistFile(fileNameRemote))
                            {
                                result = Program.forDownload(out directoryLocal);

                                if (result) // место выбрано
                                {
                                    fileNameLocal = directoryLocal + "\\" + fileNameRemote.Substring(fileNameRemote.LastIndexOf("/") + 1);
                                    result = client.DownloadFile(fileNameLocal, fileNameRemote);
                                    if (result && File.Exists(fileNameLocal))
                                        Console.WriteLine("Файл [" + fileNameRemote + "] скачан и успешно сохранен в [" + fileNameLocal + "].");
                                    else
                                        Console.WriteLine("Произошла ошибка при скачивании и сохранении файла [" + fileNameRemote + "] в [" + fileNameLocal + "].!");
                                }
                                else
                                {
                                    Console.WriteLine("Не был выбран путь для сохранения! Скачанный файл ["+ fileNameRemote + "] не сохранен.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Указанного файла c именем [" + fileNameRemote + "] НЕ существует!");
                            }

                            break;
                        }
                        case 7: //Загрузить файл на сервер
                        {
                            bool result = true;
                            string fileNameLocal;
                            string fileNameRemote;
                            result = Program.forUpload(out fileNameLocal);
                            if (result)
                            {
                                if (File.Exists(fileNameLocal))
                                {
                                    if (new FileInfo(fileNameLocal).Length > 0)
                                    {
                                        fileNameRemote = client.getCurrentDirectory() + "/" + fileNameLocal.Substring(fileNameLocal.LastIndexOf("\\") + 1);
                                        result = client.UploadFile(fileNameLocal, fileNameRemote); // загрузили
                                                                                                   //fileLocalByte = File.ReadAllBytes(fileNameLocal);
                                                                                                   //result = client.UploadFile(fileLocalByte, fileNameRemote); // загрузили байтами
                                        if (result)
                                            Console.WriteLine("Файл [" + fileNameLocal + "] загружен в [" + fileNameRemote + "].");
                                        else
                                            Console.WriteLine("Произошла ошибка при загрузке файла на сервер!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Локального файла c именем [" + fileNameLocal + "] имеет нулевой размер!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Локального файла c именем [" + fileNameLocal + "] НЕ существует!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Не был выбран файл для его хагрузки на сервер!");
                            }
                            break;
                        }
                        case 8: //Удалить указаный файл
                        {
                            Console.Write("Введите имя удаляемого файла: ");
                            string nameFile = Console.ReadLine();
                            string fullPath = client.getCurrentDirectory() + "/" + nameFile;
                            bool result = true;
                            if (client.CheckExistFile(fullPath))
                            {
                                result = client.deleteFile(fullPath); // удалили
                                if (!result)
                                    Console.WriteLine("Произошла ошибка при удалении указаного файла!");
                                else
                                    Console.WriteLine("Файл [" + fullPath + "] удален.");
                            }
                            else
                            {
                                Console.WriteLine("Файла с таким именем НЕ существует!");
                            }
                            break;
                        }
                        case 9: //Переименовать указаный файл
                        {
                            Console.Write("Введите текущее имя файла: ");
                            string nameFileOld = Console.ReadLine();
                            string fullPathOld = client.getCurrentDirectory() + "/" + nameFileOld;
                            Console.Write("Введите новое имя текущего файла для переименования: ");
                            string nameFileNew = Console.ReadLine();
                            string fullPathNew = client.getCurrentDirectory() + "/" + nameFileNew;
                            bool result = true;
                            if (client.CheckExistFile(fullPathOld))
                            {
                                result = client.RenameFile(fullPathOld, fullPathNew); // переименовали
                                if (!result)
                                    Console.WriteLine("Произошла ошибка при переименовании файла [" + fullPathOld + "] !");
                                else
                                    Console.WriteLine("Файл [" + fullPathOld + "] переменован в [" + fullPathNew + "].");
                            }
                            else
                            {
                                Console.WriteLine("Файла с таким именем НЕ существует!");
                            }
                            break;
                        }
                        case 0: //Выход
                        {
                            client.closeConnection();
                            Environment.Exit(0);
                            break;
                        }
                        default:
                        {
                            Console.WriteLine("Введен некоректный номер пункта меню!");
                            break;
                        }
                    }
                }
                //Console.WriteLine("\t\tДействие выполнено успешно");
                Console.WriteLine("=========================================================");
            }
        }

        // Вывод меню
        public static void printMenu()
        {
            string strMenu = "Меню:" + Environment.NewLine;
            strMenu += "<1> Создать каталог" + Environment.NewLine;
            strMenu += "<2> Удалить каталог" + Environment.NewLine;
            strMenu += "<3> Перейти в родительский каталог" + Environment.NewLine;
            strMenu += "<4> Перейти в указаный каталог" + Environment.NewLine;
            strMenu += "<5> Показать содержимое текущего каталога" + Environment.NewLine;
            strMenu += "<6> Получить указаный файл" + Environment.NewLine;
            strMenu += "<7> Загрузить файл на сервер" + Environment.NewLine;
            strMenu += "<8> Удалить указаный файл" + Environment.NewLine;
            strMenu += "<9> Переименовать указаный файл" + Environment.NewLine;
            strMenu += "<0> Выход" + Environment.NewLine;

            Console.Write(strMenu);
        }

        // Выбор файла для загрузки на сервер
        public static bool forUpload(out string fileNameLocal)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите файл ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // путь откуда запустили
            //ofd.Filter = "Text documents (*.txt)|*.txt";
            ofd.Multiselect = false;

            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                fileNameLocal = ofd.FileName;
                return true;
            }
            else
            {
                fileNameLocal = "";
                return false;
            }
        }

        // Выбор файла для скачивания с сервера
        public static bool forDownload(out string directoryLocal)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "Выберите каталог для сохранения скачанного файла с сервера.";
            //fbd.RootFolder = Environment.SpecialFolder.ApplicationData;

            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                directoryLocal = fbd.SelectedPath;
                return true;
            }
            else
            {
                directoryLocal = "";
                return false;
            }
        }
    }
}
