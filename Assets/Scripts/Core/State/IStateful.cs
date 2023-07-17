namespace ExtremeSnake.Core
{
    //interface for state machines
    public interface IStateful<T> where T: IMonobehaviourState
    {
        public void ChangeState(T state);
    }
}
