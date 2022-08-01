namespace PopStudio.Plugin
{
    internal class IDisposablePool : IDisposable
    {
        List<IDisposable> all = new List<IDisposable>();

        public IDisposable Add(IDisposable n)
        {
            all.Add(n);
            return n;
        }

        public T Add<T>(T n)
        {
            all.Add(n as IDisposable);
            return n;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (IDisposable s in all)
                {
                    s?.Dispose();
                }
                all.Clear();
                //Can't Set all into null
            }
        }
    }
}
