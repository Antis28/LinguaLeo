using System;
using System.Collections.Generic;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using URandom = UnityEngine.Random;

namespace LinguaLeo.Scripts.Workout
{
    public class Reiteration : MonoBehaviour, IObserver, IWorkout
    {
        #region Static Fields and Constants

        private const int QUEST_COUNT = 10;
        private const int ANSWER_COUNT = 5;

        #endregion

        #region SerializeFields

        [SerializeField]
        private Text questionText = null; // Поле для вопроса

        [SerializeField]
        private Text transcriptText = null; // Поле для транскрипции

        [SerializeField]
        private Image wordImage = null; // Картинка ассоциаци со словом

        [SerializeField]
        private Image progressImage = null; // Картинка прогесса изучения слова

        [SerializeField]
        private Toggle sayToggle = null; // checkbox для автопроизношения

        [SerializeField]
        private Slider scoreSlider = null; // Протренировано слов прогресс

        [SerializeField]
        private Text scoreText = null; // Текст числа протренированых слов

        [SerializeField]
        private int answersCount; // Число протренированых слов

        [SerializeField]
        private Text contextText = null; //Текст для контекста

        #endregion

        #region Private variables

        private GameObject contextPanel; //Панель для контекста

        private List<QuestionLeo> questions;
        private int questionID;

        private bool trainingСompleted;

        private ButtonsHandler buttonsHandler;

        private List<WordLeo> untrainedWords;

        WorkoutNames IWorkout.WorkoutName
        {
            get { return WorkoutNames.reiteration; }
        }

        #endregion

        #region Events

        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {
                case GAME_EVENTS.BuildTask:
                    WordProgressUpdate();
                    ProgeressUpdate();
                    HideImage();
                    break;
                case GAME_EVENTS.LoadedVocabulary:
                    LoadTasks();
                    BuildTask(0);
                    FindObjectOfType<DebugUI>().FillPanel(questions);
                    break;
                case GAME_EVENTS.ShowResult:
                    ShowImage();
                    WordProgressUpdate();
                    ShowContext();
                    buttonsHandler.SetNextQuestion(() => BuildTask(questionID + 1));
                    break;
            }
        }

        #endregion

        #region Public Methods

        public void ClearTextInQestion()
        {
            questionText.text = "";
        }


        public QuestionLeo GetCurrentQuest()
        {
            return questions[questionID];
        }

        public void HideImage()
        {
            wordImage.enabled = false;
        }

        public void SetContext(string context)
        {
            if (context != string.Empty)
                contextText.text = context;
            else
                contextText.text = "(нет контекста)";
        }

        public void SetImage(string fileName)
        {
            var sprite = GameManager.ResourcesLoader.GetPicture(fileName);
            wordImage.sprite = sprite;
            wordImage.preserveAspect = true;
        }

        public void SetQuestion(string quest)
        {
            questionText.text = quest;
        }

        public void SetSound(string file)
        {
            GameManager.AudioPlayer.SetSound(MyUtilities.ConverterUrlToName(file, false));
            if (sayToggle.isOn)
                GameManager.AudioPlayer.SayWord();
        }

        public void SetTranscript(string transcript)
        {
            transcriptText.text = transcript;
        }

        public void ShowContext()
        {
            contextPanel.SetActive(true);
        }

        public void ShowImage()
        {
            wordImage.enabled = true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Добавляет слова к существующему списку слов
        /// до ANSWER_COUNT * 2
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private List<WordLeo> AddWordsForAnswers(List<WordLeo> words)
        {
            List<WordLeo> TempWords = new List<WordLeo>(ANSWER_COUNT * 2);
            if (words != null)
                TempWords.AddRange(words);
            List<WordLeo> allWords = GameManager.WordManager.GetAllWords();

            //TODO: Заменить на случайный индекс
            allWords = ShuffleList(allWords);
            int index = 0;
            while (TempWords.Count < ANSWER_COUNT * 2) { TempWords.Add(allWords[index++]); }

            return TempWords;
        }


        // Use this for initialization
        private void Awake()
        {
            GameManager.Notifications.AddListener(this, GAME_EVENTS.ShowResult);
            GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
            GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);

            contextPanel = contextText.transform.parent.gameObject;
            buttonsHandler = FindObjectOfType<ButtonsHandler>();
        }

        private void BuildTask(int current)
        {
            buttonsHandler.ClearTextInButtons();

            if (trainingСompleted || questions.Count == 0)
            {
                GameManager.Notifications.PostNotification(this, GAME_EVENTS.WordsEnded);
                return;
            }

            questionID = FindNodeByID(current);
            if (questionID < 0)
            {
                Debug.LogError(this + "отсутствует или указан неверно идентификатор узла.");
                return;
            }

            int toNode = questionID + 1;
            if (questions.Count <= toNode)
            {
                toNode = 0;
                trainingСompleted = true;
            }

            QuestionLeo questionLeo = questions[questionID];

            // добавление слова для перевода
            string questionWord = questionLeo.questWord.wordValue;
            SetQuestion(questionWord);
            SetTranscript(questionLeo.questWord.transcription);

            //TODO: заполнять все кнопки одновременно
            List<string> answers = new List<string>(ANSWER_COUNT);
            foreach (WordLeo item in questionLeo.answers)
                answers.Add(item.wordValue);

            buttonsHandler.FillingButtonsWithOptions(answers, questionWord);
            buttonsHandler.FillingEnterButton(true);

            SetImage(questionLeo.questWord.pictureUrl);
            SetSound(questionLeo.questWord.soundUrl);
            SetContext(questionLeo.questWord.highlightedContext);
            HideContext();

            // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
            EventSystem.current.SetSelectedGameObject(gameObject);
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.BuildTask);
        }

        private static void FillAnswers(QuestionLeo questionLeo, Stack<WordLeo> answers)
        {
            int[] numAnswers = {0, 1, 2, 3, 4};
            int indexOfQuestWord = URandom.Range(0, ANSWER_COUNT);
            questionLeo.answers = new List<WordLeo>(ANSWER_COUNT);

            foreach (var item in numAnswers)
            {
                // номер совпал с индексом для ответа
                if (item == indexOfQuestWord)
                {
                    questionLeo.answers.Add(questionLeo.questWord);
                    continue;
                }

                // пропустить повтор ответа для задания
                if (answers.Count != 0 && answers.Peek() == questionLeo.questWord)
                    answers.Pop();

                //TODO: заполнять варианты ответов из общего словаря
                if (answers.Count != 0)
                    questionLeo.answers.Add(answers.Pop());
            }
        }

        /// <summary>
        /// заполнить варианты ответов
        /// </summary>
        /// <param name="questionLeo"></param>
        private void FillAnswersForQuestion(QuestionLeo questionLeo)
        {
            List<WordLeo> words = GameManager.WordManager.GetWordsWithLicense();
            Stack<WordLeo> answers = null;

            if (words.Count <= ANSWER_COUNT * 2)
            {
                words = AddWordsForAnswers(words);
                //words = GameManager.WordManager.GetAllWords();
            }

            answers = PrepareAnswers(words, ANSWER_COUNT);
            FillAnswers(questionLeo, answers);
            questionLeo.answers = ShuffleList(questionLeo.answers);
        }

        /// <summary>
        /// Поиск вопроса по ID 
        /// </summary>
        /// <param name="i">ID для поиска</param>
        /// <returns></returns>
        private int FindNodeByID(int i)
        {
            int j = 0;
            foreach (var quest in questions)
            {
                if (quest.id == i)
                    return j;
                j++;
            }

            return -1;
        }

        private QuestionLeo GeneratorTask(int id, List<QuestionLeo> exceptWords)
        {
            QuestionLeo questionLeo = new QuestionLeo();
            questionLeo.id = id;

            //if (words.GroupExist())
            untrainedWords = ShuffleList(untrainedWords);
            questionLeo.questWord = GetNewWord(exceptWords, untrainedWords);

            if (questionLeo.questWord == null)
            {
                Debug.LogWarning("Уникальных слов нет");
                return null;
            }

            FillAnswersForQuestion(questionLeo);

            return questionLeo;
        }

        Workout IWorkout.GetCore()
        {
            throw new NotImplementedException();
        }

        WordLeo IWorkout.GetCurrentWord()
        {
            return questions[questionID].questWord;
        }

        /// <summary>
        /// Найти слово которого нет в списке
        /// </summary>
        /// <param name="exceptWords"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        private static WordLeo GetNewWord(List<QuestionLeo> exceptWords, List<WordLeo> words)
        {
            foreach (var item in words)
            {
                if (!exceptWords.Contains(new QuestionLeo(item))) { return item; }
            }

            return null;
        }

        private void HideContext()
        {
            contextPanel.SetActive(false);
        }

        private void LoadTasks()
        {
            questions = new List<QuestionLeo>(QUEST_COUNT);
            untrainedWords = GameManager.WordManager.GetWordsWithLicense();

            for (int i = 0; i < QUEST_COUNT; i++)
            {
                QuestionLeo question = GeneratorTask(i, questions);

                if (question == null)
                    break;
                questions.Add(question);
            }
        }

        /// <summary>
        /// Заполнить стек случайным образом
        /// </summary>
        /// <param name="words"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private Stack<WordLeo> PrepareAnswers(List<WordLeo> words, int count)
        {
            Stack<WordLeo> stack = new Stack<WordLeo>();
            List<WordLeo> wordsTemp = new List<WordLeo>(words);

            System.Random random = new System.Random();
            while (stack.Count < count && stack.Count < wordsTemp.Count)
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

        private void ProgeressUpdate()
        {
            answersCount++;
            scoreText.text = answersCount + "/" + scoreSlider.maxValue;
            scoreSlider.value = answersCount;
        }

        /// <summary>
        /// перемешать слова
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private List<WordLeo> ShuffleList(List<WordLeo> words)
        {
            List<WordLeo> list = new List<WordLeo>(words);

            System.Random random = new System.Random();

            // для избежания зацикливания установлен максимум попыток
            //int maxAttempts = words.Count * 2;
            //int numberAttempts = 0;

            //foreach (var item in words)
            //{
            //    int randomIndex = 0; 
            //    do
            //    {
            //        numberAttempts++;
            //        if (numberAttempts > maxAttempts)
            //            goto ToEndLoop;

            //        randomIndex = random.Next(words.Count); 
            //    } while (list.Contains(words[randomIndex]));

            //    list[randomIndex] = item;
            //    continue;

            //    ToEndLoop:
            //    list.Add(item);
            //}

            for (int i = list.Count; i > 1; i--)
            {
                int j = random.Next(i);
                list.Add(list[j]);
                list.RemoveAt(j);
            }

            return list;
        }

        /// <summary>
        /// показывает прогресс изучения слова
        /// </summary>
        private void WordProgressUpdate()
        {
            progressImage.fillAmount = GetCurrentQuest().questWord.GetProgressCount();
        }

        #endregion
    }
}
