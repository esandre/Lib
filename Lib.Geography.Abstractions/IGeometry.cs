using System;
using System.Collections.Generic;

namespace Lib.Geography.Abstractions
{
    /// <summary>
    /// A geometry
    /// </summary>
    public interface IGeometry : IEquatable<IGeometry>
    {
        IEnumerable<IContinguousGeometry> Parts { get; }

        /// <summary>
        /// True if point is inside the geometry, including borders
        /// </summary>
        bool Contains(ICoordinate point);

        /// <summary>
        /// True if point is inside the geometry, excluding borders
        /// </summary>
        bool Covers(ICoordinate point);
    }
}
