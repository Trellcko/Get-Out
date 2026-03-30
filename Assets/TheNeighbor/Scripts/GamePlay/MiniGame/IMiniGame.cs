using System;
using Trellcko.Gameplay.Interactable;

namespace Trellcko.Gameplay.MiniGame
{
    public interface IMiniGame
    {
        bool IsPlaying { get; }
        MiniGameType MinigameType { get; }
        void StartGame(MiniGamesParamsHolder param = null);
        void FinishGame(bool success);
        event Action<bool, IMiniGame> Finished;
        void ExitGame();
    }
}