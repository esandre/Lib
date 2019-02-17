using Lib.Geometry.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Geometry.Test
{
    [TestClass]
    public class PolygonTest : GeometryTestAbstract
    {
        protected override IGeometry TestingGeometry
            => new Polygon(
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
                new Point(0, 0));

        protected override string TestingWKT => "POLYGON ((0 0, 0 1, 1 1, 1 0, 0 0))";
        protected override IPoint OutsidePoint => new Point(2, 1);
        protected override IPoint InsidePoint => new Point(0.5, 0.5);
        protected override IPoint BorderPoint => new Point(0.5, 1);
    }
}
