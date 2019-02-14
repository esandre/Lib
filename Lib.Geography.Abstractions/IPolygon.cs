using System.Collections.Generic;

namespace Lib.Geography.Abstractions
{
    /// <summary>
    /// A contiguous polygon
    /// </summary>
    public interface IPolygon : IGeometry
    {
        /// <summary>
        /// Polygon exterior envelope
        /// </summary>
        IEnumerable<ICoordinate> Envelope { get; }

        /// <summary>
        /// Eventual polygon holes
        /// </summary>
        IEnumerable<IPolygon> Holes { get; }
    }
}
