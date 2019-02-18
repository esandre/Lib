using Lib.Geometry.Abstractions;
using Lib.Geometry.NTS;
using Lib.Patterns;
using NetTopologySuite.IO;

namespace Lib.Geometry.WKT
{
    /// <summary>
    /// Returns a geometry WKT representation
    /// </summary>
    public class WktFromGeometryFactory : IFactory<IGeometry, string>
    {
        private readonly WKTWriter _writer = new WKTWriter();

        /// <inheritdoc />
        public string Factory(IGeometry input)
        {
            return _writer.Write(input.ToNTS());
        }
    }
}
