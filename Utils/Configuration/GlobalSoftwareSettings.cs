namespace Isogeo.Utils.Configuration
{
    public class GlobalSoftwareSettings
    {
        public int NbResult { get; }

        public string EncryptCode { get; }

        public GlobalSoftwareSettings(int nbResults, string encryptCode)
        {
            NbResult = nbResults;
            EncryptCode = encryptCode;
        }
    }
}
