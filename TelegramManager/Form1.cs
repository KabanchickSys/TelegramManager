using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TelegramManager.Main;
using System.Diagnostics;
using System.Linq;

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
                    OpenFolder(tag.Path);
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

        private void OpenFolder(string folderPath)
        {
            string telegramExePath = Path.Combine(folderPath, "Telegram.exe");

            if (File.Exists(telegramExePath))
            {
                try
                {
                    var process = System.Diagnostics.Process.Start(telegramExePath);
                }
                catch (Exception)
                {
                    MessageBox.Show("Невозможно открыть Telegram",
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var processes = Process.GetProcessesByName(processName);
            foreach (var process in processes)
            {
                try
                {
                    if (process.MainModule?.FileName == exePath)
                    {
                        IntPtr hWnd = process.MainWindowHandle;
                        if (hWnd != IntPtr.Zero)
                        {
                            MoveWindow(hWnd, posX, posY, width, height, true);
                        }
                        break;
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
                    OpenFolder(folder); 
                    System.Threading.Thread.Sleep(200);
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
            if (scannedTelegramFolders.Count > 0)
            {
                int windowWidth = 380;  
                int windowHeight = 500;
                int xOffset = 0;       
                int yOffset = 0; 
                const string processName = "Telegram";
                int screenWidth = Screen.PrimaryScreen.Bounds.Width;

                try
                {
                    foreach (var folder in scannedTelegramFolders)
                    {
                        try
                        {
                            if(xOffset + windowWidth > screenWidth)
                            {
                                xOffset = 0;
                                yOffset += windowHeight + 2;
                            }
                            if (yOffset > windowHeight + 2)
                            {
                                xOffset = 0;
                                yOffset = 0;
                            }
                            SetTelegramWindowSize(processName, windowWidth, windowHeight, xOffset, yOffset, Path.Combine(folder, "Telegram.exe"));
                            xOffset += windowWidth+2; 
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при размещении окна для папки {folder}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выполните сканирование для поиска папок с Telegram.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
