﻿using System;

namespace Lib.Geometry.Abstractions
{
    /// <summary>
    /// A geometry
    /// </summary>
    public interface IGeometry : IEquatable<IGeometry>
    {
        /// <summary>
        /// True if the input geometry is inside the geometry, including borders
        /// </summary>
        bool Contains(IGeometry input);

        /// <summary>
        /// True if the input geometry is inside the geometry, excluding borders
        /// </summary>
        bool Covers(IGeometry input);
    }
}
