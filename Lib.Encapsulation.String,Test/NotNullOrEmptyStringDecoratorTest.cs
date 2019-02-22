using System;
using Lib.Encapsulation.String;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Encapsulation.String_Test
{
    [TestClass]
    public class NotNullOrEmptyStringDecoratorTest
    {
        [TestMethod]
        public void Input_CannotBeEmpty()
        {
            var emptyEncapsulatedString = Mock.Of<IEncapsulatedValue<string>>(m => m.Value == string.Empty);

            Check.ThatCode(() => new NotNullOrEmptyStringDecorator(emptyEncapsulatedString))
                .Throws<FormatException>()
                .WithMessage(NotNullOrEmptyStringDecorator.NullOrEmptyInputMessage);

            Check.ThatCode(() => new NotNullOrEmptyStringDecorator(string.Empty))
                .Throws<FormatException>()
                .WithMessage(NotNullOrEmptyStringDecorator.NullOrEmptyInputMessage);
        }

        [TestMethod]
        public void Input_CannotBeNull()
        {
            var nullEncapsulatedString = Mock.Of<IEncapsulatedValue<string>>(m => m.Value == null);

            Check.ThatCode(() => new NotNullOrEmptyStringDecorator(nullEncapsulatedString))
                .Throws<FormatException>()
                .WithMessage(NotNullOrEmptyStringDecorator.NullOrEmptyInputMessage);

            Check.ThatCode(() => new NotNullOrEmptyStringDecorator((string)null))
                .Throws<FormatException>()
                .WithMessage(NotNullOrEmptyStringDecorator.NullOrEmptyInputMessage);
        }
    }
}
