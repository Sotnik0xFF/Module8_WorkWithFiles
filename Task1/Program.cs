using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Task1
{
    public class Program
    {
        const int MinutesCount = 30;
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string path = GetDirectoryPath();

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (!directoryInfo.Exists ) 
                {
                    Console.WriteLine(path);
                    Console.WriteLine("Папка не найдена.");
                    return;
                }
                var details = ClearDirectory(directoryInfo);
                Console.WriteLine("Очистка завершена");
                Console.WriteLine($"Папка: {directoryInfo.FullName}");
                Console.WriteLine($"Удалено файлов: {details.FilesCount}");
                Console.WriteLine($"Удалено папок: {details.FoldersCount}");
                Console.WriteLine(Environment.NewLine + "*******ЖУРНАЛ*******");
                Console.WriteLine(details.Log);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }

        public static string GetDirectoryPath()
        {
            string path = GetPathFromArgs();
            if (String.IsNullOrEmpty(path))
            {
                path = GetPathFromConsole();
            }
            return path.Trim('\"');
        }

        public static (int FilesCount, int FoldersCount, long Size, string Log) ClearDirectory(DirectoryInfo directoryInfo)
        {
            TimeSpan lastAccessSpan = TimeSpan.FromMinutes(MinutesCount);
            DateTime nowDateTime = DateTime.Now;
            int filesCount = 0;
            int foldersCount = 0;
            long filesSize = 0;
            StringBuilder logBuilder = new StringBuilder();

            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                TimeSpan fileLastAccessSpan = nowDateTime - file.LastAccessTime;
                if (fileLastAccessSpan >= lastAccessSpan)
                {
                    filesCount++;
                    filesSize += file.Length;
                    file.Delete();
                    logBuilder.AppendLine($"Файл удален - \"{file.FullName}\" (не использовался {fileLastAccessSpan.Minutes} минут)");
                }
                else
                {
                    logBuilder.AppendLine($"Файл сохренен - \"{file.FullName}\" (не использовался {fileLastAccessSpan.Minutes} минут)");
                }
            }

            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                var clearingDetails = ClearDirectory(dir);
                filesCount += clearingDetails.FilesCount;
                foldersCount += clearingDetails.FoldersCount;
                filesSize += clearingDetails.Size;
                logBuilder.Append(clearingDetails.Log);

                TimeSpan dirLastAccessSpan = nowDateTime - dir.LastAccessTime;
                if (dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0 && dirLastAccessSpan >= lastAccessSpan)
                {
                    foldersCount++;
                    dir.Delete(true);
                    logBuilder.AppendLine($"Папка удалена - \"{dir.FullName}\" (не использовалась {dirLastAccessSpan.Minutes} минут)");
                }
                else
                {
                    logBuilder.AppendLine($"Папка сохранена - \"{dir.FullName}\" (не использовалась {dirLastAccessSpan.Minutes} минут)");
                }
            }
            return (filesCount, foldersCount, filesSize, logBuilder.ToString());
        }

        private static string GetPathFromConsole()
        {
            Console.Write("Укажите путь к очищаемой папке: ");
            return Console.ReadLine();
        }

        private static string GetPathFromArgs() 
        {
            string[] args = Environment.GetCommandLineArgs();
            return args.Length > 1 ? args[1] : String.Empty;
        }
    }
}
