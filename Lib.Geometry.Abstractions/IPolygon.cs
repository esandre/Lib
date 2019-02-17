using System;
using System.Collections.Generic;

namespace Lib.Geometry.Abstractions
{
    /// <summary>
    /// A contiguous polygon
    /// </summary>
    public interface IPolygon : IGeometry, IEquatable<IPolygon>
    {
        /// <summary>
        /// Polygon exterior envelope
        /// </summary>
        IEnumerable<IPoint> Shell { get; }

        /// <summary>
        /// Polygon eventual holes
        /// </summary>
        IEnumerable<IEnumerable<IPoint>> Holes { get; }
    }
}
