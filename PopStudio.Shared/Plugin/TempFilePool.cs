namespace PopStudio.Plugin
{
    internal class TempFilePool : IDisposable
    {
        List<string> Files = new List<string>();

        public string Add()
        {
            string s = Path.GetTempFileName();
            Files.Add(s);
            return s;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (string s in Files)
                {
                    if (File.Exists(s)) File.Delete(s);
                }
                Files.Clear();
                Files = null;
            }
        }
    }
}
