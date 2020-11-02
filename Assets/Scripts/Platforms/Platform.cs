using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : AbstractObstacle
{
     protected override void OnCollisionEnter(Collision collision)
     {
          print("Platform collision");
     }
}
