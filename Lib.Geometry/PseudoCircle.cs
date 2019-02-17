using Lib.Geometry.Abstractions;
using NetTopologySuite.Geometries;
using INTSPolygon = GeoAPI.Geometries.IPolygon;
using NTSPoint = NetTopologySuite.Geometries.Point;

namespace Lib.Geometry
{
    /// <summary>
    /// Approximates a circle
    /// </summary>
    public class PseudoCircle : Polygon
    {
        private static INTSPolygon DrawCircle(IPoint center, double radius, int verticesPerQuarter)
        {
            var point = new NTSPoint(center.X, center.Y);
            var geometry = point.Buffer(radius, verticesPerQuarter);
            return new NetTopologySuite.Geometries.Polygon(new LinearRing(geometry.Coordinates));
        }

        /// <summary>
        /// Builds a pseudocircle
        /// </summary>
        public PseudoCircle(IPoint center, double radius, int vertices)
            : base(DrawCircle(center, radius, vertices))
        {
        }
    }
}
