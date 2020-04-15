//Singleton Design Pattern Example
//Author: Owen Schaffer
//This Singleton does NOT extend MonoBehavior.
//This example is closest to the classic 
//Singleton Pattern shown in 
//Head First Design Patterns, chapter 5.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SingletonPatternConsoleOutput
{
    public class SingletonClass
    {

        // A 'static' variable is shared between all instances
        // of the class
        public static SingletonClass instance;

        // An example instance variable that is accessible from anywhere in your code
        // with SimpleSingletonClass.instance.instanceVariable
        public string textToOutput;
        public int number;

        //This private constructor prevents this object being instantiated outside of this class
        private SingletonClass()
        {}

        //Instead of a constructor, this static method creates a single instance
        //or, if one instance already exists, it will return that one instance.
        public static SingletonClass GetInstance()
        {
            if (instance == null)
            {
                instance = new SingletonClass();
            }
            return instance;
        }

        // An example instance method that any other part of the code can call, as long
        // as this script is somewhere in your Assets folder or its subfolders
        // and the GetInstance() method above is called at least once.
        public void OutputTextToConsole()
        {
            textToOutput = "This is a singleton using a normal class.  \n" +
                                "This class does NOT extend MonoBehavior.  " +
                                "Yay!  " + number;
            Debug.Log(textToOutput);
        }

    }
}