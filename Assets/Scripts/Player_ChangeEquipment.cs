using UnityEngine;

public class Player_ChangeEquipment : MonoBehaviour
{
    public Player_Combat combat;
    public Player_Bow bow;
    void Update()
    {
        if (Input.GetButtonDown("ChangeEquipment"))
        {
            combat.enabled = !combat.enabled;   
            bow.enabled = !bow.enabled;

            // ensure shooting state is cleared when switching equipment so player isn't locked
            if (bow != null && bow.playerMovement != null)
            {
                bow.playerMovement.isShooting = false;
            }
        }
    }
}
