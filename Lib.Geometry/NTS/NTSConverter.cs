using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;
using IGeometry = Lib.Geometry.Abstractions.IGeometry;
using IPolygon = Lib.Geometry.Abstractions.IPolygon;
using IPoint = Lib.Geometry.Abstractions.IPoint;
using NTSGeometry = NetTopologySuite.Geometries.Geometry;
using NTSPolygon = NetTopologySuite.Geometries.Polygon;
using NTSLinearRing = NetTopologySuite.Geometries.LinearRing;
using NTSCoordinate = NetTopologySuite.Geometries.Coordinate;

namespace Lib.Geometry.NTS
{
    internal static class NTSConverter
    {
        private static NTSLinearRing FromPoints(IEnumerable<IPoint> coordinates)
            => new NTSLinearRing(coordinates.Select(point => new NTSCoordinate(point.X, point.Y)).ToArray());

        public static IEnumerable<IPoint> FromLinearRing(NTSLinearRing linearRing) =>
            linearRing.Coordinates.Select(point => new Point(point));

        public static NTSPolygon FromEnvelope(IEnumerable<IPoint> envelope, params IEnumerable<IPoint>[] holes)
            => new NTSPolygon(FromPoints(envelope), holes.Select(FromPoints).ToArray());

        private static NTSPolygon ToNTS(this IPolygon polygon)
            => new NTSPolygon(
                FromPoints(polygon.Shell), 
                polygon.Holes.Select(FromPoints).ToArray());
        
        public static NTSGeometry ToNTS(this IGeometry geometry)
        {
            var polygons = geometry.Polygons.ToArray();
            if(polygons.Length > 1) return new MultiPolygon(polygons.Select(ToNTS).ToArray());

            var singlePolygon = geometry.Polygons.First();
            var envelope = singlePolygon.Shell.ToArray();

            if(envelope.Length == 1) return new NetTopologySuite.Geometries.Point(envelope[0].X, envelope[0].Y);

            return new NTSPolygon(FromPoints(envelope), singlePolygon.Holes.Select(FromPoints).ToArray());
        }
    }
}
