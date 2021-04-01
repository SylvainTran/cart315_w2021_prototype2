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
        string? validatedCommand = CommandLineExecutor.ValidateCommand(commandLine.text);
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

        public static void Work(List<string> args)
        {
            throw new NotImplementedException();
        }

        public static void Sell(List<string> args)
        {
            throw new NotImplementedException();
        }

        public static void Buying(List<string> args)
        {
            throw new NotImplementedException();
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
            commandLineText = commandLineText.Trim().ToLower();
            return commandLineText;
        }

        public static void ExecuteCommand(string commandLineText)
        {
            List<string> args = new List<string>();
            int argsStartIndex = commandLineText.IndexOf('(');
            int argsEndIndex = commandLineText.IndexOf(')');
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
                            string[] _args = commandLineText.Substring(argsStartIndex + 1, argsRange).Split(',');
                            args.AddRange(_args);
                        }
                    } else
                    {
                        commandLineText = commandLineText.Replace('(', ' ').Replace(')', ' ').Trim();
                    }
                }
            } catch(System.ArgumentOutOfRangeException e)
            {
                Debug.LogError(e.Message);
            } catch(System.Exception e)
            {
                Debug.LogError(e.Message);
            }

            string methodCall = commandLineText.Substring(0, argsStartIndex);
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
                    Debug.Log("Working: ");
                    CommandLineActions.Work(args);
                    break;
                case "sell":
                    Debug.Log("Selling: ");
                    CommandLineActions.Sell(args);
                    break;
                case "buy":
                    Debug.Log("Buying: ");
                    CommandLineActions.Buying(args);
                    break;
                default:
                    break;
            }
            commandLine.text = "";
        }
    }
}
