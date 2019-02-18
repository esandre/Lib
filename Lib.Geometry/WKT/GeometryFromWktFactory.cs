using System;
using Lib.Geometry.Abstractions;
using Lib.Patterns;
using NetTopologySuite.IO;

namespace Lib.Geometry.WKT
{
    /// <summary>
    /// Factories a Geometry from a WKT string
    /// </summary>
    public class GeometryFromWktFactory : IFactory<string, IGeometry>
    {
        private readonly WKTReader _reader = new WKTReader();

        /// <inheritdoc />
        /// <summary>
        /// Reads a WKT string
        /// </summary>
        public IGeometry Factory(string wkt)
        {
            var ntsGeometry = _reader.Read(wkt);

            if (ntsGeometry is GeoAPI.Geometries.IPolygon polygon) return new Polygon(polygon);
            if (ntsGeometry is GeoAPI.Geometries.IPoint point) return new Point(point);

            throw new NotImplementedException("Only POLYGON and POINT WKT objects are currently supported");
        }
    }
}
