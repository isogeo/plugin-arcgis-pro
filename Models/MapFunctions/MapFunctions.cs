using System;
using System.Globalization;
using System.IO;
using System.Linq;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Mapping;


namespace Isogeo.Models.MapFunctions
{
    internal static class MapFunctions
    {

        public static string EnvelopeToString(Envelope envelope)
        {
            return envelope.XMin.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "," +
                   envelope.YMin.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "," +
                   envelope.XMax.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "," +
                   envelope.YMax.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
        }

        public static string GetMapExtent()
        {
            return EnvelopeToString(MapView.Active.Extent);
        }

    }
}
