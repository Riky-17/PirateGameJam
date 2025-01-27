using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LandingCrash : BossAttack
{
    public LandingCrash(Boss boss, PlayerMovement player, BulletSO bullet, Enemy enemy, PickUpItemSO[] Items) : base(boss, player, bullet) 
    { 
        enemyToSpawn = enemy;
        items = Items;
    }

   
    const float CAMERA_MAX_HEIGHT = 17 / 2f;
    const float CAMERA_MAX_WIDTH = 30 / 2f;

    PickUpItemSO[] items;
    int itemsAmount = 5;

    Enemy enemyToSpawn;
    int enemiesAmount = 2;

    float landingImpactDamage = 30f; //to be used 
    float landingSpeed = 15f;   

    bool isHigh = false;
    bool isLeft = false;
    bool haslanded = false;

    float enemySpawnTime = 1f;
    float enemySpawnTimer;

    float itemsSpawnTime = 1f;
    float itemsSpawnTimer;
    public override void InitAttack()
    {
        base.InitAttack();
        boss.Stop(); //making the heli dont move
        isHigh = false;
        isLeft = false;
        haslanded = false;
        enemySpawnTimer = 0f;
        itemsSpawnTime = 0f;
    }

    public override void Attack()
    {
        boss.TakeAim();
        if (!isHigh)
        {
            //bring him up
            if (boss.transform.position.y >= CAMERA_MAX_HEIGHT - 3f )
            {
                isHigh = true;
                return;
            }

            boss.AddForceBoss(Vector2.up, boss.Speed, 2);       
         
            return;

        }
        if(!isLeft)
        {
            //bring him left
            if (boss.transform.position.x <= -CAMERA_MAX_WIDTH + 5f)
            {
                isLeft = true;
                return;
            }
            boss.AddForceBoss(Vector2.left, boss.Speed, 2);
            return;
        }
        if(!haslanded && isHigh && isLeft)
        {

            //make him crash ha ha ha ha
            if (boss.transform.position.y <= -CAMERA_MAX_HEIGHT + 5)
            {
                haslanded = true;
                boss.Health -= 30;
                return;
            }
            boss.AddForceBoss(Vector2.down, landingSpeed, 3);
            boss.AddForceBoss(Vector2.right, landingSpeed, 3);
            return;
        }
        if (haslanded)
        {
            enemySpawnTimer += Time.deltaTime;
            if(enemySpawnTimer >= enemySpawnTime)
            {
                //call reinforcement
                for (int i = 0; i < enemiesAmount; i++)
                {
                    float x = Random.Range(-CAMERA_MAX_WIDTH + 1, CAMERA_MAX_WIDTH - 1);
                    Vector2 pos = new(x + boss.CenterPoint.x, CAMERA_MAX_HEIGHT);
                    Enemy enemy = boss.InstantiateEnemy(enemyToSpawn, pos, Quaternion.identity);
                    if (boss is HellicopBoss hellicop)
                        hellicop.SpawnedEnemies.Add(enemy);
                   
                }
                
            }
            //calling items
            itemsSpawnTimer += Time.deltaTime;
            if (itemsSpawnTimer >= itemsSpawnTime)
            {
                for (int i = 0; i < itemsAmount; i++)
                {
                    float x = Random.Range(-CAMERA_MAX_WIDTH + 2, CAMERA_MAX_WIDTH - 2);
                    Vector2 pos = new(x, CAMERA_MAX_HEIGHT);
                    int randomItem = Random.Range(0, items.Length);
                    PickableItem item = boss.InstantiateItem(items[randomItem].item, pos, Quaternion.identity);

                    if (boss is HellicopBoss hellicop)
                       hellicop.itemsOnGame.Add(item);
                }
            }
            isAttackDone = true;

        }
        //if(boss.transform.position.y <= CAMERA_MAX_HEIGHT/2-1)
        //    boss.AddForceBoss(Vector2.up, boss.Speed, 2);
    }
}
