using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddedFeaturesController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public ConnectionsHandler connectionsHandler;
    public float projectileSpeed = 1000f; // Speed at which the projectile will move

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (connectionsHandler.getInputTrigger(0))
        {
            ShootProjectile();
        }
    }
    void ShootProjectile()
    {
        // Instantiate the projectile at the position and rotation of this GameObject
        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward*2 +transform.up +transform.right, transform.rotation);

        // Get the Rigidbody component and add force to shoot the projectile forward
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileSpeed);
    }
}
