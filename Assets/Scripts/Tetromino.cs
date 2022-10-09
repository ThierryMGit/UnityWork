using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tetromino : MonoBehaviour
{
    // Limites de la zone de jeu
    public float boardLimitLeft;
    public float boardLimitRight;
    public float boardLimitBottom;

    public GameObject Shape; // GameObject contenant les parties (une partie est un carré) d'un tétromino
    public Transform Pivot; // Centre de rotation du tétromino

    private float fallTimer = 0; // Décompte permettant de gérer la descente du tétromino

    private float coeffSpeedFall = 10; // Coefficient d'accélération donné à la descente d'un tétromino lorsque le joueur appuie sur la touche du bas

    private bool tetrominoFalling = true; // Condition pour gérer l'intéraction et la chute du tétromino

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!tetrominoFalling) {
            return;
        }

        // Déplacements horizontaux
        Vector3 move = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            move = new Vector3(-1, 0, 0);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            move = new Vector3(1, 0, 0);
        }

        if(move != Vector3.zero && canMove(move)) {
            transform.position += move;
        }

        // Rotation
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            transform.RotateAround(Pivot.position, Vector3.forward, 90);
             if(!canMove(Vector3.zero)) {
                transform.RotateAround(Pivot.position, Vector3.forward, -90);
            }
        }

        // Chute du tétromino
        if (fallTimer <= 0) {
            if(canMove(new Vector3(0, -1, 0))) {
                transform.position += new Vector3(0, -1, 0);
                fallTimer = Game.fallTimeInterval;
            } else { // Le tétromino a fini sa chute

                // Sauvegarde des parties du tétromino dans la grille de jeu
                int childQuantity = Shape.transform.childCount;
                int i = 0;
                while(i < childQuantity) {
                    Game.board[(int)Shape.transform.GetChild(i).transform.position.x, (int)Shape.transform.GetChild(i).transform.position.y] = Shape.transform.GetChild(i).transform;
                    i++;
                }

                tetrominoFalling = false;

                // Apparition du prochain tétromino
                GameObject.Find("Board").GetComponent<Game>().SpawnTetromino();

                return;
            }
        }

        // Décompte du timer pour gérer la chute avec application du coefficient d'accélération si le joueur appuie sur la touche du bas durant la frame
        fallTimer -= (Time.deltaTime * Mathf.Max(1,(Convert.ToInt32(Input.GetKey(KeyCode.DownArrow)) * coeffSpeedFall))); 
    }

    // Teste la validité des positions des parties du tétromino 
    bool canMove(Vector3 move) 
    {
        int childQuantity = Shape.transform.childCount;
        int i = 0;
            
        bool outOfBound = false; // Test si en dehors de la zone de jeu
        bool tetrominoConflict = false; // Test chevauchage avec un autre tétromino
        while(i < childQuantity && !outOfBound && !tetrominoConflict) {
            // Test si une partie du tétromino est dans la zone de jeu après le déplacement move
            Vector3 childNextPosition = Shape.transform.GetChild(i).transform.position + move;
            outOfBound = isOutOfBound(childNextPosition);

            // Test si la partie du tétromino ne chévauche pas un autre tétromino déjà présent dans la zone de jeu
            if(!outOfBound && (int)childNextPosition.y < Game.board.GetLength(1)) {
                tetrominoConflict = (Game.board[(int)childNextPosition.x, (int)childNextPosition.y] != null);
            }
            
            i++;
        }

        return (!outOfBound && !tetrominoConflict);
    }

    // Teste si une position est dans la zone de jeu
    bool isOutOfBound(Vector3 nextPosition)
    {
        return (nextPosition.x < boardLimitLeft || nextPosition.x > boardLimitRight || nextPosition.y < boardLimitBottom);
    }
}
