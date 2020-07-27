using UnityEngine;
using System; 
using System.Collections; 
using System.Collections.Generic;
using UnityEngine.UI;


public class WordSearch : MonoBehaviour {
    public Font f;
	// you may customize these variables in the Unity Inspector however you want
    public bool useWordpool; // 'should we use the wordpool?'
    public TextAsset wordpool; // if true, wordpool will be utilized
    public string[] words; // overwritten if wordpool = true
    public int maxWordCount; // max number of words used
	public int maxWordLetters; // max length of word used 
    public bool allowReverse; // if true, words can be selected in reverse order.
    public int gridX, gridY; // grid dimensions
    public float sensitivity; // sensitivity of tiles when clicked
    public float spacing; // spacing between tiles
    public GameObject tile, background, current;             
    public Color defaultTint, mouseoverTint, identifiedTint;
    public bool ready = false, correct = false;
    public string selectedString = "";
    public List<GameObject> selected = new List<GameObject>();

    private List<GameObject> tiles = new List<GameObject>();
    private GameObject temporary, backgroundObject;
    private int identified = 0;
    private float time;
    private string[,] matrix;
    private Dictionary<string, bool> word = new Dictionary<string, bool>();
    private Dictionary<string, bool> insertedWords = new Dictionary<string, bool>();
    private string[] letters = new string[26]
	{"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
    private Ray ray;
    private RaycastHit hit;
    private int mark = 0;
    //Images
    // public Sprite[] answerSprites;

    //  public GameObject answerImage = new GameObject();

    // public Image img;

    public Sprite Ear;
    public Sprite Heart;
    public Sprite Kidney;
    public Sprite Eye;
    public Sprite Brain;
    public Sprite Lungs;
    public Sprite Malaysia;
    public Sprite Airplane;
    public Sprite Angel;
    public Sprite Autumn;
    public Sprite Barbecue;
    public Sprite Butterfly;
    public Sprite Cactus;
    public Sprite Camp;
    public Sprite Candy;
    public Sprite Chameleon;
    public Sprite Cheese;
    public Sprite Coconut;
    public Sprite Computer;
    public Sprite Croissant;
    public Sprite Dinosaur;
    public Sprite Donut;
    public Sprite Farmer;
    public Sprite Fireworks;
    public Sprite Judge;
    public Sprite Monster;
    public Sprite Moon;
    public Sprite Mouth;
    public Sprite Parrot;
    public Sprite Perfume;
    public Sprite Pilot;
    public Sprite Pineapple;
    public Sprite Planet;
    public Sprite Playground;
    public Sprite Present;
    public Sprite Rainbow;
    public Sprite Rainy;
    public Sprite Saw;
    public Sprite School;
    public Sprite Summer;
    public Sprite Sun;
    public Sprite Sunny;
    public Sprite Tornado;
    public Sprite Tractor;
    public Sprite Volcano;
    public Sprite Watch;
    public Sprite Winter;
    public Sprite Apple;
    public Sprite Arm;
    public Sprite Banana;
    public Sprite Calf;
    public Sprite Face;
    public Sprite Feet;
    public Sprite Foot;
    public Sprite Grape;
    public Sprite Hair;
    public Sprite Hand;
    public Sprite Head;
    public Sprite Hip;
    public Sprite Kiwi;
    public Sprite Knee;
    public Sprite Leg;
    public Sprite Lemon;
    public Sprite Mango;
    public Sprite Neck;
    public Sprite Nose;
    public Sprite Papaya;
    public Sprite Pumpkin;
    public Sprite Rib;
    public Sprite Skin;
    public Sprite Strawberry;
    public Sprite Toe;
    public Sprite Tongue;
    public Sprite Tooth;
    public Sprite Watermelon;

    public Sprite annihilate;
    public Sprite antiquated;
    public Sprite behest;
    public Sprite belligerent;
    public Sprite culpable;
    public Sprite culprit;
    public Sprite ductile;
    public Sprite diabolical;
    public Sprite editorialize;
    public Sprite effervescence;
    public Sprite fecund;
    public Sprite firmament;
    public Sprite grandiose;
    public Sprite gravamen;
    public Sprite hapless;
    public Sprite harrowing;
    public Sprite implausible;
    public Sprite imponderable;
    public Sprite jaundiced;
    public Sprite junoesque;
    public Sprite kismet;
    public Sprite kowtow;
    public Sprite legerdemain;
    public Sprite lugubrious;
    public Sprite magnanimous;
    public Sprite malapropism;
    public Sprite noncommittal;
    public Sprite nugatory;
    public Sprite obstreperous;
    public Sprite orchestrate;
    public Sprite painstaking;
    public Sprite parsimonious;
    public Sprite quagmire;
    public Sprite quarantine;
    public Sprite realpolitik;
    public Sprite recrimination;
    public Sprite scapegoat;
    public Sprite sacrosanct;
    public Sprite tantamount;
    public Sprite transgression;
    public Sprite translucent;
    public Sprite unimpeachable;
    public Sprite utilitarian;
    public Sprite venturesome;
    public Sprite vehement;
    public Sprite wraith;
    public Sprite whimsical;
    public Sprite waggish;
    public Sprite xenophobia;
    public Sprite yearn;
    public Sprite yore;
    public Sprite zealot;
    public Sprite zeal;

    public SpriteRenderer spriteRenderer;


    private static WordSearch instance;
    public static WordSearch Instance {
        get {
			return instance;
		}
    }

	void Awake() {
        instance = this;
    }

    void Start() {
        List<string> findLength = new List<string>();
        int count = 0;

        if (useWordpool) {
            words = wordpool.text.Split(';');
        } else {
            maxWordCount = words.Length;
        }

        if (maxWordCount <= 0) {
            maxWordCount = 1;
        }

        Mix(words);
        Mathf.Clamp(maxWordLetters, 0, gridY < gridX ? gridX : gridY);
       
        while (findLength.Count < maxWordCount + 1) {
            if (words[count].Length <= maxWordLetters) {
                findLength.Add(words[count]);
            } 
			count++;
        }

        for (int i = 0; i < maxWordCount; i++) {
            if (!word.ContainsKey(findLength[i].ToUpper()) && !word.ContainsKey(findLength[i])) {
                    word.Add(findLength[i], false);
            }
        }

        Mathf.Clamp01(sensitivity);
        matrix = new string[gridX, gridY];
        InstantiateBG();

        for (int i = 0; i < gridX; i++) {
            for (int j = 0; j < gridY; j++) {
                temporary = Instantiate(tile, new Vector3(i * 1 * tile.transform.localScale.x * spacing, 10, j * 1 * tile.transform.localScale.z * spacing), Quaternion.identity) as GameObject;
                temporary.name = "tile-" + i.ToString() + "-" + j.ToString();
                temporary.transform.eulerAngles = new Vector3(180, 0, 0);
                temporary.transform.parent = backgroundObject.transform;
                BoxCollider boxCollider = temporary.GetComponent<BoxCollider>() as BoxCollider;
                boxCollider.size = new Vector3(sensitivity, 1, sensitivity);
                temporary.GetComponent<Letters>().letter.text = "";
                temporary.GetComponent<Letters>().gridX = i;
                temporary.GetComponent<Letters>().gridY = j;
                tiles.Add(tile);
                matrix[i, j] = "";
            }
        }
        CenterBG();
        InsertWords();
        FillRemaining();
        time = Time.time;


       // spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void CenterBG() {
        backgroundObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, (Screen.height / 2) + 50, 200));
	}

    private void InstantiateBG() {
		if (gridX % 2 != 0 && gridY % 2 == 0) {
			backgroundObject = Instantiate (background, new Vector3 ((tile.transform.localScale.x * spacing)
			* (gridX / 2), 1, (tile.transform.localScale.z * spacing)
			* (gridY / 2) - (tile.transform.localScale.z * spacing)), Quaternion.identity) as GameObject;
		} else if (gridX % 2 == 0 && gridY % 2 != 0) {
			backgroundObject = Instantiate (background, new Vector3 ((tile.transform.localScale.x * spacing) * (gridX / 2)
			- (tile.transform.localScale.x * spacing), 1, (tile.transform.localScale.z * spacing) * (gridY / 2)), Quaternion.identity) as GameObject;
		} else {
			backgroundObject = Instantiate(background, new Vector3 ((tile.transform.localScale.x * spacing) * (gridX / 2) -
				(tile.transform.localScale.x * spacing), 1, (tile.transform.localScale.z * spacing) * (gridY / 2) - (tile.transform.localScale.z * spacing)), Quaternion.identity) as GameObject;
		}
        backgroundObject.transform.eulerAngles = new Vector3(180, 0, 0);
        backgroundObject.transform.localScale = new Vector3(((tile.transform.localScale.x * spacing) * gridX), 1, ((tile.transform.localScale.x * spacing) * gridY));
   }

    void Update() {
		if (Input.GetMouseButton (0)) {
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				current = hit.transform.gameObject;
			}
			ready = true;
		}
		if (Input.GetMouseButtonUp (0)) {
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				current = hit.transform.gameObject;
			}
			Verify();
		}
	}

	private void Verify() {
        if (!correct) {
            foreach (KeyValuePair<string, bool> p in insertedWords) {
                if (selectedString.ToLower() == p.Key.Trim().ToLower()) {
                    foreach (GameObject g in selected) {
                        g.GetComponent<Letters>().identified = true;
                    }
                    correct = true;
                }
                if (allowReverse) {
                    if (Reverse(selectedString.ToLower()) == p.Key.Trim().ToLower()) {
                        foreach (GameObject g in selected) {
                            g.GetComponent<Letters>().identified = true;
                        }
                        correct = true;
                    }
                }
            }
        }

        if (correct) {
           // answerSprites = Resources.LoadAll<Sprite>("Results");
           // answerImage.AddComponent<SpriteRenderer>();
           // answerImage.GetComponent<SpriteRenderer>().sprite = answerSprites[1];
            insertedWords.Remove(selectedString);
            insertedWords.Remove(Reverse(selectedString));

			if (word.ContainsKey (selectedString)) {
				insertedWords.Add (selectedString, true);
			} else if (word.ContainsKey (Reverse (selectedString))) {
				insertedWords.Add (Reverse (selectedString), true);
			}
            ChangetheDamnSprite(selectedString);
            identified++;
        }
        ready = false;
        selected.Clear();
        selectedString = "";
        correct = false;

        
    }

    private void ChangetheDamnSprite(string spritename)
    {
        if (spritename == "heart")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Heart;
        }
        if (spritename == "eye")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Eye;
        }
        if (spritename == "ear")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Ear;
        }
        if (spritename == "brain")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Brain;
        }
        if (spritename == "kidney")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Kidney;
        }
        if (spritename == "lungs")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Lungs;
        }
        if (spritename == "malaysia")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Malaysia;
        }
        if (spritename == "airplane")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Airplane;
        }
        if (spritename == "angel")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Angel;
        }
        if (spritename == "autumn")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Autumn;
        }
        if (spritename == "barbecue")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Barbecue;
        }
        if (spritename == "butterfly")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Butterfly;
        }
        if (spritename == "cactus")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Cactus;
        }
        if (spritename == "camp")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Camp;
        }
        if (spritename == "candy")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Candy;
        }
        if (spritename == "chameleon")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Chameleon;
        }
        if (spritename == "cheese")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Cheese;
        }
        if (spritename == "coconut")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Coconut;
        }
        if (spritename == "computer")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Computer;
        }
        if (spritename == "croissant")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Croissant;
        }
        if (spritename == "dinosaur")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Dinosaur;
        }
        if (spritename == "donut")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Donut;
        }
        if (spritename == "farmer")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Farmer;
        }
        if (spritename == "fireworks")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Fireworks;
        }
        if (spritename == "judge")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Judge;
        }
        if (spritename == "monster")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Monster;
        }
        if (spritename == "moon")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Moon;
        }
        if (spritename == "mouth")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Mouth;
        }
        if (spritename == "pilot")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Pilot;
        }
        if (spritename == "perfume")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Perfume;
        }
        if (spritename == "pineapple")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Pineapple;
        }
        if (spritename == "planet")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Planet;
        }
        if (spritename == "playground")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Playground;
        }
        if (spritename == "present")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Present;
        }
        if (spritename == "rainbow")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Rainbow;
        }
        if (spritename == "rainy")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Rainy;
        }
        if (spritename == "saw")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Saw;
        }
        if (spritename == "school")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = School;
        }
        if (spritename == "summer")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Summer;
        }
        if (spritename == "sun")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Sun;
        }
        if (spritename == "sunny")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Sunny;
        }
        if (spritename == "tornado")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Tornado;
        }
        if (spritename == "tractor")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Tractor;
        }
        if (spritename == "volcano")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Volcano;
        }
        if (spritename == "watch")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Watch;
        }
        if (spritename == "winter")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Winter;
        }
        if (spritename == "apple")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Apple;
        }
        if (spritename == "arm")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Arm;
        }
        if (spritename == "banana")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Banana;
        }
        if (spritename == "calf")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Calf;
        }
        if (spritename == "face")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Face;
        }
        if (spritename == "feet")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Feet;
        }
        if (spritename == "foot")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Foot;
        }
        if (spritename == "grape")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Grape;
        }
        if (spritename == "hair")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Hair;
        }
        if (spritename == "hand")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Hand;
        }
        if (spritename == "head")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Head;
        }
        if (spritename == "hip")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Hip;
        }
        if (spritename == "kiwi")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Kiwi;
        }
        if (spritename == "knee")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Knee;
        }
        if (spritename == "leg")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Leg;
        }
        if (spritename == "lemon")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Lemon;
        }
        if (spritename == "mango")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Mango;
        }
        if (spritename == "neck")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Neck;
        }
        if (spritename == "nose")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Nose;
        }
        if (spritename == "papaya")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Papaya;
        }
        if (spritename == "pumpkin")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Pumpkin;
        }
        if (spritename == "rib")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Rib;
        }
        if (spritename == "skin")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Skin;
        }
        if (spritename == "strawberry")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Strawberry;
        }
        if (spritename == "toe")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Toe;
        }
        if (spritename == "tongue")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Tongue;
        }
        if (spritename == "tooth")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Tooth;
        }
        if (spritename == "watermelon")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = Watermelon;
        }

        if (spritename == "annihilate")// if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = annihilate;
        }
        if (spritename == "antiquated")
        {
            spriteRenderer.sprite = antiquated;
        }
        if (spritename == "behest")
        {
            spriteRenderer.sprite = behest;
        }
        if (spritename == "belligerent")
        {
            spriteRenderer.sprite = belligerent;
        }
        if (spritename == "culpable")
        {
            spriteRenderer.sprite = culpable;
        }
        if (spritename == "culprit")
        {
            spriteRenderer.sprite = culprit;
        }
        if (spritename == "ductile")
        {
            spriteRenderer.sprite = ductile;
        }
        if (spritename == "diabolical")
        {
            spriteRenderer.sprite = diabolical;
        }
        if (spritename == "editorialize")
        {
            spriteRenderer.sprite = editorialize;
        }
        if (spritename == "effervescence")
        {
            spriteRenderer.sprite = effervescence;
        }
        if (spritename == "fecund")
        {
            spriteRenderer.sprite = fecund;
        }
        if (spritename == "firmament")
        {
            spriteRenderer.sprite = firmament;
        }
        if (spritename == "grandiose")
        {
            spriteRenderer.sprite = grandiose;
        }
        if (spritename == "gravamen")
        {
            spriteRenderer.sprite = gravamen;
        }
        if (spritename == "hapless")
        {
            spriteRenderer.sprite = hapless;
        }
        if (spritename == "harrowing")
        {
            spriteRenderer.sprite = harrowing;
        }
        if (spritename == "implausible")
        {
            spriteRenderer.sprite = implausible;
        }
        if (spritename == "imponderable")
        {
            spriteRenderer.sprite = imponderable;
        }
        if (spritename == "jaundiced")
        {
            spriteRenderer.sprite = jaundiced;
        }
        if (spritename == "junoesque")
        {
            spriteRenderer.sprite = junoesque;
        }
        if (spritename == "kismet")
        {
            spriteRenderer.sprite = kismet;
        }
        if (spritename == "kowtow")
        {
            spriteRenderer.sprite = kowtow;
        }
        if (spritename == "legerdemain")
        {
            spriteRenderer.sprite = legerdemain;
        }
        if (spritename == "lugubrious")
        {
            spriteRenderer.sprite = lugubrious;
        }
        if (spritename == "magnanimous")
        {
            spriteRenderer.sprite = magnanimous;
        }
        if (spritename == "malapropism ")
        {
            spriteRenderer.sprite = malapropism;
        }
        if (spritename == "noncommittal")
        {
            spriteRenderer.sprite = noncommittal;
        }
        if (spritename == "nugatory")
        {
            spriteRenderer.sprite = nugatory;
        }
        if (spritename == "obstreperous")
        {
            spriteRenderer.sprite = obstreperous;
        }
        if (spritename == "orchestrate")
        {
            spriteRenderer.sprite = orchestrate;
        }
        if (spritename == "painstaking ")
        {
            spriteRenderer.sprite = painstaking;
        }
        if (spritename == "parsimonious")
        {
            spriteRenderer.sprite = parsimonious;
        }
        if (spritename == "quagmire")
        {
            spriteRenderer.sprite = quagmire;
        }
        if (spritename == "quarantine ")
        {
            spriteRenderer.sprite = quarantine;
        }
        if (spritename == "realpolitik ")
        {
            spriteRenderer.sprite = realpolitik;
        }
        if (spritename == "recrimination")
        {
            spriteRenderer.sprite = recrimination;
        }
        if (spritename == "scapegoat")
        {
            spriteRenderer.sprite = scapegoat;
        }
        if (spritename == "sacrosanct")
        {
            spriteRenderer.sprite = sacrosanct;
        }
        if (spritename == "tantamount")
        {
            spriteRenderer.sprite = tantamount;
        }
        if (spritename == "transgression")
        {
            spriteRenderer.sprite = transgression;
        }
        if (spritename == "translucent")
        {
            spriteRenderer.sprite = translucent;
        }
        if (spritename == "unimpeachable")
        {
            spriteRenderer.sprite = unimpeachable;
        }
        if (spritename == "utilitarian")
        {
            spriteRenderer.sprite = utilitarian;
        }
        if (spritename == "venturesome")
        {
            spriteRenderer.sprite = venturesome;
        }
        if (spritename == "vehement")
        {
            spriteRenderer.sprite = vehement;
        }
        if (spritename == "wraith")
        {
            spriteRenderer.sprite = wraith;
        }
        if (spritename == "whimsical")
        {
            spriteRenderer.sprite = whimsical;
        }
        if (spritename == "waggish")
        {
            spriteRenderer.sprite = waggish;
        }
        if (spritename == "xenophobia")
        {
            spriteRenderer.sprite = xenophobia;
        }
        if (spritename == "yearn")
        {
            spriteRenderer.sprite = yearn;
        }
        if (spritename == "yore")
        {
            spriteRenderer.sprite = yore;
        }
        if (spritename == "zealot")
        {
            spriteRenderer.sprite = zealot;
        }
        if (spritename == "zeal")
        {
            spriteRenderer.sprite = zeal;
        }


    }
    private void InsertWords()
    {
        System.Random rn = new System.Random();
        foreach (KeyValuePair<string, bool> p in word)
        {
            string s = p.Key.Trim();
            bool placed = false;
            while (placed == false)
            {
                int row = rn.Next(gridX);
                int column = rn.Next(gridY);
                int directionX = 0;
                int directionY = 0;
                while (directionX == 0 && directionY == 0)
                {
                    directionX = rn.Next(3) - 1;
                    directionY = rn.Next(3) - 1;
                }
                placed = InsertWord(s.ToLower(), row, column, directionX, directionY);
                mark++;
                if (mark > 100)
                {
                    break;
                }
            }
        }
    }

    private bool InsertWord(string word, int row, int column, int directionX, int directionY)
    {
        if (directionX > 0)
        {
            if (row + word.Length >= gridX)
            {
                return false;
            }
        }
        if (directionX < 0)
        {
            if (row - word.Length < 0)
            {
                return false;
            }
        }
        if (directionY > 0)
        {
            if (column + word.Length >= gridY)
            {
                return false;
            }
        }
        if (directionY < 0)
        {
            if (column - word.Length < 0)
            {
                return false;
            }
        }

        if (((0 * directionY) + column) == gridY - 1)
        {
            return false;
        }

        for (int i = 0; i < word.Length; i++)
        {
            if (!string.IsNullOrEmpty(matrix[(i * directionX) + row, (i * directionY) + column]))
            {
                return false;
            }
        }

        insertedWords.Add(word, false);
        char[] w = word.ToCharArray();
        for (int i = 0; i < w.Length; i++)
        {
            matrix[(i * directionX) + row, (i * directionY) + column] = w[i].ToString();
            GameObject.Find("tile-" + ((i * directionX) + row).ToString() + "-" + ((i * directionY) + column).ToString()).GetComponent<Letters>().letter.text = w[i].ToString();
        }
        return true;
    }

    private void FillRemaining() {
        for (int i = 0; i < gridX; i++) {
            for (int j = 0; j < gridY; j++) {
                if (matrix[i, j] == "") {
                    matrix[i, j] = letters[UnityEngine.Random.Range(0, letters.Length)];
                    GameObject.Find("tile-" + i.ToString() + "-" + j.ToString()).GetComponent<Letters>().letter.text = matrix[i, j];
                }
            }
        }
    }

    private void Mix(string[] words) {
        for (int t = 0; t < words.Length; t++) {
            string tmp = words[t];
            int r = UnityEngine.Random.Range(t, words.Length);
            words[t] = words[r];
            words[r] = tmp;
        }
    }

    private string TimeElapsed() {
        TimeSpan t = TimeSpan.FromSeconds(Mathf.RoundToInt(Time.time - time));
        return String.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        
    }

    private string Reverse(string word) {
        string reversed = "";
        char[] letters = word.ToCharArray();
        for (int i = letters.Length - 1; i >= 0; i--) {
            reversed += letters[i];
        }
        return reversed;
    }
    Vector2 scrollPosition;
        void OnGUI()
    {   

        GUI.skin.font = f;
        // The timer gui indicator 
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("   Timer:");
        GUILayout.Label(TimeElapsed());
        GUILayout.EndHorizontal();
        //The questions list scrollview gui 
        GUILayout.Label("   [Find The Words]");
        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, GUILayout.Width(250), GUILayout.Height(280));

                // get all the value(words) from the words database
                foreach (KeyValuePair<string, bool> p in insertedWords)
                {

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("   " + p.Key);
                    if (p.Value)
                    {
                        GUILayout.Label("*");
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }


}