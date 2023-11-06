/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

public class Boar : Creature
{
    protected override void Awake()
    {
        base.Awake();

        Omnivore dietary = new(this);
        int health = 150;
        int weight = 130;
        float speed = .2f;
        initAttributes(dietary, health, weight, speed);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (statusManager.status != StatusManager.Status.SLEEPING)
        {
            movement.MoveToTarget();
            evaluateVision();
            makeStatusBasedMove();
        }
    }

    /*Gets called by Parent*/
    protected override bool isSameSpecies(Creature c)
    {
        Boar partner = c.gameObject.GetComponent<Boar>();
        if (partner == null) return false;
        return true;
    }

    protected override void giveBirth()
    {
        Spawner.spawnBoars(1);
    }
}
