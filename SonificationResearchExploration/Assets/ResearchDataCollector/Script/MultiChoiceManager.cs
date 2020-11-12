using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiChoiceManager : MonoBehaviour
{
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public TextMeshProUGUI button3Text;
    public TextMeshProUGUI button4Text;
    public List<TextMeshProUGUI> buttonList = new List<TextMeshProUGUI>();
    public TMP_Dropdown dropdown;
    TempAnswerHolder tempAnsw;
    List<string> answerOptionsLocal;
    // Start is called before the first frame update
    private void Awake()
    {
        tempAnsw = GameObject.Find("NextQuestion").GetComponent<TempAnswerHolder>();
        buttonList.Add(button1Text);
        buttonList.Add(button2Text);
        buttonList.Add(button3Text);
        buttonList.Add(button4Text);

    }
    public void SetAnswers(List<string> anwserOptions)
    {
        foreach (var item in buttonList)
        {
            item.transform.parent.gameObject.SetActive(false);
        }
        dropdown.gameObject.SetActive(false);
        answerOptionsLocal = anwserOptions;
        if (anwserOptions.Count <=4)
        {
            int answerCounter = 0;
            foreach (string answer in anwserOptions)
            {
                buttonList[answerCounter].transform.parent.gameObject.SetActive(true);
                buttonList[answerCounter].text = answer;
                answerCounter += 1;
            }

        }
        else
        {
            dropdown.ClearOptions();
            dropdown.gameObject.SetActive(true);
            foreach (string answer in anwserOptions)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData() {text=answer });
            }
            
        }

    }

    public void AnswerQuestion(TextMeshProUGUI answer)
    {       
        foreach (var button in buttonList)
        {
            button.transform.parent.gameObject.SetActive(false);
        }
    }

    public void AnswerQuestion(TMP_Dropdown answer)
    {
        tempAnsw.SetAnswer(answerOptionsLocal[answer.value]);
    }
}
