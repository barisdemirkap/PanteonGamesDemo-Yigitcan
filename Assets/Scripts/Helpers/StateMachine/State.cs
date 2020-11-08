/// <summary>
/// Finite state machine states base class
/// </summary>
/// <typeparam name="T">Context object of state machine</typeparam>
public abstract class State<T>
{
     protected T context;

     protected State(T context)
     {
          this.context = context;
     }
     public abstract void Update();
     public virtual void OnStateEnter() { }
     public virtual void OnStateExit() { }
}
