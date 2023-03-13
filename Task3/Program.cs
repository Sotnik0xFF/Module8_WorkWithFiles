using System;
using System.IO;

namespace Task3
{
    internal class Program
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
                    Console.WriteLine($"Папка: \"{directoryInfo.FullName}\"");
                    Console.WriteLine($"Исходный размер папки: {Task2.Program.CalculateDirectorySize(directoryInfo)} байт");
                    var clearingDetails = Task1.Program.ClearDirectory(directoryInfo);
                    Console.WriteLine($"Удалено файлов: {clearingDetails.FilesCount}");
                    Console.WriteLine($"Удалено папок: {clearingDetails.FoldersCount}");
                    Console.WriteLine($"Освобождено: {clearingDetails.Size}");
                    Console.WriteLine($"Текущий размер папки: {Task2.Program.CalculateDirectorySize(directoryInfo)} байт");
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
    }
}
