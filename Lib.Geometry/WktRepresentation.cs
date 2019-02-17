using System;
using Lib.Geometry.Abstractions;
using Lib.Geometry.NTS;
using NetTopologySuite.IO;

namespace Lib.Geometry
{
    /// <summary>
    /// Tools to represent a <see cref="IGeometry"/> as WKT
    /// </summary>
    public static class WktRepresentation
    {
        private static readonly WKTReader Reader = new WKTReader();
        private static readonly WKTWriter Writer = new WKTWriter();

        /// <summary>
        /// Reads a WKT string
        /// </summary>
        public static IGeometry ReadWkt(string wkt)
        {
            var ntsGeometry = Reader.Read(wkt);

            if(ntsGeometry is GeoAPI.Geometries.IPolygon polygon) return new Polygon(polygon);
            if (ntsGeometry is GeoAPI.Geometries.IPoint point) return new Point(point);

            throw new NotImplementedException("Only POLYGON and POINT WKT objects are currently supported");
        }

        /// <summary>
        /// Writes a geometry as a WKT string
        /// </summary>
        public static string ToWkt(this IGeometry geometry)
        {
            return Writer.Write(geometry.ToNTS());
        }
    }
}
