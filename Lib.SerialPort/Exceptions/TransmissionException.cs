using System;

namespace Lib.SerialPort.Exceptions
{
    public class TransmissionException : Exception
    {
        internal TransmissionException(Exception inner) 
            : base("Error during message transmission", inner)
        {

        }
    }
}
