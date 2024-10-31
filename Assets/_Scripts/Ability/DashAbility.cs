using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public float dashDistance;
    public GameObject damageAreaPrefab;
    public float damageAmount = 10f;

    public override void Activate(GameObject parent)
    {
        Playermove playermove = parent.GetComponent<Playermove>();
        if (playermove != null)
        {
            Transform hitpoint = playermove.currentTransform;
            Vector3 dashDirection = hitpoint.forward;
            Vector3 newPosition = hitpoint.position + dashDirection * dashDistance;
            hitpoint.position = newPosition;

            DashDamageArea damageArea = CreateDamageArea(newPosition);
            damageArea.ApplyDamage(damageAmount);
        }
    }

    private DashDamageArea CreateDamageArea(Vector3 position)
    {
        GameObject area = Instantiate(damageAreaPrefab, position, Quaternion.identity);
        return area.GetComponent<DashDamageArea>();
    }
}
