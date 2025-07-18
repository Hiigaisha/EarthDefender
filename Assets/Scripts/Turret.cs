using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float horizontalSpeed;
    public GameObject missilePrefab;
    public Transform spawnPoint;

    public Transform canon;

    public float maxAngle;
    public float munitionSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Mouvement();
        FireMissile();

    }

    public void Mouvement()
    {

        float input = Input.GetAxis("Horizontal");
        Vector3 vitesse = new Vector3(input, 0, 0) * horizontalSpeed;

        Vector3 newPostion = transform.position + vitesse * Time.deltaTime;

        newPostion.x = Mathf.Clamp(newPostion.x, -12.0f, 12.0f); // Clamp the x position between -8 and 8

        transform.position = newPostion; // Update the turret's position

    }
    public void FireMissile()
    {
        //Permet d'obtenir la position de la souris
        Vector3 mousePosition = Input.mousePosition; //Permet d'obtenir la position en pixels de la souris sûr l'écran
        Vector3 mouseToWorld = Camera.main.ScreenToWorldPoint(mousePosition); //On transforme la position en pixels en coordonnées dans le monde /!\ Pour que "Camera.main" fonctionne la caméra doit avoir le tag "MainCamera" 
        mouseToWorld.z = 0;

        //Calcule l'angle entre le canon et la souris
        Vector3 canonMouse = mouseToWorld - canon.position;
        Vector3 canonHaut = Vector3.up; 

        float angle = Vector3.Angle(canonMouse, canonHaut);
        if (angle > maxAngle)
            angle = maxAngle;

        Vector3 cross = Vector3.Cross(canonMouse, canonHaut);

        //On corrige le but qui empêche le tracking de la souris sûr le côté droit de l'écran
        if (cross.z >= 0) 
            canon.eulerAngles = new Vector3(0, 0, -angle); 
        else
            canon.eulerAngles = new Vector3(0, 0, angle);

        //Permet de spawn le missile lorsqu'on appuie sûr le clic gauche de la souris
        if (Input.GetMouseButtonDown(0))
        {
            GameObject spawnedMissile = Instantiate(missilePrefab, spawnPoint.position, Quaternion.identity);

            //Permet de faire tirer le missile selon la position du canon
            Missile munition = spawnedMissile.GetComponent<Missile>(); //On obtient le script Missile
            munition.vitesse = spawnPoint.up * munitionSpeed; //On applique l'angle du point spawnPoint
        }
    }
} 
