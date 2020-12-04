using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public static int height = 20;
    public static int width = 10;

    private float previousTime;
    public float fallTime = 0.5f;

    public Vector3 rotationPoint;

    public static Transform[,] grid = new Transform[width, height];

    private GameObject ghostPiece;

    private void Start()
    {
        ghostPiece = transform.GetChild(0).gameObject;
        Instantiate(ghostPiece);
    }

    public void Update()
    {
        // Movement 

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (!ValidMove())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);

            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 14 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);

            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckForLines();

                if (AboveGrid())
                {
                    FindObjectOfType<GameStateManager>().GameOver();
                    return;
                }

                else
                {
                    FindObjectOfType<SpawnBlock>().NewBlock();
                }

                this.enabled = false;
            }

            previousTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (ValidMove())
            {
                transform.position += new Vector3(0, -1, 0);

                if (!ValidMove())
                {
                    transform.position -= new Vector3(0, -1, 0);
                    break;
                }
            }
        }
    }

    public void GhostBlock()
    {
        while (ValidMove())
        {
            ghostPiece.transform.position += new Vector3(0, -1, 0);

            if (!ValidMove())
            {
                ghostPiece.transform.position -= new Vector3(0, -1, 0);
                break;
            }
        }
    }

    public bool AboveGrid()
    {
        for (int x = 0; x < width; x++)
        {
            foreach (Transform children in transform)
            {
                int roundedY = Mathf.RoundToInt(children.transform.position.y);

                if (roundedY > height - 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    private bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }

        return true;
    }

    private void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            StartCoroutine(WaitTime());
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    private void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    StartCoroutine(WaitTime());
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (!AboveGrid())
            {
                grid[roundedX, roundedY] = children;
            }
        }
    }

    private bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.4f);
    }
}
