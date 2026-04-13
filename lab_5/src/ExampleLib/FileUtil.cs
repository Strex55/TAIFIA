using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExampleLib;

public static class FileUtil
{
    /// <summary>
    /// Сортирует строки в указанном файле.
    /// Перезаписывает файл, но не атомарно: ошибка ввода-вывода при записи приведёт к потере данных.
    /// </summary>
    public static void SortFileLines(string path)
    {
        // Читаем и сортируем строки файла.
        List<string> lines = File.ReadLines(path, Encoding.UTF8).ToList();
        lines.Sort();

        // Перезаписываем файл с нуля (режим Truncate).
        using FileStream file = File.Open(path, FileMode.Truncate, FileAccess.Write);
        for (int i = 0, iMax = lines.Count; i < iMax; ++i)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(lines[i]);
            file.Write(bytes);
            if (i != iMax - 1)
            {
                file.Write("\n"u8);
            }
        }
    }

    /// <summary>
    /// Перезаписывает указанный текстовый файл, добавляя номера строк к каждой строке.
    /// Нумерация начинается с 1. Формат: "N. {исходная строка}".
    /// Перезапись проводится в режиме FileMode.Truncate (неатомарная).
    /// </summary>
    public static void AddLineNumbers(string path)
    {
        // Читаем все строки
        List<string> lines = File.ReadLines(path, Encoding.UTF8).ToList();

        // Перезаписываем файл с нуля (режим Truncate).
        using FileStream file = File.Open(path, FileMode.Truncate, FileAccess.Write);
        for (int i = 0, iMax = lines.Count; i < iMax; ++i)
        {
            // Формируем префикс "N. " и содержимое строки.
            string numbered = (i + 1).ToString() + ". " + lines[i];
            byte[] bytes = Encoding.UTF8.GetBytes(numbered);
            file.Write(bytes);
            if (i != iMax - 1)
            {
                file.Write("\n"u8);
            }
        }
    }
}