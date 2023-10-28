public class Male : IGender
{
    public bool isReadyForMating
    {
        get
        {
            return false;
        }
    }
    
    public bool isMale
    {
        get
        {
            return true;
        }
    }
}
