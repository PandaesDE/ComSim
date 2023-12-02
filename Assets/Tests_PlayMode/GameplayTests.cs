/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - class for play tests
 *  
 *  References:
 *      Scene:
 *          - scene independent
 *      Script:
 *          - 
 *          
 *  Notes:
 *      - Possible Tests:
 *          - Check if Vision functions properly
 *               - Does food get recognized & stored properly?
 *               - can eat when no food in range?
 *               - Do Mates get recognized & stored properly?
 *               - Can mate if no partner in range?
 *               - Do Enemies get recognized & stored properly?
 *               - Does Water get recognized & stored properly?
 *               - Can drink if no water in range?
 *           - Check if Mating functions properly
 *               - in Range?
 *               - Same Sex?
 *               - Same Species?
 *          - Check if Movement functions properly ?
 *  
 *  Test Workflow:
 *      - Create Scene with predefined aspects (x amount of Creatures doing y)
 *      - See if behavior matches Expected
 *  
 *  Sources:
 *      - 
 */

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GameplayTests
{
    [SetUp]
    public void SetupTests()
    {
        SceneManager.LoadScene("TestScene_Mating");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator Mating_Passed()
    {
        //Test_MapGenerator tmg = GameObject.Find("SystemNode").GetComponent<Test_MapGenerator>();

        //Human h1 = Spawner.SpawnHumans(new SpawnOptions()
        //    .SetPosition(new Vector2(0, 0))
        //    .SetIsMale(true))[0];
        //Human h2 = Spawner.SpawnHumans(new SpawnOptions()
        //    .SetPosition(new Vector2(0, 1))
        //    .SetIsMale(true))[0];
        
        //tmg.RenderNormalTestMap(50, 50);
        yield return new WaitForSeconds(3);
    }

}
