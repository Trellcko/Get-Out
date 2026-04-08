using System;
using System.Collections;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

public class CorridorTikTok : MonoBehaviour
{
    [SerializeField] private Phone phone;
    [SerializeField] private Door door;


    private void OnEnable()
    {
        StartCoroutine(Corun());
    }

    private IEnumerator Corun()
    {
        door.PlayKnockingSound(true);
        Debug.Log("Stop Knocking");
        yield return new WaitForSeconds(4F);
        Debug.Log("Start Ringing");
        door.StopKnockingSound();
        phone.Activate();
        phone.InteractionFinished += OnInteractionFinished;
    }

    private void OnInteractionFinished()
    {
        Debug.Log("Stop Interaction");
        door.TryInteract(out QuestItem _, QuestItem.None);
        
    }
}
