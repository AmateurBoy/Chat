namespace ChatMarchenkoIlya.Data
{
    public class ContextSingle
    {
        private static ContextSingle instance;
        public ApplicationContext AC { get; private set; }
        
        private static object syncRoot = new Object();

        protected ContextSingle(ApplicationContext AC)
        {
            this.AC = AC;

        }
      
        public static ContextSingle getInstance(ApplicationContext AC)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new ContextSingle(AC);
                }
            }
            return instance;
        }
        
    }
}
