using LinguaLeo._Adapters;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Managers.Parts;
using LinguaLeo.Scripts.Workout;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LinguaLeo.Scripts.Managers
{
    public class WorkoutManager : MonoBehaviour, IObserver
    {
        #region Static Fields and Constants

        private static WordLeo currentWord = null;

        #endregion

        #region SerializeFields

        /// <summary>
        /// Количество слов которое можно изучать в мозговом штурме.
        /// </summary>
        [SerializeField]
        [Header("Количество слов для изучения")]
        private int brainstormQuestCount = 5;

        /// <summary>
        /// Количество слов которое можно изучать в простых тренировках.
        /// </summary>
        [SerializeField]
        private int simpleQuestCount = 10;

        /// <summary>
        /// Множитель для перевода в количество изученых слов в мозговом штурме
        /// </summary>
        [SerializeField]
        [Header("Множитель для перевода в количество изученых слов.")]
        private float factorScoreBrainStorm = 0.25f;

        /// <summary>
        /// Множитель для перевода в количество изученых слов в одиночных тренировках
        /// </summary>
        [SerializeField]
        private float factorScoreSimpleWorkOut = 1f;

        #endregion

        #region Public variables

        public BrainStorm BrainStorm
        {
            get { throw new System.NotImplementedException(); }

            set { }
        }

        #endregion

        #region Private variables

        private int questMaxCount = 0;

        private BrainStorm brainStorm = null;

        private SceneLoader sceneLoader;
        private WorkoutNames currentWorkout;
        private WorkoutNames subWorkout;
        private Workout.Workout core;

        #endregion

        #region Events

        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {
                case GAME_EVENTS.ButtonHandlerLoaded:
                    StartBehaviour();
                    break;
                case GAME_EVENTS.CorrectAnswer:
                    AddWorkoutProgress(currentWord, subWorkout);
                    if (currentWord.AllWorkoutDone())
                        currentWord.AddLicenseLevel();
                    break;
                case GAME_EVENTS.WordsEnded:
                    WordsEndedBehaviour();
                    break;
                case GAME_EVENTS.BuildTask:
                    IWorkout workout = parametr as IWorkout;
                    subWorkout = workout.WorkoutName;
                    currentWord = workout.GetCurrentWord();
                    break;
                case GAME_EVENTS.ContinueWorkout:
                    RestartWorkOut();
                    break;
            }
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            sceneLoader = FindObjectOfType<SceneLoader>();
            SubscribeToEvents();
        }

        #endregion

        #region Public Methods

        public int GetBrainTasks()
        {
            int workoutCount = 4;
            return questMaxCount * workoutCount;
        }

        public float GetCorrectAnswers()
        {
            if (currentWorkout == WorkoutNames.brainStorm)
            {
                return GameManager.ScoreKeeper.ScoreValue * factorScoreBrainStorm;
            }

            return GameManager.ScoreKeeper.ScoreValue;
        }

        public Workout.Workout GetWorkout()
        {
            if (brainStorm == null)
                return core;
            return brainStorm.GetsSubCore();
        }

        /// <summary>
        /// Перезапуск последней тренировки.
        /// </summary>
        public void RestartWorkOut()
        {
            RunWorkOut(currentWorkout);
        }

        /// <summary>
        /// Запуск новой тренировки.
        /// </summary>
        /// <param name="name">Название тренировки</param>
        public void RunWorkOut(WorkoutNames name)
        {
            currentWorkout = name;
            questMaxCount = simpleQuestCount;
            GameManager.ScoreKeeper.SetScoreFactor(factorScoreSimpleWorkOut);

            if (name == WorkoutNames.brainStorm)
            {
                GameManager.ScoreKeeper.SetScoreFactor(factorScoreBrainStorm);
                questMaxCount = brainstormQuestCount;
                core = PrepareWorkout(WorkoutNames.brainStorm);
                brainStorm = new BrainStorm(core, sceneLoader);
                return;
            }

            string sceneName = GetSceneName(name);

            if (sceneName != string.Empty) { sceneLoader.LoadLevel(sceneName); }
        }

        #endregion

        #region Private Methods

        private void AddWorkoutProgress(WordLeo word, WorkoutNames workout)
        {
            switch (workout)
            {
                case WorkoutNames.WordTranslate:
                case WorkoutNames.reiteration:
                    word.LearnWordTranslate();
                    break;
                case WorkoutNames.TranslateWord:
                    word.LearnTranslateWord();
                    break;
                case WorkoutNames.Audio:
                    word.LearnAudio();
                    break;
                case WorkoutNames.Puzzle:
                    word.LearnPuzzle();
                    break;
            }
        }

        private void CoreInitialization()
        {
            if (core != null)
                GameManager.Notifications.PostNotification(core, GAME_EVENTS.CoreBuild);
            else
            {
                Debug.LogError("core == null");
                GameManager.Notifications.PostNotification(core, GAME_EVENTS.NotUntrainedWords);
            }
        }

        private string GetSceneName(WorkoutNames name)
        {
            string sceneName = string.Empty;
            switch (name)
            {
                case WorkoutNames.WordTranslate:
                    sceneName = "worldTranslate";
                    break;
                case WorkoutNames.TranslateWord:
                    sceneName = "translateWorld";
                    break;
                case WorkoutNames.Audio:
                    sceneName = "audioTest";
                    break;
                case WorkoutNames.Puzzle:
                    sceneName = "wordPuzzle";
                    break;
                case WorkoutNames.reiteration:
                    sceneName = string.Empty;
                    break;
                case WorkoutNames.brainStorm:
                    sceneName = string.Empty;
                    break;
                case WorkoutNames.Savanna:
                    sceneName = "savanna";
                    break;
            }

            return sceneName;
        }

        /// <summary>
        /// Подготавливает ядро для тренировки
        /// </summary>
        /// <param name="currentWorkout"></param>
        /// <returns></returns>
        private Workout.Workout PrepareWorkout(WorkoutNames currentWorkout)
        {
            Workout.Workout core = new Workout.Workout(currentWorkout, questMaxCount);
            core.LoadQuestions();
            if (!core.TaskExists())
            {
                Debug.LogError("Нет доступных слов для тренировки" + currentWorkout);
                return null;
            }

            return core;
        }

        private void StartBehaviour()
        {
            SceneManagerAdapt.LoadSceneAsync("wordinfo", LoadSceneMode.Additive);

            switch (currentWorkout)
            {
                case WorkoutNames.WordTranslate:
                case WorkoutNames.TranslateWord:
                    core = PrepareWorkout(currentWorkout);
                    CoreInitialization();
                    break;
                case WorkoutNames.Audio:
                    core = PrepareWorkout(currentWorkout);
                    CoreInitialization();
                    break;
                case WorkoutNames.Savanna:
                    core = PrepareWorkout(currentWorkout);
                    CoreInitialization();
                    break;
                case WorkoutNames.brainStorm:
                    SceneManager.LoadSceneAsync("brainInfo", LoadSceneMode.Additive);
                    brainStorm.CoreInitialization();
                    break;
                case WorkoutNames.Puzzle:
                    core = PrepareWorkout(currentWorkout);
                    CoreInitialization();
                    break;
                case WorkoutNames.reiteration:
                    break;
            }
        }

        private void SubscribeToEvents()
        {
            var notification = GameManager.Notifications;

            notification.AddListener(this, GAME_EVENTS.ButtonHandlerLoaded);
            notification.AddListener(this, GAME_EVENTS.CorrectAnswer);
            notification.AddListener(this, GAME_EVENTS.WordsEnded);
            notification.AddListener(this, GAME_EVENTS.BuildTask);
            notification.AddListener(this, GAME_EVENTS.ContinueWorkout);
        }

        private void WordsEndedBehaviour()
        {
            switch (currentWorkout)
            {
                case WorkoutNames.WordTranslate:
                case WorkoutNames.TranslateWord:
                case WorkoutNames.Savanna:
                case WorkoutNames.Audio:
                case WorkoutNames.Puzzle:
                case WorkoutNames.reiteration:
                    GameManager.SceneLoader.LoadResultWorkOut();
                    break;
                case WorkoutNames.brainStorm:

                    brainStorm.Run();
                    break;
            }
        }

        #endregion
    }
}
