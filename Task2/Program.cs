using System;
using System.IO;

namespace Task2
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string path = Task1.Program.GetDirectoryPath();

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (directoryInfo.Exists)
                {
                    long size = CalculateDirectorySize(directoryInfo);
                    Console.WriteLine($"Папка: \"{directoryInfo.FullName}\"");
                    Console.WriteLine($"Размер: {size} байт");
                }
                else
                {
                    Console.WriteLine(directoryInfo.FullName);
                    Console.WriteLine("Папка не найдена.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }

        public static long CalculateDirectorySize(DirectoryInfo dir)
        {
            long size = 0;
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                size += file.Length;
            }

            DirectoryInfo[] directories = dir.GetDirectories();
            foreach (DirectoryInfo directory in directories)
            {
                size += CalculateDirectorySize(directory);
            }
            return size;
        }
    }
}
