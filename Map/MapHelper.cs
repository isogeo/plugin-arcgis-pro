using ArcGIS.Desktop.Mapping;
using System.Linq;
using Isogeo.Map.Models;

namespace Isogeo.Map
{
    public static class MapHelper
    {
        public static bool HasActiveMap() => MapView.Active?.Map != null;

        public static bool IsogeoDataHasInvalidUrl(IsogeoData isogeoData) => string.IsNullOrWhiteSpace(isogeoData?.Url);

        public static bool IsIsogeoDataUsingSdeFileButMissingFilePath(IsogeoData isogeoData)
        {
            var databaseTypes = new[] { "ARCSDE", "POSTGIS" };
            return databaseTypes.Contains(isogeoData.Type?.ToUpper())
                   && string.IsNullOrWhiteSpace(isogeoData.Url);
        }
    }
}
