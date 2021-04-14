using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CommandLineController : MonoBehaviour
{
    public Canvas commandLineCanvas;
    public GameObject commandLineInputField;
    public static TMP_InputField commandLine;
    public delegate void OnCommandLineFocused();
    public static event OnCommandLineFocused onCommandLineFocused;
    public delegate void OnCommandLineDeFocused();
    public static event OnCommandLineDeFocused onCommandLineDeFocused;
    public delegate void OnDeclaredBankruptcy();
    public static event OnDeclaredBankruptcy onDeclaredBankruptcy;

    // UI
    public delegate void OnWorkerStatisticsUpdated();
    public static event OnWorkerStatisticsUpdated onWorkerStatisticsUpdated;
    // Sound effects
    public GameObject soundEffectsGO;

    public void Start()
    {
        commandLine = GameObject.FindGameObjectWithTag("CommandLine").GetComponent<TMP_InputField>();
        commandLine.onSubmit.AddListener((data) => { ParseCommand(data); });
        commandLineCanvas.GetComponent<Canvas>().enabled = false;
    }

    public void Update()
    {
        if(commandLine.isFocused == true)
        {
           onCommandLineFocused();
        } else
        {
           onCommandLineDeFocused();
        }
    }

    public static void ParseCommand(string data)
    {
        if (!commandLine) return;
        string validatedCommand = CommandLineExecutor.ValidateCommand(data);
        if (validatedCommand != null)
        {
            CommandLineExecutor.ExecuteCommand(validatedCommand);
        }
    }

    public class CommandLineActions
    {
        public static void Feed(string methodCaller, List<string> args)
        {
            if(args == null)
            {
                // Feed all case
                Debug.Log("Feeding all cubs");
                if(AccountBalanceAI.cubFood >= 10)
                {
                    for(int i = 0; i < Main.currentCubRooster.Count; i++)
                    {
                        Main.currentCubRooster[i].Satiety += 10;
                        AccountBalanceAI.cubFood -= 10;
                        AccountBalanceAI.UpdateTotalBalance();
                    }
                }
            } else
            {
                foreach(string arg in args)
                {
                    Debug.Log($"{arg}");
                }
            }
        }

        public static void Rest(string methodCaller, List<string> args)
        {
            List<GameObject> activeWorkers = WorkerManager.activeWorkers;
            int len = activeWorkers.Count;
            if (len == 0)
            {
                return;
            }
            for (int i = 0; i < len; i++)
            {
                Worker w = activeWorkers[i].GetComponent<Worker>();

                if (w.Name.ToLower().Equals(methodCaller))
                {
                    if (!args[0].Equals(string.Empty)) // dude.rest(5) // dude, rest 5 hours
                    {
                        print("Pause working hours specific rom CLI called");
                        print($"Pause time: {Single.Parse(args[0])}");
                        if (w.StartWorkCoroutine != null)
                        {
                            w.StopCoroutine("StartWorking");
                            w.StartWorkCoroutine = null;
                        }
                        w.StopWorkCoroutine = w.StartCoroutine("PauseWorking", Single.Parse(args[0]));
                        UpdateRestStatus(w);
                    }
                    else
                    {
                        // Default pause time is one tick
                        print("Pause working generic from CLI called");
                        if (w.StartWorkCoroutine != null)
                        {
                            w.StopCoroutine("StartWorking");
                            w.StartWorkCoroutine = null;
                        }
                        w.StopWorkCoroutine = w.StartCoroutine("PauseWorking", 1.0f); // 1 hour default test
                        UpdateRestStatus(w);
                    }
                }
                print(w.ToString());
            }
        }

        public static void Exercise(List<string> args)
        {
            throw new NotImplementedException();
        }

        public static void CreateTask(List<string> args)
        {
            TaskController.InitTask(new Task(), args);
            onWorkerStatisticsUpdated();
        }

        public static void Work(string methodCaller, List<string> args)
        {
            if (args == null)
            {
                return;
            }
            Debug.Log("New work process initiated.");
            List<GameObject> activeWorkers = WorkerManager.activeWorkers;
            int len = activeWorkers.Count;
            if(len == 0)
            {
                print("No active workers");
                return;
            }
            for (int i = 0; i < len; i++)
            {
                Worker w = activeWorkers[i].GetComponent<Worker>();
                print("Worker: " + w.Name);
                if(w.Name.ToLower().Equals(methodCaller))
                {
                    if(!args[0].Equals(string.Empty)) // dude.work(5) // work 5 hours dude
                    {
                        print("current task " + w.CurrentTask);
                        if(w.CurrentTask == null && TaskController.tasksQueue.Count > 0) 
                        {
                            if(w.StopWorkCoroutine != null)
                            {
                                w.StopCoroutine(w.StopWorkCoroutine);
                                w.StopWorkCoroutine = null;
                            }
                            w.CurrentTask = TaskController.GetTaskFromQueue();
                            w.CurrentTask.ProgressHoursRequired = Single.Parse(args[0]); // currently just hours, could add batch limits later
                            if (AccountBalanceAI.money < w.CalculateCoinRequired(w.CurrentTask))
                            {
                                print($"Not enough coins to pay worker salary: {w.CalculateCoinRequired(w.CurrentTask)}");
                                return;
                            }
                            w.StartWorkCoroutine = w.StartCoroutine("StartWorking");
                            UpdateWorkStatus(w);
                            onWorkerStatisticsUpdated();
                        }
                        else if(w.CurrentTask != null)
                        {
                            print("resume working from resting condition");
                            // resume working from resting condition
                            if (w.StopWorkCoroutine != null)
                            {
                                w.StopCoroutine(w.StopWorkCoroutine);
                                w.StopWorkCoroutine = null;
                            }
                            w.StartWorkCoroutine = w.StartCoroutine("StartWorking");
                            UpdateWorkStatus(w);
                            onWorkerStatisticsUpdated();
                        }
                        else 
                        {
                            print("No tasks left. Please create a new task with createTask(hours, workbatchlimit)");
                        }
                    }
                }
                print(w.ToString());          
            }
        }

        public static void Train(List<string> args)
        {

        }

        public static void UpdateRestStatus(Worker w)
        {
            w.InternalMoveToDestination("RESTING_SANCTUARY");
            w.MoveToDestination(w.RESTING_SANCTUARY.instance.gameObject.transform.position);
            w.CheckLocationAction();
        }

        public static void UpdateWorkStatus(Worker w)
        {
            w.InternalMoveToDestination("WORKFIELD_1");
            w.MoveToDestination(w.WORKFIELD_1.instance.gameObject.transform.position);
            w.CheckLocationAction(); // 10 workbatches is default                        
        }

        public static void Sell(List<string> args)
        {
            if (args == null)
            {
                Debug.Log("Feeding all cubs");
                if (AccountBalanceAI.cubFood >= 10)
                {
                    for (int i = 0; i < Main.currentCubRooster.Count; i++)
                    {
                        Main.currentCubRooster[i].Satiety += 10;
                        AccountBalanceAI.cubFood -= 10;
                        AccountBalanceAI.UpdateTotalBalance();
                    }
                }
            }
            else if(args[0] != null && args[1] != null)
            {
                if(int.Parse(args[0]) > 0 && args[1].ToLower().Equals("food"))
                {
                    if(AccountBalanceAI.cubFood >= int.Parse(args[0]))
                    {
                        float foodValue = 2.5f;
                        AccountBalanceAI.UpdateFood(-int.Parse(args[0]));
                        AccountBalanceAI.UpdateMoney(int.Parse(args[0]) * foodValue);
                        print($"Sold x{int.Parse(args[0])} food for {int.Parse(args[0]) * foodValue}");
                    }
                    else
                    {
                        print("Not enough food of that quantity to sell");
                    }
                }
            }
        }

        public static void Examine(List<string> args)
        {
            throw new NotImplementedException();
        }

        public static void FreeMoneyCheatCode(List<string> args)
        {
            if(args == null)
            {
                return;
            }
            AccountBalanceAI.UpdateMoney(Single.Parse(args[0]));
        }

        public static void FreeFoodCheatCode(List<string> args)
        {
            if(args == null)
            {
                return;
            }
            AccountBalanceAI.UpdateFood(int.Parse(args[0]));
        }

        private void Awake()
        {
            GameObject[] explosions = GameObject.FindGameObjectsWithTag("FxTemporaire");                                  
            foreach(GameObject explosion in explosions)
            {
                ParticleSystem childPS = explosion.GetComponent<ParticleSystem>();
                print("Found one explosion" + childPS);
                childPS.Stop();
            }
        }

        public static void DeclareBankruptcy(List<string> args)
        {
            onDeclaredBankruptcy();
            // Destroy Everything
            Destroy(GameObject.FindGameObjectWithTag("ProgramManagement").gameObject);
            Destroy(GameObject.FindGameObjectWithTag("Menagerie").gameObject);
            Destroy(GameObject.FindGameObjectWithTag("Slaughterhouse").gameObject);
            Destroy(GameObject.FindGameObjectWithTag("RestingSanctuary").gameObject);  
            GameObject[] explosions = GameObject.FindGameObjectsWithTag("FxTemporaire");                                  
            foreach(GameObject explosion in explosions)
            {
                ParticleSystem childPS = explosion.GetComponent<ParticleSystem>();
                childPS.Play();
            }
        }

        public static void Buy(List<string> args)
        {
            if (AccountBalanceAI.money == 0)
            {
                print("Broke.");
                return;
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
                                if(!Main.currentCubRoosterFull && AccountBalanceAI.money >= 50)
                                {
                                    print("Buying a new cub.");
                                    Main.LevelController.GenerateNewCub(qty, args[i]);
                                    AccountBalanceAI.UpdateMoney(-50);
                                } else
                                {
                                    Debug.Log("Cub rooster is full or lacking sufficient funds.");
                                }
                            }
                            else if(args[i].Equals("worker"))
                            {
                                if(WorkerManager.activeWorkers.Count < 3)
                                {
                                    WorkerManager.BuyWorker(int.Parse(args[0]));
                                    onWorkerStatisticsUpdated();
                                } else 
                                {
                                    print("Full worker capacity reached. Max is 3.");
                                }
                            }
                            else
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
            commandLineText = commandLineText.Trim().ToLower().Replace(" ", string.Empty);
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
                        args = null;
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
                //We need to distinguish subroutines that are part of tasks from the high-level management stuff we're interested in
                case "createtask":
                    if(args != null) CommandLineActions.CreateTask(args);                
                    else {
                        print("Work command registered successfully. Invoking call... Cannot. Need to provide required progress hours, or work batch limit to create a new task.");
                    }
                    break;
                case "work": // this is essentially dispatch
                    print("Work command registered successfully. Invoking call...");
                    CommandLineActions.Work(methodCaller, args);
                    break;
                case "rest": // sadly people have to do this shameful thing
                    CommandLineActions.Rest(methodCaller, args);
                    break;
                case "feed":
                    CommandLineActions.Feed(methodCaller, args);
                    break;
                case "sell":
                    CommandLineActions.Sell(args);
                    break;
                case "buy":
                    CommandLineActions.Buy(args);
                    break;
                case "train":
                    CommandLineActions.Train(args);
                    break;
                case "helpmedaddy":
                    CommandLineActions.FreeMoneyCheatCode(args);
                    break;
                case "helpmemommy":
                    CommandLineActions.FreeFoodCheatCode(args);
                    break;
                case "declarebankruptcy":
                    CommandLineActions.DeclareBankruptcy(args);                
                    break;                    
                default:
                    break;
            }
            commandLine.text = "";
        }
    }
}
