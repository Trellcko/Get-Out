using Trellcko.Core.Audio;
using UnityEngine;
using Zenject;

namespace Trellcko.MainMenu
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private Trellcko.Gameplay.Interactable.Door _door;
        
        private ISoundController _soundController;

        [Inject]
        private void Construct(ISoundController soundController)
        {
            _soundController = soundController;
        }

        private void Awake()
        {
            _soundController.PlayAmbience(Ambience.MainMenu);
            _door.PlayKnockingSound(true);
        }
    }
}