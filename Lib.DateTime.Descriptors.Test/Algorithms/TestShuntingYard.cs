using System.Collections.Generic;
using Lib.DateTime.Descriptors.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.DateTime.Descriptors.Test.Algorithms
{
    [TestClass]
    public class TestShuntingYard
    {
        [TestMethod]
        public void TestToReversePolish()
        {
            const string input = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 POW 3";
            var operators = new Dictionary<string, Operator>
            {
                {"^", new Operator(4, false)},
                {"POW", new Operator(4, false)},
                {"*", new Operator(3, true)},
                {"/", new Operator(3, true)},
                {"+", new Operator(2, true)},
                {"-", new Operator(2, true)}
            };

            var shuntingYard = new ShuntingYard(operators);

            Assert.AreEqual("3 4 2 * 1 5 - 2 3 POW ^ / +", shuntingYard.ToReversePolishNotation(input));
        }
    }
}
