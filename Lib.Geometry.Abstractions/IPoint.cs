using System;

namespace Lib.Geometry.Abstractions
{
    /// <summary>
    /// A 2D point
    /// </summary>
    public interface IPoint : IPolygon, IEquatable<IPoint>
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        double X { get; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        double Y { get; }
    }
}
