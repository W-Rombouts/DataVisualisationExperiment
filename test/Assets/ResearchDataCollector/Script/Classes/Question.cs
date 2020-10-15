using System;
using System.Collections.Generic;

[Serializable]
public class Question
{
    public string question;
    public QuestionType questionType;
    public bool likertIs5;
    public List<string> multiAnswers;
    public int numberScaleMax;
    public int numberScaleMin;
    public string numPadDefaultValue;

}

public enum QuestionType
{
    Likert,
    NumberScale,
    MultiChoise,
    NumberPad,
    OpenQuestion
}