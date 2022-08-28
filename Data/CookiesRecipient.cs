namespace ChatMarchenkoIlya.Data
{
    public class CookiesRecipient
    {
        private static CookiesRecipient instance;

        public int CountMesg { get; set; }
        public string Name { get; private set; }
        private static object syncRoot = new Object();

        protected CookiesRecipient(string name)
        {
            this.Name = name;

        }
        protected CookiesRecipient(int countMesg)
        {
            this.CountMesg = countMesg;
        }

        public static CookiesRecipient getInstance(string name)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new CookiesRecipient(name);
                }
            }
            return instance;
        }
        public static CookiesRecipient getInstance(int countMesg)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new CookiesRecipient(countMesg);
                }
            }
            return instance;
        }

    }
}
