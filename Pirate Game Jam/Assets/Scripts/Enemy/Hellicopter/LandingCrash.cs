using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class LandingCrash : BossAttack
{
    public LandingCrash(Boss boss, PlayerMovement player, BulletSO bullet, Enemy enemy, PickUpItemSO[] Items, GameObject explosion) : base(boss, player, bullet)
    {
        enemyToSpawn = enemy;
        items = Items;
        grenadeExplosion = explosion;
    }

    GameObject grenadeExplosion;

    const float CAMERA_MAX_HEIGHT = 17 / 2f;
    const float CAMERA_MAX_WIDTH = 30 / 2f;

    PickUpItemSO[] items;
    int itemsAmount = 5;

    Enemy enemyToSpawn;
    int enemiesAmount = 2;

    float landingSpeed = 35f;

    bool isHigh = false;
    bool isLeft = false;
    bool haslanded = false;

    float enemySpawnTime = 1f;
    float enemySpawnTimer;

    float itemsSpawnTime = 1f;
    float itemsSpawnTimer;

    float attackTimer;
    float attackTime = 5f;

    bool hasExploded = false;
    float landingExplosion = 15f;
    public override void InitAttack()
    {
        base.InitAttack();
        boss.Stop(); //making the heli dont move
        isHigh = false;
        isLeft = false;
        haslanded = false;
        hasExploded = false;
        enemySpawnTime = 0f;
        itemsSpawnTime = 0f;
        attackTimer = attackTime;
        grenadeExplosion.transform.localScale = new Vector3(landingExplosion, landingExplosion, 1);
    }

    public override void Attack()
    {
        boss.TakeAim();

        if (!isHigh)
        {
            //bring him up
            if (boss.transform.position.y >= CAMERA_MAX_HEIGHT - 3f)
            {
                isHigh = true;
                return;
            }

            boss.AddForceBoss(Vector2.up, boss.Speed, 2);

            return;

        }

        if (!isLeft && isHigh)
        {
            //bring him left
            if (boss.transform.position.x <= boss.CenterPoint.x - CAMERA_MAX_WIDTH + 2)
            {
                isLeft = true;
                return;
            }
            boss.AddForceBoss(Vector2.left, boss.Speed, 2);
            return;
        }

        if (!haslanded && isHigh && isLeft)
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
            //attackTimer += Time.deltaTime;
            enemySpawnTimer += Time.deltaTime;
            if (!hasExploded)
            {
                DeathExplosion();
                hasExploded = true;
            }

            //calling items
            itemsSpawnTimer += Time.deltaTime;
            if (itemsSpawnTimer >= itemsSpawnTime)
            {
                for (int i = 0; i < itemsAmount; i++)
                {
                    float x = Random.Range(boss.CenterPoint.x - CAMERA_MAX_WIDTH + 2, boss.CenterPoint.x + CAMERA_MAX_WIDTH - 2);
                    Vector2 pos = new(x, CAMERA_MAX_HEIGHT + 3);
                    int randomItem = Random.Range(0, items.Length);
                    PickableItem item = boss.InstantiateItem(items[randomItem].item, pos, Quaternion.identity);

                    if (boss is HellicopBoss hellicop)
                        hellicop.itemsOnGame.Add(item);
                }
            }
            if (enemySpawnTimer >= enemySpawnTime)
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
            if (boss.transform.position.y < boss.CenterPoint.y - CAMERA_MAX_HEIGHT - 3)
                boss.AddForceBoss(Vector2.up, boss.Speed, 2);


            
            //add explotion
            isAttackDone = true;


        }

        void DeathExplosion()
        {
            Vector3 exploPo = new Vector3(boss.transform.position.x + 3f,boss.transform.position.y -2, boss.transform.position.z);
            GameObject Explosion = boss.InstantiateObject(grenadeExplosion, exploPo, Quaternion.identity);                        
            boss.DestroyObject(Explosion, .5f);
        }
        
           
        
    }
}
