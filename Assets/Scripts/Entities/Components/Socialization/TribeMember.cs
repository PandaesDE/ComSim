/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - 
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

using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TribeMember : ISocialBehaviour
{
    //Popularty System
    /*Idea:
            - int Groupbias: [0,100]
                - chance to stay close to other people(gets inhereted to children)
                - expectation: people in groups are more likely to survive a predator*/
    private float _alertDistance = 10f;
    private Creature _creature;

    public TribeMember(Creature creature)
    {
        _creature = creature;
    }

    #region Alert System
    public void OnAttacked(Creature attacker)
    {
        List<Human> humans = GetAlertList(2f);
        AlertList(humans, attacker);

    }

    public void OnAttacking(Creature victim)
    {
        List<Human> humans = GetAlertList();
        AlertList(humans, victim);
    }

    public void OnFleeing(Creature attacker)
    {
        List<Human> humans = GetAlertList();
        AlertList(humans, attacker);
    }

    private void AlertList(List<Human> humans, Creature attacker)
    {
        foreach (Human human in humans)
        {
            if (human == null) continue;

            human.Alert(attacker);
        }
    }

    private List<Human> GetAlertList(float distanceFactor = 1f)
    {
        //this shortcut can only be made when Human is the only social creature around!
        return GameObject.FindObjectsOfType<Human>().ToList()
            .Where(human => Vector3.Distance(_creature.transform.position, human.transform.position) <= _alertDistance * distanceFactor).ToList();
    }
    #endregion
}
