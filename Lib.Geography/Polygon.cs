using System.Collections.Generic;
using System.Linq;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace Lib.Geography
{
    /// <summary>
    /// A continguous polygon
    /// </summary>
    public class Polygon : Geometry
    {
        private static IPolygon FromEnvelope(IEnumerable<ICoordinate> coordinates)
        {
            var envelope = coordinates.Select(point => new Coordinate(point.Latitude, point.Longitude));
            return new NetTopologySuite.Geometries.Polygon(new LinearRing(envelope.ToArray()));
        }

        /// <inheritdoc />
        public Polygon(params ICoordinate[] envelope) : base(FromEnvelope(envelope))
        {
        }
    }
}
