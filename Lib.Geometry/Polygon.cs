using System.Collections.Generic;
using System.Linq;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace Lib.Geometry
{
    /// <inheritdoc />
    public class Polygon : Geometry
    {
        private static IPolygon FromEnvelope(IEnumerable<IPoint> coordinates)
        {
            var envelope = coordinates.Select(point => new Coordinate(point.X, point.Y));
            return new NetTopologySuite.Geometries.Polygon(new LinearRing(envelope.ToArray()));
        }

        /// <inheritdoc />
        public Polygon(params IPoint[] envelope) : base(FromEnvelope(envelope))
        {
        }
    }
}
