using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

[Serializable]
public class Question
{
    public string question;
    public QuestionType questionType;
    public bool likertIs5;
    public string likertLowestText;
    public string likertHighestText;
    public List<string> multiAnswers;
    public int numberScaleMax;
    public int numberScaleMin;
    public string numPadDefaultValue;

}

public enum QuestionType
{
    Likert,
    NumberScale,
    MultiChoice,
    NumberPad,
    OpenQuestion
}