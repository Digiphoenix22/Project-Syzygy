using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

//
//
//
//
//Too lazy to fully comment this right now. I'll get to it
//
//
//
public class playerMove1 : MonoBehaviour
{
   [SerializeField] SplineContainer spline; //Spline used
   [SerializeField] float speed; //Speed of spline traversal
   [SerializeField] GameObject pathSprite; //sprite to use when drawing visible path
   [SerializeField] ParticleSystem shipTrail; //trail particle system
   [SerializeField] string[] Destinations;
   [SerializeField] Text DestinationText;

   
   private float distancePercentage = 0f; //Percentage of the time it takes to complete the spline
   private float splineLength; //Time in seconds it takes to traverse a spline
   private int numSplines;
   
   private bool isMoving = false; //tracks if player should be moving
   private bool justMovedBack = false; //tracks if player just moved backward
   private bool justMovedForward = false; //tracks if player just moved forward

   
   private bool plottingCourse = false; //active when moving to a new planet for the first time
   private float pathTimer = 0f; //timer to place paths while plottingcourse
   private int planetValue = 0; //currentPlanet
   private int highestPlanetReached = 0; //farthest you've gone


   private void Start()
   {
    DestinationText.enabled = false;
    shipTrail.Stop();
    splineLength = spline.CalculateLength();
    numSplines = spline.Splines.Count;
   }

   private void Update()
   {
    if(isMoving)
    {
        DestinationText.enabled = false;
        MoveOnSpline();
    }
    else if(Input.GetKeyDown(KeyCode.S))
    {
        if(planetValue > 0)
        {
            shipTrail.Play();
            planetValue -= 1;
            if(justMovedBack)
            {
                splineBackward();
            }
            else if (justMovedForward)
            {
                reverse();
            }
            distancePercentage = 0f;
            isMoving = true;
            justMovedBack = true;
            justMovedForward = false;  
        }
    }
    else if(Input.GetKeyDown(KeyCode.W))
    {
        if(planetValue < numSplines)
        {
            shipTrail.Play();
            planetValue += 1;
            if(planetValue > highestPlanetReached)
            {
                highestPlanetReached = planetValue;
                plottingCourse = true;
            }
            if(justMovedForward)
            {
                splineForward();
            }
            else if (justMovedBack)
            {
                reverse();
            }
            distancePercentage = 0f;
            isMoving = true;
            justMovedForward = true;
            justMovedBack = false; 
        } 
    }
    else if(Input.GetKeyDown(KeyCode.Space ))
    {
        string message = "Going to ";
        DestinationText.text = (message + Destinations[planetValue]);
        DestinationText.enabled = true;
        StartCoroutine(enterLevel(Destinations[planetValue]));
    }
   }
   
   private void MoveOnSpline()
   {
    distancePercentage += speed * Time.deltaTime / splineLength; //Percentage done

    Vector3 currentPosition = spline.EvaluatePosition(distancePercentage); //Finds the current position on spline based on percentage done
    pathTimer += Time.deltaTime;
    if(plottingCourse)
    {
        if(pathTimer >= 0.1f)
            {
                drawPath();
                pathTimer = 0f;
            }
    }
    transform.position = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - 30); //Sets position

    if((distancePercentage * 1000) % 2 == 0)
    {
        drawPath();
    }
    if (distancePercentage > 1f) //Loops spline
    {
        shipTrail.Stop();
        isMoving = false;
        plottingCourse = false;
    }
    else
    {
        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f); //Calculates position to look at. This is not for movement
        Vector3 direction = nextPosition - currentPosition; //Finds direction to look at
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;               
        Quaternion rotation = Quaternion.Euler(0, 0, angle); //Using Quaternion.Euler because LookAt is fucking goofy or sumn

        transform.rotation = rotation;
    }
    
    
   }

   private void splineForward()
   {
    for(int i = 0; i < numSplines - 1; i++)
    {
        spline.ReorderSpline(i+1, i);
        Debug.Log(i + " moved to " + (i + 1));
    }
    splineLength = spline.CalculateLength();
   }

   private void splineBackward()
   {
    for(int i = numSplines - 1; i > 0; i--)
    {
        spline.ReorderSpline(i - 1, i);
        Debug.Log(i + " moved to " + (i - 1));
    }
        splineLength = spline.CalculateLength();
   }

   private void reverse()
   {
    for(int i = 0; i < numSplines; i++)
    {
        spline.ReverseFlow(i);
        Debug.Log("reversed " + i);
    }
    splineLength = spline.CalculateLength();
   }

   private void drawPath()
   {
    Instantiate(pathSprite, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), transform.rotation);
   }

   IEnumerator enterLevel(string levelName)
   {
    yield return new WaitForSeconds(2);
    SceneManager.LoadScene(levelName);
   }
}
