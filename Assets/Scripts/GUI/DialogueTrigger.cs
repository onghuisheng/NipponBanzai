using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{

    public string m_DialogueName;

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

    public static void DoDialogue(string dialogueName)
    {
        var dialogues = FindObjectsOfType<DialogueTrigger>();

        foreach (var dialogue in dialogues)
        {
            if (dialogue.m_DialogueName == dialogueName)
            {
                dialogue.TriggerDialogue();
                return;
            }
        }

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
