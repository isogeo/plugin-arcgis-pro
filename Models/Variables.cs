using Isogeo.Models.Configuration;
using Search = Isogeo.Models.API.Search;

namespace Isogeo.Models
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static class Variables
    {
        public static ConfigurationManager configurationManager;

        public static List<Action> functionsSetlist = new();

        public static string encryptCode = "alo(-'oàkd:jdthe";
        public static Search search = new();
        public static bool listLoading = false;

        public static int nbResult = 10;

        public static string searchText;

    }
}
