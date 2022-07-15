namespace FontHandlerFormat
{
    public class Base
    {
        public string GetFormatName()
        {
            return "Burstypo";
        }

        public string[] GetFontTypeFileFormats()
        {
            return new string[] { ".bfon" };
        }

        public string[] GetPicTypeFileFormats()
        {
            return new string[] { ".bpic" };
        }
    }
}
