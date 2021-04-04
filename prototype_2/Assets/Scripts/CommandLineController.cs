using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CommandLineController : MonoBehaviour
{
    public static TMP_InputField commandLine;

    public void Start()
    {
        commandLine = GameObject.FindGameObjectWithTag("CommandLine").GetComponent<TMP_InputField>();
    }

    public static void ParseCommand()
    {
        string validatedCommand = CommandLineExecutor.ValidateCommand(commandLine.text);
        if (validatedCommand != null)
        {
            CommandLineExecutor.ExecuteCommand(validatedCommand);
        }
    }

    public class CommandLineActions
    {
        public static void Feed(List<string> args)
        {
            if(args == null)
            {
                // Feed all case
                Debug.Log("Feeding all cubs");
                
            } else
            {
                foreach(string arg in args)
                {
                    Debug.Log($"{arg}");
                }
            }
        }

        public static void Rest(List<string> args)
        {
            throw new NotImplementedException();
        }

        public static void Exercise(List<string> args)
        {
            throw new NotImplementedException();
        }
        // TODO make it a method call via Worker instance.Work() instead to get the worker's name -- just get the first part with substr, index of '.'
        public static void Work(string methodCaller, List<string> args)
        {
            Debug.Log("Working");
            Debug.Log(args);
            print(WorkerManager.activeWorkers);
            print($"{methodCaller}.work('{args[0]}', {args[1]})");
            List<GameObject> activeWorkers = WorkerManager.activeWorkers;
            int len = activeWorkers.Count;
            if(len == 0) return;
            
            if (methodCaller.Length == 0 || args == null || args.Count == 0 || args[0].Equals(string.Empty))
            {
                // Work all workers
                for(int i = 0; i < len; i++)
                {
                    Worker w = activeWorkers[i].GetComponent<Worker>();
                    // Internal
                    w.InternalMoveToDestination("WORKFIELD_1");
                    print(w.ToString());
                    w.CheckLocationAction(); // 10 workbatches is default
                    // Display
                    w.MoveToDestination(w.WORKFIELD_1.position);
                }
            } else
            { // second arg is usually the number of workbatches to complete 
                // first arg would be the target work location
                // find worker matching method caller name
                for(int i = 0; i < len; i++)
                {
                    Worker w = activeWorkers[i].GetComponent<Worker>();
                    string workerName = w.Name;
                    if(Equals(workerName, methodCaller)) {
                        // Internal
                        string locationTarget = args[0];
                        w.InternalMoveToDestination(locationTarget);
                        w.CurrentTaskProgressHoursRequired = Single.Parse(args[1]);
                        print(w.ToString());
                        w.CheckLocationAction();
                        // Display
                        w.MoveToDestination(w.WORKFIELD_1.position);
                    }
                }
            }
        }

        public static void Sell(List<string> args)
        {
            throw new NotImplementedException();
        }

        public static void Examine(List<string> args)
        {
            throw new NotImplementedException();
        }

        public static void Buy(List<string> args)
        {
            if (args == null)
            {
                // Feed all case
                Debug.Log("Buying all that is possible within the boundaries of personal material resources.");
            }
            else
            {
                int qty = default;
                if (int.TryParse(args[0], out qty))
                {
                    if(qty > 0 && args.Count > 1)
                    {
                        // Buying qty of X
                        for(int i = 1; i < args.Count; i++)
                        {
                            if (args[i].Equals("chicken") || args[i].Equals("sheep") ||
                                args[i].Equals("cow") || args[i].Equals("duck") ||
                                args[i].Equals("fox") || args[i].Equals("pig") ||
                                args[i].Equals("wolf"))
                            {
                                Debug.Log("Buying a silly cub!");
                                if(!Main.currentCubRoosterFull)
                                {
                                    Main.LevelController.GenerateNewCub(qty, args[i]);
                                } else
                                {
                                    Debug.Log("Cub rooster is full");
                                }
                            } else
                            {
                                Debug.Log("Type of goods doesn't exist.");
                            }                            
                        }
                    }
                }
            }
        }
    }

    public class CommandLineExecutor
    {
        public CommandLineExecutor() { }

        public static string ValidateCommand(string commandLineText)
        {
            if(commandLineText.Length <= 0 || commandLineText == null)
            {
                return null;
            }
            commandLineText = commandLineText.Trim().Replace(" ", string.Empty);
            return commandLineText;
        }

        public static void ExecuteCommand(string commandLineText)
        {
            List<string> args = new List<string>();
            int argsStartIndex = commandLineText.IndexOf('(');
            int argsEndIndex = commandLineText.IndexOf(')');
            if(argsStartIndex == -1 || argsEndIndex == -1) {
                return;
            }
            // Get the method caller
            int methodCallerIndex = commandLineText.IndexOf('.');
            string methodCaller = "";
            if(methodCallerIndex != -1) {
                methodCaller = commandLineText.Substring(0, methodCallerIndex);   
                print("Player invoked method on object: " + methodCaller);
            }

            int argsRange = argsEndIndex - argsStartIndex - 1;
            try
            {
                if (argsStartIndex != -1)
                {
                    if (commandLineText[argsStartIndex + 1] != ')')
                    {
                        int argCount = commandLineText.IndexOf(',');
                        if(argCount == -1)
                        {
                            // Single arg
                            args.Add(commandLineText.Substring(argsStartIndex + 1, argsRange));
                        } else
                        {
                            string[] _args = commandLineText.Substring(argsStartIndex + 1, argsRange).Replace('"', ' ').Trim().Split(',');
                            args.AddRange(_args);
                        }
                    } else
                    {
                        commandLineText = commandLineText.Replace('(', ' ').Replace(')', ' ');
                    }
                }
            } catch(System.ArgumentOutOfRangeException e)
            {
                Debug.LogError(e.Message);
            } catch(System.Exception e)
            {
                Debug.LogError(e.Message);
            }

            string methodCall = methodCallerIndex != -1? commandLineText.Substring(methodCallerIndex + 1, argsStartIndex - methodCallerIndex - 1) : commandLineText.Substring(0, argsStartIndex);
            switch (methodCall)
            {
                case "feed":
                    Debug.Log("Feeding: ");
                    CommandLineActions.Feed(args);
                    break;
                case "rest":
                    Debug.Log("Resting: ");
                    CommandLineActions.Rest(args);
                    break;
                case "exercise":
                    Debug.Log("Exercising: ");
                    CommandLineActions.Exercise(args);
                    break;
                case "work":
                    print("Working: ");
                    CommandLineActions.Work(methodCaller, args);
                    break;
                case "sell":
                    Debug.Log("Selling: ");
                    CommandLineActions.Sell(args);
                    break;
                case "buy":
                    Debug.Log("Buying: ");
                    CommandLineActions.Buy(args);
                    break;
                default:
                    break;
            }
            commandLine.text = "";
        }
    }
}
