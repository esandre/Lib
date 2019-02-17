using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;
using IGeometry = Lib.Geometry.Abstractions.IGeometry;
using IPolygon = Lib.Geometry.Abstractions.IPolygon;
using IPoint = Lib.Geometry.Abstractions.IPoint;
using INTSGeometry = GeoAPI.Geometries.IGeometry;
using INTSPoint = GeoAPI.Geometries.IPoint;
using INTSPolygon = GeoAPI.Geometries.IPolygon;
using NTSPolygon = NetTopologySuite.Geometries.Polygon;
using INTSLinearRing = GeoAPI.Geometries.ILinearRing;
using NTSLinearRing = NetTopologySuite.Geometries.LinearRing;
using NTSCoordinate = GeoAPI.Geometries.Coordinate;

namespace Lib.Geometry.NTS
{
    internal static class NTSConverter
    {
        private static INTSLinearRing FromPoints(IEnumerable<IPoint> coordinates)
            => new NTSLinearRing(coordinates.Select(point => new NTSCoordinate(point.X, point.Y)).ToArray());

        public static IEnumerable<IPoint> FromLinearRing(INTSLinearRing linearRing) =>
            linearRing.Coordinates.Select(point => new Point(point));

        public static INTSPolygon FromEnvelope(IEnumerable<IPoint> envelope, params IEnumerable<IPoint>[] holes)
            => new NTSPolygon(FromPoints(envelope), holes.Select(FromPoints).ToArray());

        private static INTSPolygon ToNTS(this IPolygon polygon)
            => new NTSPolygon(
                FromPoints(polygon.Shell), 
                polygon.Holes.Select(FromPoints).ToArray());
        
        public static INTSGeometry ToNTS(this IGeometry geometry)
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
