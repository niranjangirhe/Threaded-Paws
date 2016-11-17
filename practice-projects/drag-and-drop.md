#Drag and Drop

###Transforms

Position, rotation and scale of an object.

Every object in a scene has a Transform. It's used to store and manipulate the position, rotation and scale of the object. Every Transform can have a parent, which allows you to apply position, rotation and scale hierarchically. This is the hierarchy seen in the Hierarchy pane. They also support enumerators so you can loop through children using:

``` cs
using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
    void Example() {
        foreach (Transform child in transform) {
            child.position += Vector3.up * 10.0F;
        }
    }
}
```