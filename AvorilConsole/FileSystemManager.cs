using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AvorilConsole
{
    public static class FileSystemManager
    {
        public static string BaseEnemiesPath { get => GetMainDirectory() + "Content\\" + "BaseEnemies\\"; }

        /// <summary>
        /// Получение полной строки вызова (.exe)
        /// </summary>
        /// <returns></returns>
        public static string GetFullExecutionPath()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

        /// <summary>
        /// Получение пути к основной директории
        /// </summary>
        /// <returns></returns>
        public static string GetMainDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\";
        }

        /// <summary>
        /// Проверка на наличие всех игровых каталогов
        /// </summary>
        public static void CheckAllFoldersExistance()
        {
            var md = GetMainDirectory();
            
            if (Directory.Exists(md+"Content") == false)
                throw new System.Exception("Direcory: Content does not exist");
            var content = md + "Content\\";

            if (Directory.Exists(content + "BaseEnemies") == false)
                throw new System.Exception("Direcory: BaseEnemies does not exist");
        }

    }
}
