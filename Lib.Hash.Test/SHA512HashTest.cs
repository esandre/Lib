using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Hash.Test
{
    [TestClass]
    public class SHA512HashTest : StreamHashAbstractTest<SHA512>
    {
        protected override SHA512 Instance => SHA512.Create();
    }
}
