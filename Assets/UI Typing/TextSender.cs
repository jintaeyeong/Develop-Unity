using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSender : MonoBehaviour
{
    [Header("Dialogs")]
    public string[] dialogStrings;
    public TextMeshProUGUI textObj;
    void Start()
    {
        TypingManager.Instance.Typing(dialogStrings,textObj);
    }

    
}
