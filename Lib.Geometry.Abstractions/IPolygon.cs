using System.Collections.Generic;

namespace Lib.Geometry.Abstractions
{
    /// <summary>
    /// A contiguous polygon without holes
    /// </summary>
    public interface IPolygon : IGeometry
    {
        /// <summary>
        /// Polygon exterior envelope
        /// </summary>
        IEnumerable<IPoint> Envelope { get; }
    }
}
