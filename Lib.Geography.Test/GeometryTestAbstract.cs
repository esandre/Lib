using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Geography.Test
{
    public abstract class GeometryTestAbstract
    {
        protected static ICoordinate Coordinates(double latitude, double longitude)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return Mock.Of<ICoordinate>(m => m.Latitude == latitude && m.Longitude == longitude);
        }

        protected abstract Geometry TestingGeometry { get; }
        protected abstract string TestingWKT { get; }

        protected abstract ICoordinate OutsidePoint { get; }
        protected abstract ICoordinate InsidePoint { get; }
        protected abstract ICoordinate BorderPoint { get; }

        [TestMethod]
        public void ToWKT_ReturnsExpectedWKT()
        {
            var geometry = TestingGeometry;
            Check.That(geometry.ToWkt()).Equals(TestingWKT);
        }

        [TestMethod]
        public void FromWKT_ReturnsExpectedGeometry()
        {
            var wkt = TestingWKT;
            Check.That(WktRepresentation.ReadWkt(wkt)).Equals(TestingGeometry);
        }

        [TestMethod]
        public void ContainsAndCovers_ReturnsFalse_ForOutsidePoint()
        {
            var geometry = TestingGeometry;

            Check.That(geometry.Contains(OutsidePoint)).IsFalse();
            Check.That(geometry.Covers(OutsidePoint)).IsFalse();
        }

        [TestMethod]
        public void ContainsAndCovers_ReturnsTrue_ForInsidePoint()
        {
            var geometry = TestingGeometry;

            Check.That(geometry.Contains(InsidePoint)).IsTrue();
            Check.That(geometry.Covers(InsidePoint)).IsTrue();
        }

        [TestMethod]
        public void Contains_ReturnsFalse_ForBorderPoint()
        {
            var geometry = TestingGeometry;
            Check.That(geometry.Contains(BorderPoint)).IsFalse();
        }

        [TestMethod]
        public void Covers_ReturnsTrue_ForBorderPoint()
        {
            var geometry = TestingGeometry;
            Check.That(geometry.Covers(BorderPoint)).IsTrue();
        }
    }
}
