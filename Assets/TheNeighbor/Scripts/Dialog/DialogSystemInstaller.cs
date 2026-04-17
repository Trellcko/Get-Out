using UnityEngine;
using Zenject;

namespace Trellcko.Dialog
{
    public class DialogSystemInstaller : MonoInstaller
    {
        [SerializeField] private DialogSystem _dialogSystem;
        
        public override void InstallBindings()
        {
            Container.Bind<IDialogSystem>().FromInstance(_dialogSystem);
        }
    }
}