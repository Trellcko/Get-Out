using UnityEngine;

namespace Trellcko.Core.Input
{
    public class CursorController : ICursorController
    {
        public bool CursorLocked => Cursor.lockState == CursorLockMode.Locked;
        
        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        public void HideCursor()
        {
            Cursor.visible = false;
        }
        
        public void ShowCursor()
        {
            Cursor.visible = true;
        }
    }
}