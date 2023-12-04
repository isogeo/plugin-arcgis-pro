﻿using Isogeo.Models.Configuration;
using Search = Isogeo.Models.API.Search;

namespace Isogeo.Models
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static class Variables
    {
        public static ConfigurationManager configurationManager;

        public static readonly string EncryptCode = "alo(-'oàkd:jdthe";

        public static Search search = new();

        public static readonly int NbResult = 10;

        public static string searchText;

    }
}
