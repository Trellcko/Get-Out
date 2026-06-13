namespace Trellcko.Core.Input
{
    public interface ICursorController
    {
        void LockCursor();
        void UnlockCursor();
        void ShowCursor();
        void HideCursor();
        bool CursorLocked { get; }
    }
}