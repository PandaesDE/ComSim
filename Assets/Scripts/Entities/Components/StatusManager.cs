/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Status manager component of a creature
 *  
 *  References:
 *      Scene:
 *          - Indirectly (Component of Creature.cs) for simulation scene(s)
 *      Script:
 *          - One instance per creature
 *          
 *  Notes:
 *      - TODO: Refactor to a better State Machine
 *          - https://www.youtube.com/watch?v=Vt8aZDPzRjI
 *  
 *  Sources:
 *      - 
 */

public class StatusManager
{
    public State Status { get; private set; } = State.wandering;

    private Brain _brain = null;

    public StatusManager(Brain brain)
    {
        this._brain = brain;
    }
    public enum State  
    {
        wandering,
        sleeping,
        hungry,
        thirsty,
        looking_for_partner,
        //collecting? TODO,
        hunting,
        starving,
        dehydrated,
        fleeing,
        //protecting? TODO,
        giving_birth
    }

    public void SetState(State status)
    {
        _brain.OnStateChange();
        this.Status = status;
    }

}
