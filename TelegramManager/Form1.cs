using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TelegramManager.Main;

namespace TelegramFolderScanner
{
    public partial class MainForm : Form
    {
        private List<string> scannedTelegramFolders = new List<string>(); // Список отсканированных папок

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            // Очистить все элементы управления в панели
            this.scrollablePanel.Controls.Clear();

            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string rootPath = folderDialog.SelectedPath;

                    // Вызов метода из TelegramScanFolder
                    scannedTelegramFolders = TelegramScanFolder.GetTelegramFolders(rootPath); // Сохраняем результаты сканирования

                    // Создать кнопки для каждой найденной папки
                    int yPosition = 10; // Начальная позиция по вертикали
                    foreach (string folder in scannedTelegramFolders)
                    {
                        // Создаём кнопку
                        Button folderButton = new Button
                        {
                            Text = Path.GetFileName(folder), // Имя папки как текст кнопки
                            Tag = folder, // Сохраняем путь к папке в Tag для использования
                            Size = new System.Drawing.Size(350, 30),
                            Location = new System.Drawing.Point(10, yPosition)
                        };

                        // Добавляем событие нажатия на кнопку
                        folderButton.Click += (s, args) => OpenFolder(folder);

                        // Добавляем кнопку на панель
                        this.scrollablePanel.Controls.Add(folderButton);

                        // Увеличиваем позицию для следующей кнопки
                        yPosition += 40;
                    }

                    // Если подходящих папок не найдено
                    if (scannedTelegramFolders.Count == 0)
                    {
                        MessageBox.Show("Папки с Telegram.exe не найдены.", "Результаты поиска", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        private void OpenFolder(string folderPath)
        {
            // Открыть Telegram
            string telegramExePath = Path.Combine(folderPath, "Telegram.exe");

            // Проверяем, существует ли файл Telegram.exe
            if (File.Exists(telegramExePath))
            {
                try
                {
                    // Запускаем Telegram.exe
                    System.Diagnostics.Process.Start(telegramExePath);
                }
                catch (Exception ex)
                {
                    // Если не удалось запустить, показываем сообщение
                    MessageBox.Show($"Не удалось запустить Telegram в папке: {folderPath}. Ошибка: {ex.Message}",
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Если файл Telegram.exe не найден, сообщаем об этом
                MessageBox.Show($"Не найден файл Telegram.exe в папке: {folderPath}",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnOpenAll_Click(object sender, EventArgs e)
        {
            // Проверяем, были ли отсканированы папки
            if (scannedTelegramFolders.Count > 0)
            {
                // Открываем все найденные Telegram
                foreach (var folder in scannedTelegramFolders)
                {
                    OpenFolder(folder);  // Используем уже существующий метод для запуска Telegram
                }
            }
            else
            {
                // Если папки не были отсканированы, выводим сообщение
                MessageBox.Show("Пожалуйста, выполните сканирование для поиска папок с Telegram.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
