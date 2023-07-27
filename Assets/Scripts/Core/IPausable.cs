namespace ExtremeSnake.Core
{
    public interface IPausable
    {
        bool IsPaused { get; }
        void HandlePause(object sender);
        void Pause();
        void HandleResume(object sender);
        void Resume();
    }
}