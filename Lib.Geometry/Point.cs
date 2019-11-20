using System;
using System.Collections.Generic;
using Lib.Geometry.Abstractions;
using NTSPoint = NetTopologySuite.Geometries.Point;
using NTSCoordinate = NetTopologySuite.Geometries.Coordinate;

namespace Lib.Geometry
{
    /// <summary>
    /// A point
    /// </summary>
    public class Point : IPoint
    {
        /// <summary>
        /// A point
        /// </summary>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        internal Point(NTSPoint point)
        {
            X = point.X;
            Y = point.Y;
        }

        internal Point(NTSCoordinate point)
        {
            X = point.X;
            Y = point.Y;
        }

        /// <inheritdoc />
        public IEnumerable<IPolygon> Polygons => new[] {this};

        /// <inheritdoc />
        public bool Contains(IGeometry input) => input is IPoint point 
                                                    && Math.Abs(point.X - X) < 0 
                                                    && Math.Abs(point.Y - Y) < 0;

        /// <inheritdoc />
        public bool Covers(IGeometry input) => false;

        /// <inheritdoc />
        public double X { get; }

        /// <inheritdoc />
        public double Y { get; }

        /// <inheritdoc />
        public IEnumerable<IPoint> Shell => new[] {this};

        /// <inheritdoc />
        public IEnumerable<IEnumerable<IPoint>> Holes => new IEnumerable<IPoint>[0];

        /// <inheritdoc />
        public bool Equals(IGeometry other) => other is IPoint point && Equals(point);

        /// <inheritdoc />
        public bool Equals(IPolygon other) => other is IPoint point && Equals(point);

        /// <inheritdoc />
        public bool Equals(IPoint other) => X.Equals(other?.X) && Y.Equals(other?.Y);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Point other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }
    }
}
