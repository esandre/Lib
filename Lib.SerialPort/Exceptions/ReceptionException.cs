using System;
using System.Linq;

namespace Lib.SerialPort.Exceptions
{
    public class ReceptionException : Exception
    {
        internal ReceptionException(Exception inner, byte[] receivedBeforeError)
            : base(ErrorMessageWithBytes(receivedBeforeError), inner)
        {

        }

        private static string ErrorMessageWithBytes(byte[] bytesReceived)
        {
            return "Error during message reception." +
                (bytesReceived.Any() 
                    ? "Before that received " + BitConverter.ToString(bytesReceived)
                    : string.Empty);
        }
    }
}
