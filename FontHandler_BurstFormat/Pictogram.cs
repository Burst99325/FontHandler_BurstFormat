using System;
using System.Text;
using System.IO;
using FontHandler;

namespace FontHandlerFormat
{
    public class FontHandlerPic
    {
        public Pictogram Load(string filePath)
        {
            try
            {
                StreamReader stream = new StreamReader(filePath, Encoding.Default, true);
                if (stream.CurrentEncoding != Encoding.UTF8) throw new Exception("File encoding is not UTF-8!");
                string[] picHeader = stream.ReadLine().Split(',');
                if (picHeader[0] != "PICTOGRAM") throw new Exception("File is not Burstypo pictogram file!");
                Pictogram pic = new Pictogram
                {
                    Name = Path.GetFileNameWithoutExtension(filePath),
                    FilePath = filePath,
                    Width = Convert.ToInt32(picHeader[1]),
                    Height = Convert.ToInt32(picHeader[2]),
                };
                bool[,] pixels = new bool[pic.Width, pic.Height];
                for (int y = 0; y < pic.Height; y++)
                {
                    string binaryLine = stream.ReadLine();
                    for (int x = 0; x < pic.Width; x++)
                        pixels[x, y] = binaryLine[x] == '1';
                }
                pic.Pixels = pixels;
                stream.Close();
                return pic;
            }
            catch 
            {
                throw new Exception("File is corrupt!");
            }
        }

        public void Save(Pictogram pic, string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Create);
            StreamWriter stream = new StreamWriter(file, Encoding.UTF8);
            string[] picHeader = new string[]
            {
                "PICTOGRAM",
                pic.Width.ToString(),
                pic.Height.ToString()
            };
            stream.WriteLine(string.Join(",", picHeader));
            for (int y = 0; y < pic.Height; y++)
            {
                string binaryLine = string.Empty;
                for (int x = 0; x < pic.Width; x++)
                    binaryLine += pic.Pixels[x, y] ? "1" : "0";
                stream.WriteLine(binaryLine);
            }
            stream.Close();
            file.Close();
        }
    }
}
