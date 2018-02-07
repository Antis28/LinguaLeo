using System;
using System.Collections.Generic;


public class QuestionLeo : IEquatable<QuestionLeo>
{
    public int id;
    public WordLeo questWord;
    public List<WordLeo> answers;

    public QuestionLeo() { }
    public QuestionLeo(WordLeo word)
    {
        questWord = word;
    }
    #region сравнение в методе Contains
    public bool Equals(QuestionLeo other)
    {
        if (other == null)
            return false;

        if (this.questWord.wordValue == other.questWord.wordValue)
            return true;
        else
            return false;
    }
    public bool Equals(WordLeo other)
    {
        if (other == null)
            return false;

        if (this.questWord.wordValue == other.wordValue)
            return true;
        else
            return false;
    }

    public override bool Equals(Object obj)
    {
        if (obj == null)
            return false;

        QuestionLeo quest = obj as QuestionLeo;
        if (quest == null)
            return false;
        else
            return Equals(quest);
    }

    public override int GetHashCode()
    {
        return this.questWord.wordValue.GetHashCode();
    }

    public static bool operator ==(QuestionLeo quest1, QuestionLeo quest2)
    {
        if (((object)quest1) == null || ((object)quest2) == null)
            return Object.Equals(quest1, quest2);

        return quest1.Equals(quest2);
    }

    public static bool operator !=(QuestionLeo quest1, QuestionLeo quest2)
    {
        if (((object)quest1) == null || ((object)quest2) == null)
            return !Object.Equals(quest1, quest2);

        return !(quest1.Equals(quest2));
    }
    #endregion    
}
