using NetTopologySuite.IO;

namespace Lib.Geometry
{
    /// <summary>
    /// Tools to represent a <see cref="Geometry"/> as WKT
    /// </summary>
    public static class WktRepresentation
    {
        private static readonly WKTReader Reader = new WKTReader();
        private static readonly WKTWriter Writer = new WKTWriter();

        /// <summary>
        /// Reads a WKT string
        /// </summary>
        public static Geometry ReadWkt(string wkt)
        {
            return new Geometry(Reader.Read(wkt));
        }

        /// <summary>
        /// Writes a geometry as a WKT string
        /// </summary>
        public static string ToWkt(this Geometry geometry)
        {
            return Writer.Write(geometry.NTSGeometry);
        }
    }
}
