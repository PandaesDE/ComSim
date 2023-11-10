using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

public class MatingTests
{
    [Test]
    public void NoSameSexMating_Passed()
    {
        Human human1 = Spawner.spawnHuman(new UnityEngine.Vector2(0, 0));
        Human human2 = Spawner.spawnHuman(new UnityEngine.Vector2(0, 0));
    }

    [Test]
    public void MatingOnlyInRange_Passed()
    {
        Human human1 = Spawner.spawnHuman(new UnityEngine.Vector2(0,0));
        Human human2 = Spawner.spawnHuman(new UnityEngine.Vector2(0,3));
        
    }

    /*This function is to test and verify the Entity Senses*/
    //public void SensesCheck_Passed()
    //{
    //    int a = 20;
    //    Human hum = spawnHuman(new Vector2(a, 0));
    //    Boar ani = spawnAnimal(animalType.BOAR,new Vector2(-a, 0));
    //    hum.movement.setTarget(new Vector2(-a, 0));
    //    ani.movement.setTarget(new Vector2(a, 0));
    //}


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
