using System.Windows.Input;

namespace Isogeo.AddIn.Models
{
    public static class WpfHelper
    {
        // Commands only checked whenever interaction occurs on the UI.
        // This Method force the Front to Check Commands
        // https://stackoverflow.com/a/783121/12319802
        public static void ForceFrontToCheckCommands()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
