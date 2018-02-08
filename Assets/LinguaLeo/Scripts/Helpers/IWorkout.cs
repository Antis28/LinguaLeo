internal interface IWorkout
{
    WordLeo GetCurrentWord();
    WorkoutNames WorkoutName { get; }
}