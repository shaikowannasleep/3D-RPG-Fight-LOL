using UnityEngine;


[CreateAssetMenu]
public class FireBallShoot : Ability
{
    private Playermove movement;
    //[SerializeField] private float projectileSpeed = 15f;

    [Header("Bullets Config")]
    [SerializeField] private GameObject fireballFXPrefab;
    public float damageAmount = 10f;

    public override void Activate(GameObject parent)
    {
        //Playermove playermove = parent.GetComponent<Playermove>();
        //if (playermove != null)
        //{
        //    GameObject fireballFX = Instantiate(fireballFXPrefab, playermove.currentTransform.position, Quaternion.identity);
        //   Bullet bullet = fireballFX.GetComponent<Bullet>();
        //    bullet.Initialize(20f, null);
        //    Vector3 direction = playermove.transform.forward;
        //    Rigidbody rb = fireballFX.AddComponent<Rigidbody>();
        //    rb.useGravity = false;
        //    rb.velocity = direction * projectileSpeed;
        //}
        //else
        //{
        //    Debug.LogWarning("Playermove component not found on the parent GameObject.");
        //}

        IndicatorController indicatorController = parent.GetComponent<IndicatorController>();

        if (indicatorController != null)
        {
            Vector3 castLocation;

            if (useIndicator)
            {
                castLocation = indicatorController.GetCurrentCastLocation();
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMask = LayerMask.GetMask("Environment");

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    castLocation = hit.point;
                }
                else
                {
                    castLocation = parent.transform.position; 
                }
            }

            AuraHit damageArea = CreateDamageArea(castLocation);
            damageArea.ApplyDamage(damageAmount);
        }
        else
        {
           // Debug.LogWarning("IndicatorController component not found on the parent GameObject.");
        }

    }


    private AuraHit CreateDamageArea(Vector3 position)
    {
        GameObject area = Instantiate(fireballFXPrefab, position, Quaternion.identity);
        return area.GetComponent<AuraHit>();
    }

}
