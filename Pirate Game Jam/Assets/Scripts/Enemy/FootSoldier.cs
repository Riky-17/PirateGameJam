public class FootSoldier : Enemy
{
    void FixedUpdate()
    {
        if(player == null)
        {
            LookForPlayer();
            Patrol();
        }
        else
            ChasePlayer();
    }
}
