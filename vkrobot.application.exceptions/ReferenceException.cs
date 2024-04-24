namespace vkrobot.application.exceptions
{
    [Serializable]
    public class ReferenceException : Exception
    {
        public ReferenceException() : base() { }
        public ReferenceException(string message) : base(message) { }
        public ReferenceException(string message, Exception inner) : base(message, inner) { }
        protected ReferenceException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
