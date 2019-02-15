namespace Lib.Geometry.Abstractions
{
    /// <summary>
    /// A 2D point
    /// </summary>
    public interface IPoint : IGeometry
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
