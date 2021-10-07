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

            }


            Console.ReadLine();
        }

        public void printMenu()
        {
            string strMenu = "";
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
