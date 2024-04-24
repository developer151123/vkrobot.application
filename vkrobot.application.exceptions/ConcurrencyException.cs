namespace vkrobot.application.exceptions
{
    [Serializable]
    public class ConcurrencyException : Exception
    {
        private readonly bool _rowDeleted;

        public bool RowDeleted { get { return _rowDeleted; } }
        public ConcurrencyException(Exception e, bool rowDeleted = false) : base(e.Message, e.InnerException)
        {
            _rowDeleted = rowDeleted;
        }
        protected ConcurrencyException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
