public class StatusManager
{
    public Status status { get; private set; } = Status.WANDERING;

    private Brain _brain = null;

    public StatusManager(Brain brain)
    {
        this._brain = brain;
    }
    public enum Status
    {
        WANDERING,
        SLEEPING,
        HUNTING,
        FLEEING,
        THIRSTY,
        DEHYDRATED,
        HUNGRY,
        STARVING,
        LOOKING_FOR_PARTNER,
        GIVING_BIRTH
    }

    public void setState(Status status)
    {
        _brain.onStateChange();
        this.status = status;
    }

}
