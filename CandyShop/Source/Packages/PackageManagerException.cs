namespace CandyShop.Packages
{
    [System.Serializable]
    public class PackageManagerException : System.Exception
    {
        public PackageManagerException() { }
        public PackageManagerException(string message) : base(message) { }
        public PackageManagerException(string message, System.Exception inner) : base(message, inner) { }
        protected PackageManagerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
