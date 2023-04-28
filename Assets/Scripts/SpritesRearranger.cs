using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SpritesRearranger : MonoBehaviour
{
    // The blank space
    public GameObject blankSpace;

    // The blank space
    public GameObject spriteObjectPrefab;

    // The sprites
    public List<GameObject> sprites = new List<GameObject>();

    // Width of grid
    public int gridWidth;
    // Height of grid
    public int gridHeight;

    public float stepSize;

    private Object[] spritesObjects;
    private SpriteRenderer sr;
    private string[] names;

    public string pathName = "Assets/sprites/dragon.png";

    bool GameStarted = false;

    // Number of shuffle we do before game start
    public int numShuffle;

    public GameObject winText;

    bool GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        winText.SetActive(false);

        // Get all the slices of the sprite and 
        Object[] data = AssetDatabase.LoadAllAssetsAtPath(pathName);
        if (data != null)
        {
            foreach (Object obj in data)
            {
                if (obj.GetType() == typeof(Sprite))
                {
                    Sprite sprite = obj as Sprite;
                    GameObject instance = Instantiate(spriteObjectPrefab);
                    instance.GetComponent<SpriteRenderer>().sprite = sprite;
                    sprites.Add(instance);
                }
            }
        }

        // Arrange the sprites
        for (int x = 0; x < gridHeight; x++)
        {
            for (int y = 0; y < gridWidth; y++)
            {
                Vector3 pos = new Vector3(transform.position.x + x * stepSize, transform.position.y - y * stepSize, 0f);
                sprites[y * gridWidth + x].transform.position = pos;
                sprites[y * gridWidth + x].GetComponent<SpriteStat>().initialPos = pos;
                sprites[y * gridWidth + x].GetComponent<SpriteStat>().index = y * gridWidth + x;
            }
        }

        // Assign the blank space pos
        Vector3 blankSpacePos = new Vector3(transform.position.x, transform.position.y + stepSize, 0f);
        blankSpace.transform.position = blankSpacePos;

        // Move up first
        moveUp();

        // Shuffle the image to create the level
        for(int i = 0; i<numShuffle; i++)
        {
            int rnd = Random.Range(0, 4);
            if(rnd == 0)
            {
                moveUp();
            }
            else if(rnd == 1)
            {
                moveDown();
            }
            else if(rnd == 2)
            {
                moveLeft();
            }
            else if(rnd == 3)
            {
                moveRight();
            }
        }

        GameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameOver)
        {
            // Move up
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveUp();
            }

            // Move down
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveDown();
            }

            // Move left
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveLeft();
            }

            // Move right
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveRight();
            }

            checkWin();
        }
    }

    // Check if win
    void checkWin()
    {
        int numIncorrect = 0;

        foreach (GameObject sprite in sprites)
        {
            if (Vector3.Distance(sprite.gameObject.transform.position, sprite.gameObject.GetComponent<SpriteStat>().initialPos) > 0.1f)
            {
                numIncorrect++;
            }
        }

        if(numIncorrect == 0 && GameStarted)
        {
            GameOver = true;

            winText.SetActive(true);
        }
    }

    void moveUp()
    {
        Vector3 posSprite = new Vector3(blankSpace.transform.position.x, blankSpace.transform.position.y - stepSize, 0f);

        foreach (GameObject sprite in sprites)
        {
            if (Vector3.Distance(sprite.transform.position, posSprite) < 0.1f)
            {
                Vector3 spritePosTracker = sprite.transform.position;
                sprite.transform.position = new Vector3(blankSpace.transform.position.x, blankSpace.transform.position.y, 0f);

                blankSpace.transform.position = spritePosTracker;
            }
        }
    }

    void moveDown()
    {
        Vector3 posSprite = new Vector3(blankSpace.transform.position.x, blankSpace.transform.position.y + stepSize, 0f);

        foreach (GameObject sprite in sprites)
        {
            if (Vector3.Distance(sprite.transform.position, posSprite) < 0.1f)
            {
                Vector3 spritePosTracker = sprite.transform.position;
                sprite.transform.position = new Vector3(blankSpace.transform.position.x, blankSpace.transform.position.y, 0f);

                blankSpace.transform.position = spritePosTracker;
            }
        }
    }

    void moveLeft()
    {
        Vector3 posSprite = new Vector3(blankSpace.transform.position.x + stepSize, blankSpace.transform.position.y, 0f);

        foreach (GameObject sprite in sprites)
        {
            if (Vector3.Distance(sprite.transform.position, posSprite) < 0.1f)
            {
                Vector3 spritePosTracker = sprite.transform.position;
                sprite.transform.position = new Vector3(blankSpace.transform.position.x, blankSpace.transform.position.y, 0f);

                blankSpace.transform.position = spritePosTracker;
            }
        }
    }

    void moveRight()
    {
        Vector3 posSprite = new Vector3(blankSpace.transform.position.x - stepSize, blankSpace.transform.position.y, 0f);

        foreach (GameObject sprite in sprites)
        {
            if (Vector3.Distance(sprite.transform.position, posSprite) < 0.1f)
            {
                Vector3 spritePosTracker = sprite.transform.position;
                sprite.transform.position = new Vector3(blankSpace.transform.position.x, blankSpace.transform.position.y, 0f);

                blankSpace.transform.position = spritePosTracker;
            }
        }
    }
}
