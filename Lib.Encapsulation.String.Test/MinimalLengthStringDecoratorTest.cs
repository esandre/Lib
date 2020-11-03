using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Encapsulation.String.Test
{
    [TestClass]
    public class MinimalLengthStringDecoratorTest
    {

        [TestMethod]
        public void Input_CannotBe_ShorterThan_Minima()
        {
            const int minima = 12;

            var tooShortContent = StringGenerator.GenerateDesiredLengthString(minima - 1);
            var encapsulatedStringMock = Mock.Of<IEncapsulatedValue<string>>(m => m.Value == tooShortContent);

            Check.ThatCode(() => new MinimalLengthStringDecorator(minima, tooShortContent))
                .Throws<FormatException>()
                .WithMessage(MinimalLengthStringDecorator.TooShortMessage(minima));

            Check.ThatCode(() => new MinimalLengthStringDecorator(minima, encapsulatedStringMock))
                    .Throws<FormatException>()
                    .WithMessage(MinimalLengthStringDecorator.TooShortMessage(minima));
        }
    }
}
