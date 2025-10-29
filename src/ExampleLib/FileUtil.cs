using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExampleLib;

public static class FileUtil
{
/// Перезаписывает указанный текстовый файл, добавляя номера строк к каждой строке
    public static void AddLineNumbers(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File not found: {path}");
        }

        // Читаем все строки файла
        string[] lines = File.ReadAllLines(path);

        // Обрабатываем каждую строку - добавляем номер
        List<string> numberedLines = new List<string>();
        for (int i = 0; i < lines.Length; i++)
        {
            numberedLines.Add($"{i + 1}. {lines[i]}");
        }

        // Записываем обратно в файл
        File.WriteAllLines(path, numberedLines);
    }

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
}