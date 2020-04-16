//Singleton Design Pattern Example
//Author: Owen Schaffer
//This code shows how the Singletons can be used.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SingletonPatternConsoleOutput
{
    public class UsingSingletons : MonoBehaviour
    {
        //Notice there is NO reference to either of our Singletons here
        //because Singletons are globally accessible.

        //The SingletonClass that does not extend MonoBehavior needs 
        //its GetInstance() method to be called at least once to set it up.
        private void Awake()
        {
            SingletonClass.GetInstance();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Setting an instance variable on the singleton
                SingletonMonoBehaviour.instance.number = 42;

                // Accessing the singleton and calling a method on it
                SingletonMonoBehaviour.instance.OutputTextToConsole();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // Setting an instance variable on the singleton
                SingletonClass.instance.number++;

                // Accessing the singleton and calling a method on it
                SingletonClass.instance.OutputTextToConsole();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // Setting an instance variable on the static class
                StaticClass.number++;

                // Accessing the singleton and calling a method on it
                StaticClass.OutputTextToConsole();
            }

        }
    }
}