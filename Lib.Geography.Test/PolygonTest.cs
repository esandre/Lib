using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Geography.Test
{
    [TestClass]
    public class PolygonTest : GeometryTestAbstract
    {
        protected override Geometry TestingGeometry => new Polygon(Coordinates(0, 0), Coordinates(0, 1), Coordinates(1, 1), Coordinates(1, 0), Coordinates(0, 0));
        protected override string TestingWKT => "POLYGON ((0 0, 0 1, 1 1, 1 0, 0 0))";
        protected override ICoordinate OutsidePoint => Coordinates(2, 1);
        protected override ICoordinate InsidePoint => Coordinates(0.5, 0.5);
        protected override ICoordinate BorderPoint => Coordinates(0.5, 1);
    }
}
