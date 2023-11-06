public interface IGender
{
    public bool isReadyForMating { get; }
    public bool isMale { get; }
    public void FixedUpdate();
    public void mating();
}
