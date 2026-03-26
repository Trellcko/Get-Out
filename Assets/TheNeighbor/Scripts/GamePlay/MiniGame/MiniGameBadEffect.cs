using DG.Tweening;
using Trellcko.Core.Audio;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class MiniGameBadEffect : MonoBehaviour
    {
        private Sequence _corpseEffectSequence;
        private Vignette _vignette;

        private ISoundController _soundController;

        [Inject]
        private void Construct(ISoundController soundController)
        {
            _soundController = soundController;
        }
        
        public void PlayCorpseEffect(Volume volume)
        {
            _soundController.PlayOtherSound(OtherSound.ThrowingUp);
            volume.profile.TryGet(out _vignette);
            _corpseEffectSequence?.Kill();
            _vignette.color.value = Color.black;
            _vignette.intensity.value = 0.402f;
            _corpseEffectSequence = DOTween.Sequence();
            _corpseEffectSequence
                .Append(DOTween.To(
                    () => _vignette.color.value,
                    x => _vignette.color.value = x,
                    Color.red,
                    0.5f
                )).Join(
                    DOTween.To(
                        () => _vignette.intensity.value,
                        x => _vignette.intensity.value = x,
                        0.652f,
                        0.5f))
                .AppendInterval(0.25f)
                .Append(DOTween.To(
                    () => _vignette.color.value,
                    x => _vignette.color.value = x,
                    Color.black,
                    0.5f))
                .Join(DOTween.To(
                    () => _vignette.intensity.value,
                    x => _vignette.intensity.value = x,
                    0.402f,
                    0.5f));
        }
    }
}