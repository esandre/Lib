using System;

namespace Lib.DateTime.Descriptors
{
    public class EmptyTimeSlotException : Exception { }
    public class TooLongTimeSpanException : Exception { }
    public class InvalidDescriptorException : Exception { public InvalidDescriptorException(string descriptor) : base(descriptor + " is not a valid descriptor") {}}
}
