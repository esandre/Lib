using System;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace Lib.Geography
{
    /// <summary>
    /// A generic Geometry
    /// </summary>
    public class Geometry : IEquatable<Geometry>
    {
        internal IGeometry NTSGeometry { get; }
        
        internal Geometry(IGeometry geometry)
        {
            NTSGeometry = geometry;
        }

        /// <summary>
        /// True if point is inside the geometry, including borders
        /// </summary>
        public bool Contains(ICoordinate point)
        {
            return NTSGeometry.Contains(new Point(new Coordinate(point.Latitude, point.Longitude)));
        }

        /// <summary>
        /// True if point is inside the geometry, excluding borders
        /// </summary>
        public bool Covers(ICoordinate point)
        {
            return NTSGeometry.Covers(new Point(new Coordinate(point.Latitude, point.Longitude)));
        }

        /// <inheritdoc />
        public bool Equals(Geometry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(NTSGeometry, other.NTSGeometry);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Geometry other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (NTSGeometry != null ? NTSGeometry.GetHashCode() : 0);
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator ==(Geometry left, Geometry right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        public static bool operator !=(Geometry left, Geometry right)
        {
            return !Equals(left, right);
        }
    }
}
