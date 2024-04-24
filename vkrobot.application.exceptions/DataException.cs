namespace vkrobot.application.exceptions
{
    [Serializable]
    public class DataException : Exception
    {
        public DataException() : base() { }
        public DataException(string message) : base(message) { }
        public DataException(Exception e) : base(e.Message, e.InnerException) { }
        public DataException(string message, Exception inner) : base(message, inner) { }
        protected DataException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
