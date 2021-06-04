namespace CandyShop.Packages.Chocolatey
{
    // use for exceptions with Chocolatey software

    [System.Serializable]
    public class ChocolateyException : System.Exception
    {
        public ChocolateyException() { }
        public ChocolateyException(string message) : base(message) { }
        public ChocolateyException(string message, System.Exception inner) : base(message, inner) { }
        protected ChocolateyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
