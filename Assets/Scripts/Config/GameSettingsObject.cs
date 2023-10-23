

[System.Serializable]
public class GameSettingsObject
{
    public string Seed = "";
    public PerlinSettingsObject Pso_Ground;
    public PerlinSettingsObject Pso_Bush;
    public int Human_Amount_Start = 0;
    public int Lion_Amount_Start = 0;
    public int Boar_Amount_Start = 0;
    public int Rabbit_Amount_Start = 0;

    public bool Equals(GameSettingsObject gso)
    {
        return this.Seed == gso.Seed &&
                this.Pso_Ground.Equals(gso.Pso_Ground) &&
                this.Pso_Bush.Equals(gso.Pso_Bush) &&
                this.Human_Amount_Start == gso.Human_Amount_Start &&
                this.Lion_Amount_Start == gso.Lion_Amount_Start &&
                this.Boar_Amount_Start == gso.Boar_Amount_Start &&
                this.Rabbit_Amount_Start == gso.Rabbit_Amount_Start;
    }
}
