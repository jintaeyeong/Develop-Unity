using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TypingManager : MonoBehaviour
{
    public static TypingManager Instance;

    [Header("Times for each character")]
    public float TimeForCharacter = 0.08f; // 0.08이 기본

    [Header("Times for each character when speed up")]
    public float TimeForCharacter_Fast = 0.03f; // 0.03이 빠른 텍스트

    // 실제 적용되는 문자열 속도
    private float characterTime;

    private string[] dialogsSave;
    private TextMeshProUGUI tmpSave;

    public static bool isDialogEnd;

    private bool isTypingEnd = false; // 타이핑이 끝났는가?
    private int dialogNumber = 0; // 대화 문단 숫자

    // 내부적으로 돌아가는 시간 타이머
    private float timer;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if(Instance == null)
        {
            Instance = this;
        }
        // 타자 효과가 반복되는 시간을 맞추는 변수
        timer = TimeForCharacter;

        // 실제로 적용되는 타자 시간
        characterTime = TimeForCharacter;
    }

    public void Typing(string[] dialogs, TextMeshProUGUI textobj)
    {
        isDialogEnd = false;
        dialogsSave = dialogs;
        tmpSave = textobj;

        if (dialogNumber < dialogs.Length)
        {
            char[] chars = dialogs[dialogNumber].ToCharArray();
            StartCoroutine(Typer(chars, textobj));
        }
        else
        {
            // 문장이 끝나서 다른 문장을 받을 준비함(초기화)
            tmpSave.text = "";
            isDialogEnd = true; // 호출자는 이것을 보고 다음 동작을 진행 
            dialogsSave = null;
            tmpSave = null;
            dialogNumber = 0;
        }
    }

    IEnumerator Typer(char[] chars, TextMeshProUGUI textObj)
    {
        int currentChar = 0;
        int charLength = chars.Length;
        isTypingEnd = false;

        while (currentChar < charLength)
        {
            if(timer >= 0)
            {
                yield return null;
                timer -= Time.deltaTime;
            }
            else
            {
                textObj.text += chars[currentChar].ToString();
                currentChar++;
                timer = characterTime; 
            }
        }
        if(currentChar >= charLength)
        {
            isTypingEnd = true;
            dialogNumber++;
            yield break;
        }    
    }

    public void GetInputDown()
    {
        if(dialogsSave != null)
        {
            if(isTypingEnd)
            {
                tmpSave.text = "";
                Typing(dialogsSave, tmpSave);
            }
            else
            {
                characterTime = TimeForCharacter_Fast;
            }
        }
    }

    public void GetInputUp()
    {
        if(dialogsSave != null)
        {
            characterTime = TimeForCharacter;
        }
    }

}
