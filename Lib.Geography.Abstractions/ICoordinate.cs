using System;

namespace Lib.Geography.Abstractions
{
    /// <summary>
    /// A 2D coordinate
    /// </summary>
    public interface ICoordinate : IEquatable<ICoordinate>
    {
        /// <summary>
        /// Latitude
        /// </summary>
        double Latitude { get; }

        /// <summary>
        /// Longitude
        /// </summary>
        double Longitude { get; }
    }
}
