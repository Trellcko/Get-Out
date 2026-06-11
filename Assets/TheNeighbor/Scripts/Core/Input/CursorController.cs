using UnityEngine;

namespace Trellcko.Core.Input
{
    public class CursorController : ICursorController
    {
        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        public void ShowCursor()
        {
            Cursor.visible = true;
        }
    }
}