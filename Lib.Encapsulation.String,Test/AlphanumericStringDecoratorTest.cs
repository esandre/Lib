using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Encapsulation.String.Test
{
    [TestClass]
    public class AlphanumericStringDecoratorTest
    {
        [TestMethod]
        public void Input_CanBeAlpha()
        {
            const string testedString = "daAFAzeafzergZRGzvz";
            CheckAllConstructors(testedString, false);
        }

        [TestMethod]
        public void Input_CanBeNumerical()
        {
            const string testedString = "276217172725";
            CheckAllConstructors(testedString, false);
        }

        [TestMethod]
        public void Input_CanBeAlphanumerical()
        {
            const string testedString = "da53AFAz73eafze7r33gZRGzvz";
            CheckAllConstructors(testedString, false);
        }

        [TestMethod]
        public void Input_CannotHaveSymbols()
        {
            const string testedString = "fgeg-564$_";
            CheckAllConstructors(testedString, true);
        }

        [TestMethod]
        public void Input_CannotHaveWhitespace()
        {
            const string testedString = "daAFAz eafze rgZR Gz\nvz";
            CheckAllConstructors(testedString, true);
        }

        private static void CheckAllConstructors(string input, bool shouldFail)
        {
            var encapsulatedStringMock = Mock.Of<IEncapsulatedValue<string>>(m => m.Value == input);

            if (shouldFail)
            {
                Check.ThatCode(() => new AlphanumericStringDecorator(encapsulatedStringMock))
                    .Throws<FormatException>()
                    .WithMessage(AlphanumericStringDecorator.BadInputMessage);

                Check.ThatCode(() => new AlphanumericStringDecorator(input))
                    .Throws<FormatException>()
                    .WithMessage(AlphanumericStringDecorator.BadInputMessage);
            }
            else
            {
                Check.ThatCode(() => new AlphanumericStringDecorator(encapsulatedStringMock)).DoesNotThrow();
                Check.ThatCode(() => new AlphanumericStringDecorator(input)).DoesNotThrow();
            }
        }
    }
}
