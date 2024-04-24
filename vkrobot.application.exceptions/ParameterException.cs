namespace vkrobot.application.exceptions
{
    [Serializable]
    public class ParameterException : Exception
    {
        public ParameterException() : base() { }
        public ParameterException(string message) : base(message) { }
        public ParameterException(string message, Exception inner) : base(message, inner) { }
        protected ParameterException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
