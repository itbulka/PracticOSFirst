using System;
using System.IO;
using System.Threading;
using System.IO.Compression;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FirstPractic
{
    class Edward
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Band { get; set; }
        public string Style { get; set; }
    }
    class Program
    {
        static void showDiskInfo()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Свободное пространство: {drive.AvailableFreeSpace}");
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}\n");
                }
            }
        }
        public static void creatFillFile()
        {
            string path = @"/Users/Vladimir/Documents/TestPracticOS/haha.txt";
            if (File.Exists(path)) File.Delete(path);
            using (StreamWriter fl = File.CreateText(path))
            {
                Console.Write("Введите строку для записи в файл: ");
                string str = Console.ReadLine();
                fl.WriteLineAsync(str);
                fl.Close();
            }
            using (StreamReader fa = File.OpenText(path))
            {
                string s;
                while ((s = fa.ReadLine()) != null)
                    Console.WriteLine(s);
            }
            FileInfo fileInfo = new FileInfo(path);
            Console.WriteLine("   Имя файла: {0};", fileInfo.Name);
            Console.WriteLine("   Время создания: {0};\n", fileInfo.CreationTime);

            deleteFile(path);
        }
        static async Task jsonFilesTask()
        {
            using (FileStream fs = new FileStream("programmist.json", FileMode.OpenOrCreate))
            {
                Edward edward = new Edward() { Name = "Эдуард", Age = 25, Band = "itteam", Style = "Программиста" };
                await JsonSerializer.SerializeAsync<Edward>(fs, edward);
            }
            using (FileStream fs = new FileStream("programmist.json", FileMode.OpenOrCreate))
            {
                Edward edw = await JsonSerializer.DeserializeAsync<Edward>(fs);
                Console.WriteLine($"Name: {edw.Name}; Age: {edw.Age}; Band: {edw.Band}; Style: {edw.Style}");
            }
            string path = @"/Users/Vladimir/Documents/TestPracticOS/programmist.json";
            deleteFile(path);
        }
        public static void xmlTask()
        {
            XDocument xdoc = new XDocument();

            XElement singer = new XElement("singer");
            Console.Write("Введите имя: ");
            string str = Console.ReadLine();

            XAttribute singerNameAttr = new XAttribute("name", str);
            Console.Write("Введите банду: ");
            str = Console.ReadLine();

            XElement singerBandElem = new XElement("band", str);
            Console.Write("Введите возраст: ");
            str = Console.ReadLine();

            XElement singerAgeElem = new XElement("age", str);
            singer.Add(singerNameAttr);
            singer.Add(singerBandElem);
            singer.Add(singerAgeElem);

            XElement singers = new XElement("singers");
            singers.Add(singer);
            xdoc.Add(singers);

            xdoc.Save("singers.xml");

            foreach (XElement s in xdoc.Element("singers").Elements("singer"))
            {
                XAttribute nameAttribute = s.Attribute("name");
                XElement bandElement = s.Element("band");
                XElement ageElement = s.Element("age");

                if (nameAttribute != null && bandElement != null && ageElement != null)
                {
                    Console.WriteLine($"   Name: {nameAttribute.Value}");
                    Console.WriteLine($"   Band: {bandElement.Value}");
                    Console.WriteLine($"   Age: {ageElement.Value}");
                }
                Console.WriteLine();
            }
            deleteFile("singers.xml");
        }
        public static void zipTask()
        {
            string path = @"/Users/Vladimir/Documents/TestPracticOS\test\hey.txt";
            string sourceFolder = @"/Users/Vladimir/Documents/TestPracticOS\test";
            string zipFile = @"/Users/Vladimir/Documents/TestPracticOS\test.zip";
            string targetFolder = @"/Users/Vladimir/Documents/TestPracticOS\newtest";

            DirectoryInfo dirInf = new DirectoryInfo(sourceFolder);
            if (!dirInf.Exists) dirInf.Create();
            if (File.Exists(path)) File.Delete(path);
            using (StreamWriter fl = File.CreateText(path))
            {
                Console.Write("Введите строку: ");
                string str = Console.ReadLine();
                fl.WriteLineAsync(str);
                fl.Close();
            }
            using (StreamReader fa = File.OpenText(path))
            {
                string s;
                while ((s = fa.ReadLine()) != null)
                    Console.WriteLine(s);
            }
            
            FileInfo fileInfo = new FileInfo(path);
            Console.WriteLine("Имя файла: {0}", fileInfo.Name);
            Console.WriteLine("Время создания: {0}", fileInfo.CreationTime);
            Console.WriteLine("Размер: {0}\n", fileInfo.Length);

            ZipFile.CreateFromDirectory(sourceFolder, zipFile);
            ZipFile.ExtractToDirectory(zipFile, targetFolder);

            FileInfo fileInfo1 = new FileInfo(zipFile);
            Console.WriteLine("Имя файла: {0}", fileInfo1.Name);
            Console.WriteLine("Время создания: {0}", fileInfo1.CreationTime);
            Console.WriteLine("Размер: {0}\n", fileInfo1.Length);

            deleteFile(zipFile);
            deleteFile(path);
            deleteFile(targetFolder + @"\hey.txt");
            deleteFolder(sourceFolder, "test");
            deleteFolder(targetFolder, "newtest");
        }
        static void Main(string[] args)
        {
            bool f = false;
            Console.WriteLine("1.Создание файла и заполнение");
            Console.WriteLine("2.Информация о дисках");
            Console.WriteLine("3.Сериализовать объект");
            Console.WriteLine("4.Сделать xml файл");
            Console.WriteLine("5.Сжатие и распаковка файлов");
            Console.WriteLine("6.Выход");
            while (!f)
            {
                Console.Write("   Enter number: ");
                int x = Convert.ToInt32(Console.ReadLine());
                switch (x)
                {
                    case 1:
                        creatFillFile();
                        break;
                    case 2:
                        showDiskInfo();
                        break;
                    case 3:
                        var task3 = jsonFilesTask();
                        task3.Wait();
                        break;
                    case 4:
                        xmlTask();
                        break;
                    case 5:
                        zipTask();
                        break;
                    case 6:
                        f = true;
                        break;
                    default:
                        Console.WriteLine("Ошибка!");
                        break;
                }
            }
        }
        static void deleteFile(string path)
        {
            Console.WriteLine("Удалить файл?[y/n]");
            string str = Console.ReadLine();
            if (str.CompareTo("y") == 0)
            {
                FileInfo fileinfo = new FileInfo(path);
                if (fileinfo.Exists)
                {
                    fileinfo.Delete();
                }
                Console.WriteLine("Файл удален\n");
            }
        }
        static void deleteFolder(string path, string name)
        {
            Console.WriteLine($"Удалить папку {name}?[y/n]");
            string str = Console.ReadLine();
            if (str.CompareTo("y") == 0)
            {
                DirectoryInfo dirinf = new DirectoryInfo(path);
                if (dirinf.Exists)
                {
                    dirinf.Delete();
                }
                Console.WriteLine($"{name} папка удалена\n");
            }
        }
    }
}