using System;
using System.Collections;
using Trellcko.Core.Audio;
using Trellcko.Gameplay;
using Trellcko.Gameplay.House;
using Trellcko.Gameplay.MiniGame;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class TurnOffLight : MonoBehaviour
{
    [SerializeField] private Wall[] _walls1;
    [SerializeField] private Wall[] _allWalls;
    [SerializeField] private Light[] _lights;
    
    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            foreach (Wall wall in _walls1)
            {
                wall.ShowGlitchWall();
            }
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            foreach (Wall wall in _walls1)
            {
                wall.HideGlitchWall();
            }
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            foreach (Wall wall in _allWalls)
            {
                wall.ShowGlitchWall();
            }

            foreach (Light light1 in _lights)
            {
                light1.color = Color.red;
            }
        }
    }
}
