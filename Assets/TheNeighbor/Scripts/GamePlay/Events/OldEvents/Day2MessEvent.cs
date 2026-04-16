using Trellcko.Core.Audio;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.Events
{
    public class Day2MessEvent : BaseEvent
    {
        [SerializeField] private GameObject[] _mess;
        [SerializeField] private GameObject _monster;
        [SerializeField] private Transform _monsterPoint;
        
        [SerializeField] private Light[] _lights;
        [SerializeField] private Light _lightAtTheEnd;
        private ISoundController _soundController;

        [Inject]
        private void Construct(ISoundController soundController)
        {
            _soundController = soundController;
        }

        protected override void OnBeforeNotifierInvoked()
        {
            _soundController.PlayShockMoment();
        }

        protected override void OnMakeVisible()
        {
            foreach (GameObject mess in _mess)
            {
                mess.SetActive(true);
            }
            _lightAtTheEnd.enabled = false;
            _monster.SetActive(true);
            _monster.transform.position = _monsterPoint.position;
        }

        protected override void OnNotifierInvokeHandle()
        {
            foreach (GameObject mess in _mess)
            {
                mess.SetActive(false);
            }
            _monster.SetActive(false);
            _monster.transform.position = _monsterPoint.position;
            foreach (Light light1 in _lights)
            {
                light1.enabled = false;   
            }
        }
    }
}