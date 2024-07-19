using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TeamInfoDisplayManager : MonoBehaviour
{
    public TextAsset jsonFile; // Drag and drop your JSON file here
    public GameObject playerInfoPrefab; // Assign the PlayerInfoPrefab here
    public Transform miniMapParent; // Assign the parent transform for player info objects

    // Assign these transforms in the inspector
    public Transform GK;
    public Transform CB;
    public Transform LB;
    public Transform RB;
    public Transform LWB;
    public Transform RWB;
    public Transform CM;
    public Transform CDM;
    public Transform CAM;
    public Transform LM;
    public Transform RM;
    public Transform ST;
    public Transform CF;
    public Transform LW;
    public Transform RW;
    public Transform EmptyPosition; // Fallback position for players without a valid position

    public Vector3 playerScale = new Vector3(0.5f, 0.5f, 0.5f); // Set the default player scale in the inspector

    // UI elements for displaying player details
    public Image countryFlag; // Assign the Image UI element for displaying the player's country flag
    public Image playerImage; // Assign the Image UI element for displaying the player's image
    public TMP_Text playerNameText; // Assign the TMP_Text UI element for displaying the player's name
    public TMP_Text playerAgeText; // Assign the TMP_Text UI element for displaying the player's age
    public TMP_Text playerNumberText; // Assign the TMP_Text UI element for displaying the player's number
    public TMP_Text playerPositionText; // Assign the TMP_Text UI element for displaying the player's position

    private TeamArray allTeams;
    private List<GameObject> instantiatedPlayers = new List<GameObject>(); // Store instantiated player objects

    void Start()
    {
        LoadJsonData();
    }

    void LoadJsonData()
    {
        if (jsonFile != null)
        {
            allTeams = JsonUtility.FromJson<TeamArray>(jsonFile.text);
            Debug.Log("JSON Data Loaded Successfully");
        }
        else
        {
            Debug.LogError("JSON file is not assigned!");
        }
    }

    public void DisplayTeamInformation(string teamName)
    {
        if (allTeams == null)
        {
            Debug.LogError("No team data loaded");
            return;
        }

        // Clear previous player info objects
        ClearPreviousPlayers();

        Dictionary<string, Transform> positionMapping = new Dictionary<string, Transform>
        {
            { "GK", GK },
            { "CB", CB },
            { "LB", LB },
            { "RB", RB },
            { "LWB", LWB },
            { "RWB", RWB },
            { "CM", CM },
            { "CDM", CDM },
            { "CAM", CAM },
            { "LM", LM },
            { "RM", RM },
            { "ST", ST },
            { "CF", CF },
            { "LW", LW },
            { "RW", RW }
        };

        HashSet<Transform> occupiedPositions = new HashSet<Transform>();
        List<Player> playersToInstantiate = new List<Player>();

        foreach (Team team in allTeams.teams)
        {
            if (team.country == teamName)
            {
                Debug.Log("Team Country: " + team.country);
                Debug.Log("Jersey Color: " + team.jerseyColor);
                foreach (Player player in team.players)
                {
                    if (playersToInstantiate.Count < 11)
                    {
                        playersToInstantiate.Add(player);
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            }
        }

        int playerCount = 0;

        foreach (Player player in playersToInstantiate)
        {
            if (playerCount >= 11) break;

            Transform positionTransform;

            if (positionMapping.TryGetValue(player.position, out positionTransform) && positionTransform != null && !occupiedPositions.Contains(positionTransform))
            {
                // Use the mapped position
            }
            else
            {
                // Find the first available fallback position
                positionTransform = EmptyPosition;
                foreach (var position in positionMapping.Values)
                {
                    if (!occupiedPositions.Contains(position))
                    {
                        positionTransform = position;
                        break;
                    }
                }
            }

            GameObject playerInfo = Instantiate(playerInfoPrefab, positionTransform);
            playerInfo.transform.localPosition = Vector3.zero;
            playerInfo.transform.localRotation = Quaternion.identity;
            playerInfo.transform.localScale = playerScale;

            playerInfo.tag = "PlayerInfo";

            playerInfo.transform.Find("PlayerName").GetComponent<TMP_Text>().text = player.name;
            playerInfo.transform.Find("PlayerNumber").GetComponent<TMP_Text>().text = player.number.ToString();
            playerInfo.transform.Find("PlayerPosition").GetComponent<TMP_Text>().text = player.position.ToUpper(); // Convert to uppercase

            // Add a button component if not already present
            Button button = playerInfo.GetComponent<Button>();
            if (button == null)
            {
                button = playerInfo.AddComponent<Button>();
            }

            // Set up the click event listener
            button.onClick.AddListener(() => OnPlayerClick(player));

            occupiedPositions.Add(positionTransform);
            instantiatedPlayers.Add(playerInfo); // Add instantiated player to the list
            playerCount++;
        }

        // If there are less than 11 players, add empty positions
        while (playerCount < 11)
        {
            Transform positionTransform = EmptyPosition;
            foreach (var position in positionMapping.Values)
            {
                if (!occupiedPositions.Contains(position))
                {
                    positionTransform = position;
                    break;
                }
            }

            GameObject playerInfo = Instantiate(playerInfoPrefab, positionTransform);
            playerInfo.transform.localPosition = Vector3.zero;
            playerInfo.transform.localRotation = Quaternion.identity;
            playerInfo.transform.localScale = playerScale;

            playerInfo.tag = "PlayerInfo";

            playerInfo.transform.Find("PlayerName").GetComponent<TMP_Text>().text = "Empty";
            playerInfo.transform.Find("PlayerNumber").GetComponent<TMP_Text>().text = "";
            playerInfo.transform.Find("PlayerPosition").GetComponent<TMP_Text>().text = "";

            occupiedPositions.Add(positionTransform);
            instantiatedPlayers.Add(playerInfo); // Add instantiated player to the list
            playerCount++;
        }
    }

    private void OnPlayerClick(Player player)
    {
        // Update the UI elements for player details
        playerNameText.text = player.name;
        playerAgeText.text = player.age.ToString();
        playerNumberText.text = player.number.ToString();
        playerPositionText.text = player.position.ToUpper();

        // You can update the countryFlag and playerImage manually in the inspector or through another method if needed
    }

    public void ClearPreviousPlayers()
    {
        foreach (GameObject player in instantiatedPlayers)
        {
            Destroy(player);
        }
        instantiatedPlayers.Clear(); // Clear the list of instantiated players
    }

    [System.Serializable]
    public class Team
    {
        public string country;
        public string jerseyColor;
        public Player[] players;
    }

    [System.Serializable]
    public class Player
    {
        public string name;
        public int age;
        public int number;
        public string position;
    }

    [System.Serializable]
    public class TeamArray
    {
        public Team[] teams;
    }
}
