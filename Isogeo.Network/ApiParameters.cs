using Isogeo.Models.API;

namespace Isogeo.Network
{
    public class ApiParameters
    {
        internal string ob;
        internal string od;
        internal int offset;
        internal string lang;
        internal string rel;
        internal Token token;
        internal string query;
        internal string box;

        public ApiParameters(
            Token token,
            string query = "",
            string ob = "",
            string od = "",
            int offset = 0,
            string rel = "",
            string box = "")
        {
            this.query = query;
            this.token = token;
            this.ob = ob;
            this.od = od;
            this.rel = rel;
            this.offset = offset;
            lang = Thread.CurrentThread.CurrentCulture.Name;
            this.box = box;
        }
    }
}
