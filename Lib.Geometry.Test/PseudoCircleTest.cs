using Lib.Geometry.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Geometry.Test
{
    [TestClass]
    public class PseudoCircleTest : GeometryTestAbstract
    {
        protected override IGeometry TestingGeometry => new PseudoCircle(new Point(0, 0), 1, 1);
        protected override string TestingWKT => TestingGeometry.ToWkt();
        protected override IPoint OutsidePoint => new Point(1, 1);
        protected override IPoint InsidePoint => new Point(0, 0);
        protected override IPoint BorderPoint => new Point(1, 0);
    }
}
