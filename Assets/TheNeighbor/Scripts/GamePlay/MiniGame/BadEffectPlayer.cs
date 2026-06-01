using DG.Tweening;
using Trellcko.Core.Audio;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class BadEffectPlayer : MonoBehaviour
    {
        private Sequence _effectSequence;
        private Vignette _vignette;

        private ISoundController _soundController;

        [Inject]
        private void Construct(ISoundController soundController)
        {
            _soundController = soundController;
        }

        public void PlayOuchEffect(Volume volume)
        {
            _soundController.PlayOtherSound(OtherSound.Ouch);
            EffectVisual(volume);
        }
        
        public void PlayThrowingUpEffect(Volume volume)
        {
            _soundController.PlayOtherSound(OtherSound.ThrowingUp);
            EffectVisual(volume);
        }

        private void EffectVisual(Volume volume)
        {
            volume.profile.TryGet(out _vignette);
            _effectSequence?.Kill();
            _vignette.color.value = Color.black;
            _vignette.intensity.value = 0.402f;
            _effectSequence = DOTween.Sequence();
            _effectSequence
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