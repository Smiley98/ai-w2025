using UnityEngine;

public class DecisionTreeTheory : MonoBehaviour
{
    [SerializeField]
    bool isVisible;

    [SerializeField]
    bool isAudible;

    [SerializeField]
    bool isNear;

    [SerializeField]
    bool isFlank;

    void Update()
    {
        if (isVisible)
        {
            if (isNear)
            {
                // Attack if the enemy is visible and near!
                Attack();
            }
            else
            {
                if (isFlank)
                {
                    // Move if the enemy is far away and behind us
                    Move();
                }
                else
                {
                    // Attack if the enemy is far away but we can see him!
                    Attack();
                }
            }
        }
        else
        {
            if (isAudible)
            {
                // Creep if the enemy is invisible but audible
                Creep();
            }
            else
            {
                // Do nothing if the enemy is invisible and inaudible
            }
        }
    }

    void Creep()
    {
        Debug.Log("Creeping...");
    }

    void Attack()
    {
        Debug.Log("Attacking!");

    }

    void Move()
    {
        Debug.Log("Moving.");
    }
}
