using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Geometry.NTS;
using IGeometry = Lib.Geometry.Abstractions.IGeometry;
using IPolygon = Lib.Geometry.Abstractions.IPolygon;
using INTSPolygon = GeoAPI.Geometries.IPolygon;
using IPoint = Lib.Geometry.Abstractions.IPoint;

namespace Lib.Geometry
{
    /// <inheritdoc />
    public class Polygon : IPolygon
    {
        private readonly Lazy<INTSPolygon> _ntsPolygon;

        internal Polygon(INTSPolygon polygon)
        {
            Shell = NTSConverter.FromLinearRing(polygon.Shell);
            Holes = polygon.Holes.Select(NTSConverter.FromLinearRing).ToArray();

            _ntsPolygon = new Lazy<INTSPolygon>(() => polygon);
        }

        /// <inheritdoc />
        public Polygon(params IPoint[] shell)
        {
            Shell = shell;
            Holes = new IEnumerable<IPoint>[0];
            
            _ntsPolygon = new Lazy<INTSPolygon>(() => NTSConverter.FromEnvelope(Shell));
        }

        /// <inheritdoc />
        public IEnumerable<IPolygon> Polygons => new[] {this};

        /// <inheritdoc />
        public bool Contains(IGeometry input) => _ntsPolygon.Value.Contains(input.ToNTS());

        /// <inheritdoc />
        public bool Covers(IGeometry input) => _ntsPolygon.Value.Covers(input.ToNTS());

        /// <inheritdoc />
        public IEnumerable<IPoint> Shell { get; }

        /// <inheritdoc />
        public IEnumerable<IEnumerable<IPoint>> Holes { get; }

        /// <inheritdoc />
        public bool Equals(IGeometry other) => other is IPolygon polygon && Equals(polygon);

        /// <inheritdoc />
        public bool Equals(IPolygon other) => _ntsPolygon.Value.EqualsExact(other.ToNTS());

        private bool Equals(Polygon other) => _ntsPolygon.Value.EqualsExact(other._ntsPolygon.Value);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is Polygon other) return Equals(other);
            return obj is IPolygon polygon && Equals(polygon);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _ntsPolygon.GetHashCode();
    }
}
