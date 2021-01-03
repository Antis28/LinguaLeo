namespace LinguaLeo.Scripts.Helpers.Interfaces
{
    internal interface IWorkout
    {
        #region Public variables

        WorkoutNames WorkoutName { get; }

        #endregion

        #region Public Methods

        Workout.Workout GetCore();
        WordLeo GetCurrentWord();

        #endregion
    }
}
