using System;
using System.Collections.Generic;
using LinguaLeo.Scripts.Manegers;
using URandom = UnityEngine.Random;

namespace LinguaLeo.Scripts.Helpers
{
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

        /// <summary>
        /// заполнит варианты ответов
        /// </summary>
        public void FillInAnswers(int answerCount)
        {
            int[] numAnswers = { 0, 1, 2, 3, 4 };
            int indexOfQuestWord = URandom.Range(0, answerCount);

            List<WordLeo> GroupWords = GameManager.WordManeger.GetAllGroupWords();
            Stack<WordLeo> tempAnswers = FillRandomStack(GroupWords, answerCount);
            answers = new List<WordLeo>(answerCount);
            foreach (var item in numAnswers)
            {
                if (item == indexOfQuestWord)
                {
                    answers.Add(questWord);
                    continue;
                }
                if (tempAnswers.Peek() == questWord)
                    tempAnswers.Pop();
                answers.Add(tempAnswers.Pop());
            }
            answers = MyUtilities.ShuffleList(answers);
        }

        /// <summary>
        /// Заполнить стек случайным образом
        /// </summary>
        /// <param name="words"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private Stack<WordLeo> FillRandomStack(List<WordLeo> words, int count)
        {
            Stack<WordLeo> stack = new Stack<WordLeo>();
            List<WordLeo> wordsTemp = new List<WordLeo>(words);
            wordsTemp = MyUtilities.ShuffleList(wordsTemp);
            System.Random random = new System.Random();
            while (stack.Count < count)
            {
                int randomIndex = random.Next(wordsTemp.Count);
                if (!stack.Contains(wordsTemp[randomIndex]))
                {
                    stack.Push(wordsTemp[randomIndex]);
                    wordsTemp.RemoveAt(randomIndex);
                }
            }
            return stack;
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
}
