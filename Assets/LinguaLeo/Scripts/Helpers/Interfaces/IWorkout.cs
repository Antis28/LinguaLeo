namespace LinguaLeo.Scripts.Helpers.Interfaces
{
    internal interface IWorkout
    {
        WordLeo GetCurrentWord();
        WorkoutNames WorkoutName { get; }
        Workout.Workout GetCore();
    }
}