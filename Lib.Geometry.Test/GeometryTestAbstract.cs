using Lib.Geometry.Abstractions;
using Lib.Geometry.WKT;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.Geometry.Test
{
    public abstract class GeometryTestAbstract
    {
        private readonly GeometryFromWktFactory _geometryFromWktFactory = new GeometryFromWktFactory();
        protected readonly WktFromGeometryFactory WktFromGeometryFactory = new WktFromGeometryFactory();

        protected abstract IGeometry TestingGeometry { get; }
        protected abstract string TestingWKT { get; }

        protected abstract IPoint OutsidePoint { get; }
        protected abstract IPoint InsidePoint { get; }
        protected abstract IPoint BorderPoint { get; }

        [TestMethod]
        public void ToWKT_ReturnsExpectedWKT()
        {
            var geometry = TestingGeometry;
            Check.That(WktFromGeometryFactory.Factory(geometry)).Equals(TestingWKT);
        }

        [TestMethod]
        public void FromWKT_ReturnsExpectedGeometry()
        {
            var geometry = _geometryFromWktFactory.Factory(TestingWKT);
            Check.That(geometry).Equals(TestingGeometry);
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
