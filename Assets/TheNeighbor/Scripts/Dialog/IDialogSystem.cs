namespace Trellcko.Dialog
{
    public interface IDialogSystem
    {
        void ShowDialog(DialogData dialogData, bool useAudio = true);
    }
}