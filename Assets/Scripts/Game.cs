using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject[] availableTetrominos; // Liste des tetrominos pouvant apparaitre dans le jeu

    public static Transform[,] board;  // Grille de la zone de jeu faisant 10 cases de largeur et 22 cases de hauteur contenant chaque partie d'un tetromino 

    public Transform tetrominosSpawnPosition; // Point d'apparition d'un tetromino

    // Limites de la zone de jeu
    private float boardLimitLeft;
    private float boardLimitRight;
    private float boardLimitBottom;

    public static float fallTimeInterval = 0.7f; // Intervalle de temps avant la descente d'une case d'un tetromino


    void Awake()
    {
        // Calcul des limites de la zone de jeu
        boardLimitLeft = transform.position.x - transform.localScale.x * 0.5f;
        boardLimitRight = transform.position.x + transform.localScale.x * 0.5f;
        boardLimitBottom = transform.position.y - transform.localScale.y * 0.5f;

        // Initialisation de la grille de jeu
        board = new Transform[(int)transform.localScale.x, (int)transform.localScale.y];
    }

    // Start is called before the first frame update
    void Start()
    {
        // Apparition d'un tétromino dès le lancement du jeu
        SpawnTetromino();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTetromino()
    {
        // Choix du tetrimino dans la liste de ceux disponibles
        int indexTetrominoToSpawn = Random.Range(0, availableTetrominos.Length);
        
        // Ajout du tétromino dans la zone de jeu
        GameObject Tetromino = Instantiate(availableTetrominos[indexTetrominoToSpawn], tetrominosSpawnPosition.position, Quaternion.identity);

        // Passage des limites de la zone de jeu dans la classe gérant le tetromino
        Tetromino.GetComponent<Tetromino>().boardLimitLeft = boardLimitLeft;
        Tetromino.GetComponent<Tetromino>().boardLimitRight = boardLimitRight;
        Tetromino.GetComponent<Tetromino>().boardLimitBottom = boardLimitBottom;
    }
}
