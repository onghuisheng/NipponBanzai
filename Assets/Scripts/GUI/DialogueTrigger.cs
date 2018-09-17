using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DemDialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;

    public bool DestroyOnTrigger = true;

    private DialogueHandler dialogueHandler;

    void Start()
    {
        dialogueHandler = GameObject.FindWithTag("DialogueHandler").GetComponent<DialogueHandler>();
    }

    public void TriggerDialogue()
    {
        dialogueHandler.StartDialogue(dialogue);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TriggerDialogue();

            if (DestroyOnTrigger)
                Destroy(gameObject);
        }
    }

}
