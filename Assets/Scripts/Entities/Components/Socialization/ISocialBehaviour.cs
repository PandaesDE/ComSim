/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule N�rnberg
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISocialBehaviour
{
    public void OnAttacked(Creature attacker);
    public void OnAttacking(Creature victim);
    public void OnFleeing(Creature attacker);
    public Vector2 GetHomeArea();
}
