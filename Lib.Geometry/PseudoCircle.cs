using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace Lib.Geometry
{
    /// <summary>
    /// Approximates a circle
    /// </summary>
    public class PseudoCircle : Geometry
    {
        private static IGeometry DrawCircle(IPoint center, double radius, int verticesPerQuarter)
        {
            var point = new Point(center.X, center.Y);
            return point.Buffer(radius, verticesPerQuarter);
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
