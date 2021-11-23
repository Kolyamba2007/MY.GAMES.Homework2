using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Game;

public class ZombieTestSuite
{
    string ZombiePrefabPath = "Prefabs/Zombie";
    string PlayerPrefabPath = "Prefabs/Player";

    ZombieComponent zombie;
    PlayerController player;

    [SetUp]
    public void Setup()
    {
        zombie = Behaviour.Instantiate(Resources.Load<ZombieComponent>(ZombiePrefabPath));
        player = Behaviour.Instantiate(Resources.Load<PlayerController>(PlayerPrefabPath));
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(zombie.gameObject);
    }

    [UnityTest]
    public IEnumerator ChangeToAttackStateOnTrigger()
    {
        yield return null;

        Assert.True(zombie.State == ZombieState.Attack);

        Object.Destroy(player.gameObject);
    }

    [UnityTest]
    public IEnumerator ChangeToWanderStateAfterPlayerKill()
    {
        yield return null;

        Object.Destroy(Behaviour.FindObjectOfType<PlayerController>().gameObject);

        yield return new WaitForSeconds(1f);

        Assert.True(zombie.State == ZombieState.Wander);
    }
}
