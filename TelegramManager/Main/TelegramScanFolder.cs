using System;
using System.Collections.Generic;
using System.IO;

namespace TelegramManager.Main
{
    public static class TelegramScanFolder
    {
        public static List<string> GetTelegramFolders(string rootPath)
        {
            var telegramFolders = new List<string>();

            try
            {
                // Получить все директории в корневой папке
                string[] directories = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);

                // Найти папки, содержащие файл Telegram.exe
                foreach (var dir in directories)
                {
                    string telegramPath = Path.Combine(dir, "Telegram.exe");
                    if (File.Exists(telegramPath))
                    {
                        telegramFolders.Add(dir);
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем или пробрасываем исключение
                throw new Exception($"Ошибка при сканировании папок: {ex.Message}", ex);
            }

            return telegramFolders;
        }
    }
}
