using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaughterCubTrigger : MonoBehaviour
{
    public GameObject meatPrefab;
    public GameObject meatProduced;
    public Transform outputConveyorBeltPosition;
    public Coroutine placeOnConveyorBelt;
    private GameObject previousGo;

    private void OnTriggerStay(Collider other) 
    {   
        if(previousGo != null)
        {
            return;
        }

        if(other.gameObject.CompareTag("Cub"))
        {
            Cub cub = other.GetComponent<Cub>();
            meatProduced = Instantiate(meatPrefab);
            meatProduced.GetComponent<MeshRenderer>().enabled = false;
            meatProduced.transform.localScale = other.transform.localScale; 
            meatProduced.GetComponent<Rigidbody>().mass = meatProduced.transform.localScale.x * 9.81f;
            meatProduced.transform.position = outputConveyorBeltPosition.position;
            meatProduced.GetComponent<MeshRenderer>().enabled = true;
            Reward reward = meatProduced.GetComponent<Reward>();
            // Calculate and set rewards
            reward.MoneyReward = CalculateProduceReward(cub);
            // Update cubrooster
            Main.currentCubRooster.Remove(cub);
            AccountBalanceAI.UpdateCubCount(-1);
            // Start Coroutine to produce meat visual and spawn on other conveyor
            StartCoroutine("PlaceMeatProducedOnOutputConveyor", 5.0f);
            // Destroy gameObject
            previousGo = other.gameObject;
            Destroy(other.gameObject);
        }    
    }

    public int CalculateProduceReward(Cub cub)
    {
        return cub.valueRating * cub.tierRewards;
    }

    public IEnumerator PlaceMeatProducedOnOutputConveyor(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(meatProduced)
        {
            previousGo = null;
        }
    }
}
