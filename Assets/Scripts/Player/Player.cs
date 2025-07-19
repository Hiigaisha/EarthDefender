using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float horizontalSpeed;
    private Vector3 currentPlayerSpeed; // Current speed of the player
    public GameObject missilePrefab; // Prefab for the missile
    public Transform spawnPoint; // Point where the missile will be spawned

    public Transform weapon; // Transform of the cannon for aiming

    public float maxAngle; // Maximum angle for the cannon
    public float munitionSpeed; // Speed of the missile

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move(); // Call the Move method to handle player movement
        FireMissile(); // Call the FireMissile method to handle missile firing
    }

    public void Move()
    {
        float input = Input.GetAxis("Horizontal");
        currentPlayerSpeed = new Vector3(input, 0, 0) * horizontalSpeed; // Calculate the current speed based on input and horizontal speed

        Vector3 newPosition = transform.position + currentPlayerSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -12.0f, 12.0f); // Clamp the x position between -12 and 12
        transform.position = newPosition; // Update the player's position
    }

    public void FireMissile()
    {
        // Permet d'obtenir la position de la souris
        Vector3 mousePosition = Input.mousePosition; // Permet d'obtenir la position en pixels de la souris sûr l'écran
        Vector3 mouseToWorld = Camera.main.ScreenToWorldPoint(mousePosition); // On transforme la position en pixels en coordonnées dans le monde /!\ Pour que "Camera.main" fonctionne la caméra doit avoir le tag "MainCamera" 
        mouseToWorld.z = 0;


        // Calcule l'angle entre le canon et la souris
        Vector3 weaponMouse = mouseToWorld - weapon.position;
        Vector3 weaponRight = Vector3.right;

        // Permet de faire suivre le canon à la souris
        float angle = Vector3.Angle(weaponMouse, weaponRight);
        if (angle > maxAngle)
            angle = maxAngle;
        Vector3 cross = Vector3.Cross(weaponRight, weaponMouse);
        // On corrige ce qui empêche le tracking correct de la souris
        if (cross.z >= 0)
            weapon.eulerAngles = new Vector3(0, 0, angle);
        else
            weapon.eulerAngles = new Vector3(0, 0, -angle);

        // Permet de spawn le missile lorsqu'on appuie sûr le clic gauche de la souris
        if (Input.GetMouseButtonDown(0))
        {
            GameObject spawnedMissile = Instantiate(missilePrefab, spawnPoint.position, Quaternion.Euler(0, 0, 90));

            // Permet de faire tirer le missile selon la position du canon
            Missile munition = spawnedMissile.GetComponent<Missile>(); // On obtient le script Missile

            Vector3 missileDirection = spawnPoint.right; //On calcule la direction de la munition

            Vector3 weaponMouseDirection = mouseToWorld - weapon.position; // Direction du canon vers la souris
            float angleWeapon = Mathf.Atan2(weaponMouseDirection.y, weaponMouseDirection.x) * Mathf.Rad2Deg; // On convertit l'angle en degrés

            spawnedMissile.transform.rotation = Quaternion.Euler(0, 0, angleWeapon + 90); // On applique la rotation du canon au missile

            munition.vitesse = missileDirection * munitionSpeed; // On applique la vitesse de la munition


        }


    }
}
