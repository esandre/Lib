﻿namespace Lib.Geography
{
    /// <summary>
    /// A 2D coordinate
    /// </summary>
    public interface ICoordinate
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
