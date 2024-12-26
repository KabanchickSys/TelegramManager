using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TelegramManager.Main
{
    public static class TelegramScanFolder
    {
        public static List<string> GetTelegramFolders(string rootPath)
        {
            var telegramFolders = new List<string>();

            try
            {
                string[] directories = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
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
                throw new Exception($"Ошибка при сканировании папок: {ex.Message}", ex);
            }

            return telegramFolders;
        }

        public static List<string> ScanFoldersWithDialog()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string rootPath = folderDialog.SelectedPath;
                    return GetTelegramFolders(rootPath);
                }
            }

            return new List<string>();
        }
    }
}
