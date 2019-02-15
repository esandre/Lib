using GeoAPI.Geometries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Geometry.Test
{
    [TestClass]
    public class PolygonTest : GeometryTestAbstract
    {
        protected override Geometry TestingGeometry => new Polygon(Point(0, 0), Point(0, 1), Point(1, 1), Point(1, 0), Point(0, 0));
        protected override string TestingWKT => "POLYGON ((0 0, 0 1, 1 1, 1 0, 0 0))";
        protected override IPoint OutsidePoint => Point(2, 1);
        protected override IPoint InsidePoint => Point(0.5, 0.5);
        protected override IPoint BorderPoint => Point(0.5, 1);
    }
}
