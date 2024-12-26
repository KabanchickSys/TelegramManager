using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TelegramManager.Main;
using System.Diagnostics;

namespace TelegramFolderScanner
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        const uint SWP_NOZORDER = 0x0004; // Не изменять порядок окон
        const uint SWP_SHOWWINDOW = 0x0040; // Отображать окно

        private List<string> scannedTelegramFolders = new List<string>(); // Список отсканированных папок

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            this.scrollablePanel.Controls.Clear();
            scannedTelegramFolders = TelegramScanFolder.ScanFoldersWithDialog();
            int yPosition = 10;
            for (int i = 0; i < scannedTelegramFolders.Count; i++)
            {
                string folder = scannedTelegramFolders[i];
                Button folderButton = new Button
                {
                    Text = $"{i}: {Path.GetFileName(folder)}",
                    Tag = new { Id = i, Path = folder },
                    Size = new System.Drawing.Size(350, 30),
                    Location = new System.Drawing.Point(10, yPosition)
                };
                folderButton.Click += (s, args) =>
                {
                    var tag = (dynamic)folderButton.Tag;
                    OpenFolder(tag.Path, tag.Id);
                };
                this.scrollablePanel.Controls.Add(folderButton);
                yPosition += 40;
            }

            if (scannedTelegramFolders.Count == 0)
            {
                MessageBox.Show("Папки с Telegram.exe не найдены.", "Результаты поиска",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Добавить перенос на другую строку
        private int nextTelegramX = 0;

        private void OpenFolder(string folderPath, int id)
        {
            string telegramExePath = Path.Combine(folderPath, "Telegram.exe");

            if (File.Exists(telegramExePath))
            {
                //int windowWidth = 380;
                //int windowHeight = 500;
                //int windowY = 0;
                try
                {
                    var process = System.Diagnostics.Process.Start(telegramExePath);
                    //System.Threading.Thread.Sleep(300);
                    //SetTelegramWindowSize("Telegram", windowWidth, windowHeight, nextTelegramX, windowY, telegramExePath);
                    //nextTelegramX = windowWidth*id; // Учитываем ширину окна и отступ
                }
                catch (Exception)
                {
                }
            }
            else
            {
                MessageBox.Show($"Не найден файл Telegram.exe в папке: {folderPath}",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void SetTelegramWindowSize(string processName, int width, int height, int posX, int posY, string exePath)
        {
            // Получаем все процессы с указанным именем
            var processes = Process.GetProcessesByName(processName);

            foreach (var process in processes)
            {
                try
                {
                    // Проверяем, соответствует ли путь исполняемого файла указанному пути
                    if (process.MainModule?.FileName == exePath)
                    {
                        // Устанавливаем размеры и положение окна
                        IntPtr hWnd = process.MainWindowHandle;
                        if (hWnd != IntPtr.Zero)
                        {
                            MoveWindow(hWnd, posX, posY, width, height, true);
                        }
                        break; // Найденный процесс обработан, выходим
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обработке процесса: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnOpenAll_Click(object sender, EventArgs e)
        {
            if (scannedTelegramFolders.Count > 0)
            {
                int id = 0;
                foreach (var folder in scannedTelegramFolders)
                {
                    OpenFolder(folder, id); 
                    System.Threading.Thread.Sleep(300);
                    id++;
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выполните сканирование для поиска папок с Telegram.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnPlaceAll_Click(object sender, EventArgs e)
        {

        }
    }
}
