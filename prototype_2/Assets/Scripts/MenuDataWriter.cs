using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuDataWriter : MonoBehaviour
{
    public GameObject menuWorkerDataPrefab;

    public void DeletePreviousMenu(GameObject parent)
    {
        for(int i = 0; i < parent.transform.childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
    }

    public void SetFormattedDataLine(GameObject parent)
    {        
        int len = WorkerManager.activeWorkers.Count;
        string formattedString = "";
        for(int i = 0; i < len; i++)
        {
            Worker w = WorkerManager.activeWorkers[i].gameObject.GetComponent<Worker>();
            formattedString += w.CurrentTask != null ? $"Worker: {w.Name} Task: {w.CurrentTask.CurrentWorkBatchProgress / w.CalculateCurrentTaskProgressRequired() * 100}%" : $"Worker: {w.Name} Task: N/A";
            formattedString += $"\nStamina: {w.Stamina}";
            formattedString += w.CurrentTask != null ? $" Task Salary: {w.CalculateCoinRequired(w.CurrentTask)}\n\n": $" Task Salary: No Task.\n\n";
        }
        parent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(formattedString);
    }
}