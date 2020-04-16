//Singleton Design Pattern Example
//Author: Owen Schaffer
//This Singleton extends MonoBehavior.
//This code example is based on: https://raw.githubusercontent.com/thesecretlab/unity-game-development-cookbook-1e/master/Scripting/Assets/SimpleSingleton.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SingletonPatternConsoleOutput
{
    public class SingletonMonoBehaviour : MonoBehaviour
    {

        // A 'static' variable is shared between all instances
        // of the class
        public static SingletonMonoBehaviour instance;

        // Example instance variables that are accessible from anywhere in your code
        // with SimpleSingletonClass.instance.instanceVariable
        public string textToOutput;
        public int number;

        private void Awake()
        {
            // When this object wakes up, it sets the instance variable to
            // an instance of this script.
            // Because the instance variable is public and static, any
            // class from any location can access it and call its methods.
            
            //instance = this;
            
            // Awake is only called once, but just to be sure we only have
            // one instance of the singleton, we can do this:
            if (instance == null)
            {
                instance = this;
            }

        }

        // An example instance method, that any other part of the code can call, as long
        // as there's a game object in the scene that has this SimpleSingletonMonoBehavior
        // script attached
        public void OutputTextToConsole()
        {
            textToOutput = "This is a singleton using a class that extends MonoBehavior.  \n" +
                                "This class DOES extend MonoBehavior, so attach it to a GameObject.  " +
                                "Yay!  " + number;
            Debug.Log(textToOutput);
        }

    }
}