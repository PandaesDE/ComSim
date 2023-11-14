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
        Test_MapGenerator tmg = GameObject.Find("SystemNode").GetComponent<Test_MapGenerator>();

        Human h1 = Spawner.spawnHumans(new SpawnOptions()
            .set_Position(new Vector2(0, 0))
            .set_IsMale(true))[0];
        Human h2 = Spawner.spawnHumans(new SpawnOptions()
            .set_Position(new Vector2(0, 1))
            .set_IsMale(true))[0];
        
        tmg.RenderNormalTestMap(50, 50);
        yield return new WaitForSeconds(3);
    }

    /*  Possible Tests:
     *  - Check if Vision functions properly
     *      - Does food get recognized & stored properly?
     *      - can eat when no food in range?
     *      - Do Mates get recognized & stored properly?
     *      - Can mate if no partner in range?
     *      - Do Enemies get recognized & stored properly?
     *      - Does Water get recognized & stored properly?
     *      - Can drink if no water in range?
     *  - Check if Mating functions properly
     *      - in Range?
     *      - Same Sex?
     *      - Same Species?
     *  - Check if Movement functions properly ?
     *  
     *  Test Workflow:
     *  - Create Scene with predefined aspects (x amount of Creatures doing y)
     *  - See if behavior matches Expected
     */
}
