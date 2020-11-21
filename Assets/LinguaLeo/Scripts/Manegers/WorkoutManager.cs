using LinguaLeo.Adapters;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers.Parts;
using LinguaLeo.Scripts.Workout;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LinguaLeo.Scripts.Manegers
{
    public class WorkoutManager : MonoBehaviour, IObserver
    {
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

        private int questMaxCount = 0;

        BrainStorm brainStorm = null;

        private static WordLeo currentWord = null;

        private LevelManeger levelManeger;
        private WorkoutNames currentWorkout;
        private WorkoutNames subWorkout;
        private Workout.Workout core;

        public BrainStorm BrainStorm
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        #region Методы
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
                brainStorm = new BrainStorm(core, levelManeger);
                return;
            }

            string sceneName = GetSceneName(name);

            if (sceneName != string.Empty)
            {
                levelManeger.LoadLevel(sceneName);
            }
        }

        /// <summary>
        /// Перезапуск последней тренировки.
        /// </summary>
        public void RestartWorkOut()
        {
            RunWorkOut(currentWorkout);
        }

        public Workout.Workout GetWorkout()
        {
            if (brainStorm == null)
                return core;
            else
                return brainStorm.GetsSubCore();
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
                    GameManager.LevelManeger.LoadResultWorkOut();
                    break;
                case WorkoutNames.brainStorm:

                    brainStorm.Run();
                    break;
                default:
                    break;
            }
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
                default:
                    break;
            }
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

        private void SubscribeToEvents()
        {
            var notification = GameManager.Notifications;

            notification.AddListener(this, GAME_EVENTS.ButtonHandlerLoaded);
            notification.AddListener(this, GAME_EVENTS.CorrectAnswer);
            notification.AddListener(this, GAME_EVENTS.WordsEnded);
            notification.AddListener(this, GAME_EVENTS.BuildTask);
            notification.AddListener(this, GAME_EVENTS.ContinueWorkout);
        }
    
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

        #region Жизненый цикл
        // Use this for initialization
        private void Start()
        {
            levelManeger = FindObjectOfType<LevelManeger>();
            SubscribeToEvents();
        }
        #endregion
    }
}
