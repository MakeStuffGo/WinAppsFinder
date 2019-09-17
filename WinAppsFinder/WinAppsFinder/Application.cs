namespace WinAppsFinder
{
    public class Application
    {
        public string Name { get; set; }
        public string InstallPath { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
