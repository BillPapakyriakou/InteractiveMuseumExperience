using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public Camera GameCamera;
    public Camera PuzzleCamera;

    CameraController controller;

    public Texture2D image;
    public int blocksPerLine = 4;
    public int shuffleLength = 20;
    public float defaultMoveDuration = .4f;
    public float shuffleMoveDuration = .3f;

    enum PuzzleState { Solved, Shuffling, InPlay };
    PuzzleState state;
                    

    Block emptyBlock;
    Block[,] blocks;
    Queue<Block> inputs;

    bool blockIsMoving;

    public bool puzzleIsActive;

    public bool puzzlePromptUIActive;
    public bool puzzleInPlayUIActive;
    public bool puzzleCompletedUIActive;

    public GameObject puzzlePromptUI;
    public GameObject puzzleInPlayUI;
    public GameObject puzzleCompletedUI;

    int shuffleMovesRemaining;

    Vector2Int previousShuffleOffset;

    void Start()
    {
        CreatePuzzle(); 
        PuzzleCamera.enabled = false;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !FindObjectOfType<QuizManager>().quizIsActive && !FindObjectOfType<DialogueManager>().isDialogueShown &&!FindObjectOfType<PauseMenu>().gameIsPaused)
        {
            InitiatePuzzle();     
        }

        if (Input.GetKeyUp(KeyCode.Escape) && puzzleIsActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Debug.Log("exit");
            ExitPuzzle();
        }

        /*
        if (state == PuzzleState.Solved && Input.GetKeyDown(KeyCode.Space)) 
        {
            StartShuffle();
        }   
        */
    }

    void CreatePuzzle()
    {
        blocks = new Block[blocksPerLine, blocksPerLine];
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, blocksPerLine);
        for (int y = 0; y < blocksPerLine; y++) 
        {
            for (int x = 0; x < blocksPerLine; x++) 
            {
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad);

                /*
                blockObject.transform.position = -Vector2.one * (blocksPerLine - 1) * .5f + new Vector2(x, y);
                blockObject.transform.parent = transform;
                */

                Vector2 parentPosition = transform.position;
                Vector2 blockPosition = parentPosition - Vector2.one * (blocksPerLine - 1) * 0.5f + new Vector2(x, y);

                // Set the calculated position
                blockObject.transform.position = blockPosition;
                blockObject.transform.parent = transform;

                Block block = blockObject.AddComponent<Block>();
                block.OnBlockPressed += PlayerMoveBlockInput;  // subscribe method to event
                block.OnFinishedMoving += OnBlockFinishedMoving;  // subscribe method to event
                block.Init(new Vector2Int(x, y), imageSlices[x, y]);
                blocks[x, y] = block;

                if (y == 0 && x == blocksPerLine - 1)
                {
                    //blockObject.SetActive(false);
                    emptyBlock = block;
                }
            }

        }

        //Camera.main.orthographicSize = blocksPerLine * .55f;
        inputs = new Queue<Block>();  // initialize queue of blocks
    }

    void PlayerMoveBlockInput(Block blockToMove)
    {
        if (state == PuzzleState.InPlay) 
        {
            inputs.Enqueue(blockToMove);
            MakeNextPlayerMove();

        }
    }

    void MakeNextPlayerMove()
    {

        while (inputs.Count > 0 && !blockIsMoving)
        {
            MoveBlock(inputs.Dequeue(), defaultMoveDuration);
        }
    }

    void MoveBlock(Block blockToMove, float duration)
    {
        if ((blockToMove.coord - emptyBlock.coord).sqrMagnitude == 1)
        {

            blocks[blockToMove.coord.x, blockToMove.coord.y] = emptyBlock;
            blocks[emptyBlock.coord.x, emptyBlock.coord.y] = blockToMove;

            Vector2Int targetCoord = emptyBlock.coord;
            emptyBlock.coord = blockToMove.coord;
            blockToMove.coord = targetCoord;

            Vector2 targetPosition = emptyBlock.transform.position;
            emptyBlock.transform.position = blockToMove.transform.position;
            blockToMove.MoveToPosition(targetPosition, duration);
            blockIsMoving = true;
        }
    }

    void OnBlockFinishedMoving()
    {
        blockIsMoving = false;
        CheckIfSolved();

        if (state == PuzzleState.InPlay)
        {
            MakeNextPlayerMove();
        }

        else if (state == PuzzleState.Shuffling) { 

            if (shuffleMovesRemaining > 0)
            {
                MakeNextShuffleMove();
            }
            else
            {
                state = PuzzleState.InPlay;
            }
        }
    }

    public void StartShuffle()
    {
        state = PuzzleState.Shuffling;
        shuffleMovesRemaining = shuffleLength;
        emptyBlock.gameObject.SetActive(false);
        MakeNextShuffleMove();
    }

    void MakeNextShuffleMove()
    {
        Vector2Int[] offsets = { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        //                             right                   left                   above                 below
        int randomIndex = Random.Range(0, offsets.Length);
        
        for (int i = 0; i < offsets.Length; i++)  // if out of bounds keep trying
        {
            Vector2Int offset = offsets[(randomIndex + i) % offsets.Length];

            if (offset != previousShuffleOffset * -1)
            {
                Vector2Int moveBlockCoord = emptyBlock.coord + offset;

                if (moveBlockCoord.x >= 0 && moveBlockCoord.x < blocksPerLine && moveBlockCoord.y >= 0 && moveBlockCoord.y < blocksPerLine)
                {
                    MoveBlock(blocks[moveBlockCoord.x, moveBlockCoord.y], shuffleMoveDuration);
                    shuffleMovesRemaining--;
                    previousShuffleOffset = offset;
                    break;
                }
            }           
        }

    }

    void CheckIfSolved()
    {
        foreach (Block block in blocks)
        {
            if (!block.IsAtStartingCoord())
            {
                return;
            }
        }

        state = PuzzleState.Solved;
        emptyBlock.gameObject.SetActive(true);
    }

    public void InitiatePuzzle()  // puzzle minigame prompt
    {
        puzzleIsActive = true;

        Cursor.lockState = CursorLockMode.None;

        controller = FindObjectOfType<CameraController>();
        controller.DisableCameraMovement();

        puzzlePromptUI.SetActive(true);
        puzzlePromptUIActive = true;
    }

    public void StartPuzzle()   // puzzle minigame start
    {
        state = PuzzleState.InPlay;

        PuzzleCamera.gameObject.SetActive(true);

        GameCamera.enabled = false;
        PuzzleCamera.enabled = true;

        puzzlePromptUI.SetActive(false);
        puzzlePromptUIActive = false;

        puzzleInPlayUI.SetActive(true);
        puzzlePromptUIActive = true;

    }

    public void PuzzleOver()  
    {
        if (state == PuzzleState.Solved)
        {
            puzzleInPlayUI.SetActive(false);
            puzzleInPlayUIActive = false;

            puzzleCompletedUI.SetActive(true);
            puzzleCompletedUIActive = true;
        }

    }

    public void ExitPuzzle()  // puzzle minigame end
    {

        GameCamera.enabled = true;
        PuzzleCamera.enabled = false;

        PuzzleCamera.gameObject.SetActive(false);

        puzzleCompletedUI.SetActive(false);
        puzzleCompletedUIActive = false;

        puzzleInPlayUI.SetActive(false);
        puzzleInPlayUIActive = false;

        puzzlePromptUI.SetActive(false);
        puzzlePromptUIActive = false;

        Cursor.lockState = CursorLockMode.Locked;
        controller.ToggleMovement();

        puzzleIsActive = false;
    }
}
