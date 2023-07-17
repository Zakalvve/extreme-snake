namespace ExtremeSnake.Core
{
    //interface that concrete states must implement
    public interface IState
    {
        //can be called to initialise a state
        public void TransitionTo();
    }
}
