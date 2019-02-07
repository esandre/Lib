using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Geography.Test
{
    [TestClass]
    public class PseudoCircleTest : GeometryTestAbstract
    {
        protected override Geometry TestingGeometry => new PseudoCircle(Coordinates(0, 0), 1, 1);
        protected override string TestingWKT => TestingGeometry.ToWkt();
        protected override ICoordinate OutsidePoint => Coordinates(1, 1);
        protected override ICoordinate InsidePoint => Coordinates(0, 0);
        protected override ICoordinate BorderPoint => Coordinates(1, 0);
    }
}
