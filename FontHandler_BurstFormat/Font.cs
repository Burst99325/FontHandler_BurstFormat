﻿using System;
using System.Text;
using System.IO;
using FontHandler;

namespace FontHandlerFormat
{
    public class FontHandlerFont
    {
        public Font Load(string filePath)
        {
            StreamReader stream = new StreamReader(filePath);
            if (stream.CurrentEncoding != Encoding.UTF8) throw new Exception("File encoding is not UTF-8!");
            string[] fontHeader = stream.ReadLine().Split(',');
            if (fontHeader[0] != "FONT") throw new Exception("File is not Burstypo font file!");
            Font font = new Font
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                FilePath = filePath,
                Height = Convert.ToInt32(fontHeader[1]),
                Spacing = Convert.ToInt32(fontHeader[2])
            };
            while (true)
            {
                if (stream.EndOfStream) break;
                string[] charHeader = stream.ReadLine().Split(',');
                FontChar fontChar = new FontChar();
                if (charHeader.Length == 3)
                {
                    fontChar.Char = ',';
                    fontChar.Width = Convert.ToInt32(charHeader[2]);
                }
                else
                {
                    fontChar.Char = charHeader[0][0];
                    fontChar.Width = Convert.ToInt32(charHeader[1]);
                }
                bool[,] pixels = new bool[fontChar.Width, font.Height];
                for (int y = 0; y < font.Height; y++)
                {
                    string binaryLine = stream.ReadLine();
                    for (int x = 0; x < fontChar.Width; x++)
                        pixels[x, y] = binaryLine[x] == '1';
                }
                fontChar.Pixels = pixels;
                font.Chars.Add(fontChar);
            }
            stream.Close();
            return font;
        }

        public void Save(Font font, string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Create);
            StreamWriter stream = new StreamWriter(file, Encoding.UTF8);
            string[] fontHeader = new string[] 
            {
                "FONT",
                font.Height.ToString(),
                font.Spacing.ToString()
            };
            stream.WriteLine(string.Join(",", fontHeader));
            foreach (FontChar fontChar in font.Chars)
            {
                string[] charHeader = new string[] 
                { 
                    fontChar.Char.ToString(),
                    fontChar.Width.ToString()
                };
                stream.WriteLine(string.Join(",", charHeader));
                for (int y = 0; y < font.Height; y++)
                {
                    string binaryLine = string.Empty;
                    for (int x = 0; x < fontChar.Width; x++)
                        binaryLine += fontChar.Pixels[x, y] ? "1" : "0";
                    stream.WriteLine(binaryLine);
                }
            }
            stream.Close();
            file.Close();
        }
    }
}
