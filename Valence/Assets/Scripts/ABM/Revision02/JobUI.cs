using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JobUI : MonoBehaviour {

    //Assign agent spawner to this GUI
    AgentSpawner agentSpawner;

    //UI text that displays the current agent population size
    Text populationText;

    //UI text that displays the current number of farmer agents
    Text farmerText;

    //UI text that displays the current number of medic agents
    Text medicText;

    //keep track of the current number of farmers
    public int farmerCount;

    //keep track of the current number of medics
    public int medicCount;

    void Start() {

        populationText = GameObject.Find("PopulationCount").GetComponent<Text>();
        farmerText = GameObject.Find("FarmerCount").GetComponent<Text>();
        medicText = GameObject.Find("MedicCount").GetComponent<Text>();

        agentSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AgentSpawner>();
    }

    void Update() {

        populationText.text = "Laborer " + agentSpawner.popSize.ToString();
        farmerText.text = "Farmer: " + farmerCount.ToString();
        medicText.text = "Medic: " + medicCount.ToString();
    }
}
