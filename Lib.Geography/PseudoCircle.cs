using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Buffer;

namespace Lib.Geography
{
    /// <summary>
    /// Approximates a circle
    /// </summary>
    public class PseudoCircle : Geometry
    {
        private static IGeometry DrawCircle(ICoordinate center, double radius, int verticesPerQuarter)
        {
            var point = new Point(center.Latitude, center.Longitude);
            return point.Buffer(radius, verticesPerQuarter);
        }

        /// <summary>
        /// Builds a pseudocircle
        /// </summary>
        public PseudoCircle(ICoordinate center, double radius, int vertices) 
            : base(DrawCircle(center, radius, vertices))
        {
        }
    }
}
