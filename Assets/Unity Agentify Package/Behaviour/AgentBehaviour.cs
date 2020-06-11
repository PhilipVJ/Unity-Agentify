using UnityEngine;

public abstract class AgentBehaviour
{
    public abstract bool TakeControl();
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void Suppress() { }

}
