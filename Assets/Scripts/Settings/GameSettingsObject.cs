/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Object Class to Store GameSettings
 *  
 *  References:
 *      Scene:
 *          - 
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

[System.Serializable]
public class GameSettingsObject
{
    public string seed = "";
    public PerlinSettingsObject PSO_Ground;
    public PerlinSettingsObject PSO_Bush;
    public int startAmountHuman = 0;
    public int startAmountLion = 0;
    public int startAmountBoar = 0;
    public int startAmountRabbit = 0;

    public bool Equals(GameSettingsObject gso)
    {
        return this.seed == gso.seed &&
                this.PSO_Ground.Equals(gso.PSO_Ground) &&
                this.PSO_Bush.Equals(gso.PSO_Bush) &&
                this.startAmountHuman == gso.startAmountHuman &&
                this.startAmountLion == gso.startAmountLion &&
                this.startAmountBoar == gso.startAmountBoar &&
                this.startAmountRabbit == gso.startAmountRabbit;
    }
}
