using GeoAPI.Geometries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Geometry.Test
{
    [TestClass]
    public class PseudoCircleTest : GeometryTestAbstract
    {
        protected override Geometry TestingGeometry => new PseudoCircle(Point(0, 0), 1, 1);
        protected override string TestingWKT => TestingGeometry.ToWkt();
        protected override IPoint OutsidePoint => Point(1, 1);
        protected override IPoint InsidePoint => Point(0, 0);
        protected override IPoint BorderPoint => Point(1, 0);
    }
}
