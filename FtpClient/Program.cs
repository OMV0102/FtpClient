using System;
using System.Collections.Generic;
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
            while (true)
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
                password = "BeSwulj56";
                //ftp://pmi-b6603:BeSwulj50@fpm2.ami.nstu.ru

                FTPClient client = new FTPClient(host, user, password);
                try
                {
                    Console.WriteLine("Идет открытие соединения, пожалуйста подождите...");
                    client.openConnection();
                    if(client.getStatusConnection() == false)
                    {
                        Console.WriteLine("Не удалось открыть соединение по введенным параметрам!");
                        Console.WriteLine("Завершение работы(нажмите Enter)...");
                        Console.ReadLine();
                        Environment.Exit(1);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Произошла ошибка при открытии соединения!");
                    Console.WriteLine("Сообщение об ошибке:\n"+e.Message);
                    Console.WriteLine("Завершение работы(нажмите Enter)...");
                    Console.ReadLine();
                    Environment.Exit(1);
                }

                Program.printMenu();

                Console.WriteLine("[" + client.getCurrentDirectory() + "]");
                Console.Write("Выбор: ");
                string strNumber =  Console.ReadLine();
                int Number;
                bool resultNumber = Int32.TryParse(strNumber, out Number);
                if(resultNumber == false)
                {
                    Console.WriteLine("Введен некоректный номер пункта меню!");
                }
                else
                {
                    switch (Number)
                    {
                        case 1:
                        {
                            break;
                        }
                        case 2:
                        {
                            break;
                        }
                        case 3:
                        {
                            break;
                        }
                        case 4:
                        {
                            break;
                        }
                        case 5:
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
                        case 6:
                        {
                            break;
                        }
                        case 7:
                        {
                            break;
                        }
                        case 8:
                        {
                            break;
                        }
                        case 9:
                        {
                            break;
                        }
                        case 0:
                        {
                            client.closeConnection();
                            Environment.Exit(0);
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                }
                Console.WriteLine("\t\tДействие выполнено успешно");
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

        // Выбор файла
        public static bool Upload(string fileName, out string fileNameLocal)
        {
            string Name = fileName.Substring(fileName.LastIndexOf('/'));

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
    }
}
