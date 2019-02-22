using System;
using Lib.Encapsulation.String;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Encapsulation.String_Test
{
    [TestClass]
    public class MaximalLengthStringDecoratorTest
    {
        [TestMethod]
        public void StringInput_CannotExceed_LengthLimit()
        {
            const int limit = 12;

            var tooLongContent = StringGenerator.GenerateDesiredLengthString(limit + 1);
            var encapsulatedStringMock = Mock.Of<IEncapsulatedValue<string>>(m => m.Value == tooLongContent);

            Check.ThatCode(() => new MaximalLengthStringDecorator(limit, encapsulatedStringMock))
                .Throws<FormatException>()
                .WithMessage(MaximalLengthStringDecorator.LengthExceededMessage(limit));

            Check.ThatCode(() => new MaximalLengthStringDecorator(limit, tooLongContent))
                .Throws<FormatException>()
                .WithMessage(MaximalLengthStringDecorator.LengthExceededMessage(limit));
        }
    }
}
