namespace ChatMarchenkoIlya.Data
{
    public class Connect
    {
        private static Connect instance;

        public string Name { get; private set; }
        private static object syncRoot = new Object();

        protected Connect(string name)
        {
            this.Name = name;

        }
        
        public static Connect getInstance(string name)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new Connect(name);
                }
            }
            return instance;
        }
    }
}
