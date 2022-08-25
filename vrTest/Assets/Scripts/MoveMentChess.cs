using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMentChess : MonoBehaviour
{
    public enum ChessType { ºñ¼ó, ³ªÀÌÆ®, ·è};
    public ChessType chessType;

    Vector3 chagnePositoin;
    Vector3 originPosition;
    [SerializeField] float speed = 2f;

    int chessX, chessY;
    bool endState;

    int minusX = 0, minusY = 0;
    private void Awake()
    {
        originPosition = transform.position;
        switch (chessType)
        {
            case ChessType.ºñ¼ó:
                StartCoroutine(BiShopMoveMent(1, 0));
                break;
            case ChessType.³ªÀÌÆ®:
                StartCoroutine(KnightMoveMent(1 , 0));
                break;
            case ChessType.·è:
                StartCoroutine(RockMoveMent(1, 0));
                break;
            default:
                break;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReStart player = other.GetComponent<ReStart>();
            player.ReStartGame();
        }
    }
    IEnumerator RockMoveMent(int x, int y)
    {
        endState = false;
        chagnePositoin = this.transform.position;
        while (!endState)
        {
            if (x == 1 && y == 0)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(1, 0, 0), speed * Time.deltaTime);
                if (transform.position == chagnePositoin + new Vector3(1, 0, 0))
                {
                    endState = true;
                }
            }
            else if (x == 0 && y == 1)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(0, 0, 1), speed * Time.deltaTime);
                if (transform.position == chagnePositoin + new Vector3(0, 0, 1))
                {
                    endState = true;
                }
            }
            else if (x == -1 && y == 0)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(-1, 0, 0), speed * Time.deltaTime);
                if (transform.position == chagnePositoin + new Vector3(-1, 0, 0))
                {
                    endState = true;
                }
            }
            else if (x == 0 && y == -1)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(0, 0, -1), speed * Time.deltaTime);
                if (transform.position == chagnePositoin + new Vector3(0, 0, -1))
                {
                    endState = true;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(this.transform.position, originPosition, speed * Time.deltaTime);
                if (transform.position == originPosition)
                {
                    endState = true;
                }
            }
            yield return null;
        }

        chessX = Random.Range(-1, 2);
        chessY = Random.Range(-1, 2);

        minusX += chessX;
        minusY += chessY;

        if (minusX == 2)
        {
            chessX -= minusX;
        }

        if (minusY == 2)
        {
            chessY -= minusY;
        }
        if (minusX == -2)
        {
            chessX += minusX;
        }
        if (minusY == -2)
        {
            chessY += minusY;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(RockMoveMent(chessX, chessY));
    }

    IEnumerator BiShopMoveMent(int x, int y)
    {
        endState = false;
        chagnePositoin = this.transform.position;
        while (!endState)
        {
            if (x == 1 && y == 0)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(1, 0, 1), speed * Time.deltaTime);
                if(transform.position == chagnePositoin + new Vector3(1, 0, 1))
                {
                    endState = true;
                }
            }
            else if (x == 2 && y == 1)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(-1, 0, 1), speed * Time.deltaTime);
                if (transform.position == chagnePositoin + new Vector3(-1, 0, 1))
                {
                    endState = true;
                }
            }
            else if (x == 1 && y == 2)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(-1, 0, -1), speed * Time.deltaTime);
                if (transform.position == chagnePositoin + new Vector3(-1, 0, -1))
                {
                    endState = true;
                }
            }
            else if (x == 0 && y == 1)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(1, 0, -1), speed * Time.deltaTime);
                if (transform.position == chagnePositoin + new Vector3(1, 0, -1))
                {
                    endState = true;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(this.transform.position, originPosition, speed * Time.deltaTime);
                if (transform.position == originPosition)
                {
                    endState = true;
                }
            }
            yield return null;
        }
        
        chessX = Random.Range(1, 3);
        chessY = Random.Range(0, 3);

        minusX += chessX;
        minusY += chessY;

        if(minusX == 3)
        {
            chessX -= minusX + 1;
        }

        if (minusY == 4)
        {
            chessY -= minusY + 2;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(BiShopMoveMent(chessX, chessY));
    }

    IEnumerator KnightMoveMent(int x, int y)
    {
        endState = false;
        chagnePositoin = this.transform.position;
        while (!endState)
        {
            if (x == -1)
            {
                if(y == -1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(-2, 0, -1), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(-2, 0, -1))
                    {
                        endState = true;
                    }
                }
                else if(y == 1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(-2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(-2, 0, 1))
                    {
                        endState = true;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(-2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(-2, 0, 1))
                    {
                        endState = true;
                    }
                }
            }
            else if (x == 1)
            {
                if (y == -1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(2, 0, -1), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(2, 0, -1))
                    {
                        endState = true;
                    }
                }
                else if (y == 1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(2, 0, 1))
                    {
                        endState = true;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(2, 0, 1))
                    {
                        endState = true;
                    }
                }
            }
            else if (y == -1)
            {
                if (x == -1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(-2, 0, -1), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(-2, 0, -1))
                    {
                        endState = true;
                    }
                }
                else if (x == 1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(-2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(-2, 0, 1))
                    {
                        endState = true;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(-2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(-2, 0, 1))
                    {
                        endState = true;
                    }
                }
            }
            else if (y == 1)
            {
                if (x == -1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(-1, 0, 2), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(-1, 0, 2))
                    {
                        endState = true;
                    }
                }
                else if (x == 1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(1, 0, 2), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(1, 0, 2))
                    {
                        endState = true;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, chagnePositoin + new Vector3(1, 0, 2), speed * Time.deltaTime);
                    if (transform.position == chagnePositoin + new Vector3(1, 0, 2))
                    {
                        endState = true;
                    }
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(this.transform.position, originPosition, speed * Time.deltaTime);
                if (transform.position == originPosition)
                {
                    endState = true;
                }
            }
            yield return null;
        }

        chessX = Random.Range(-1, 2);
        chessY = Random.Range(-1, 2);
        
        //minusX += chessX;
        //minusY += chessY;

        //if (minusX == 2)
        //{
        //    chessX -= minusX;
        //}

        //if (minusY == 2)
        //{
        //    chessY -= minusY;
        //}
        //if (minusY == -2)
        //{
        //    chessX += minusY;
        //}

        //if (minusX == -2)
        //{
        //    chessY += minusX;
        //}

        yield return new WaitForSeconds(1f);
        StartCoroutine(KnightMoveMent(chessX, chessY));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.GetComponent<ReStart>().ReStartGame();
        }
    }
}
