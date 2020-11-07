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
