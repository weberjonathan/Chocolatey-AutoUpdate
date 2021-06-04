using System;

namespace CandyShop.Chocolatey
{
    [System.Serializable]
    public class ChocolateyParsingException : Exception
    {
        public ChocolateyParsingException() { }
        public ChocolateyParsingException(string message) : base(message) { }
        public ChocolateyParsingException(string message, Exception inner) : base(message, inner) { }
        protected ChocolateyParsingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
