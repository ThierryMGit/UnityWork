using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject[] availableTetrominos; // Liste des tetrominos pouvant apparaitre dans le jeu

    public static Transform[,] board;  // Grille de la zone de jeu faisant 10 cases de largeur et 22 cases de hauteur contenant chaque partie d'un tetromino 

    public Transform tetrominosSpawnPosition; // Point d'apparition d'un tetromino

    // Limites de la zone de jeu
    private float _boardLimitLeft;
    private float _boardLimitRight;
    private float _boardLimitBottom;
    private float _boardLimitTop;

    

    private int _score = 0;

    private int _level = 0;

    private int _totalLinesDestroyed = 0;

    


    //** Règles

    // Attribution des points en formant une ou plusieurs lignes d'un coup
    private const int ScoreSimpleLine = 40;
    private const int ScoreDoubleLine = 100;
    private const int ScoreTripleLine = 300;
    private const int ScoreTetris = 1200;

    private int _linesToDestroyPerLevel = 10;// Nombre de lignes à détruire pour passer un niveau
    
    public static float fallTimeInterval = 0.7f; // Intervalle de temps avant la descente d'une case d'un tetromino
    private float _coeffAccelerationFallTimePerLevel = 0.9f; // A chaque niveau les tétrominos descendent plus vite en appliquant ce coefficient

    void Awake()
    {
        // Calcul des limites de la zone de jeu
        _boardLimitLeft = transform.position.x - transform.localScale.x * 0.5f;
        _boardLimitRight = transform.position.x + transform.localScale.x * 0.5f;
        _boardLimitBottom = transform.position.y - transform.localScale.y * 0.5f;
        _boardLimitTop = transform.position.y + transform.localScale.y * 0.5f;

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
        Tetromino.GetComponent<Tetromino>().boardLimitLeft = _boardLimitLeft;
        Tetromino.GetComponent<Tetromino>().boardLimitRight = _boardLimitRight;
        Tetromino.GetComponent<Tetromino>().boardLimitBottom = _boardLimitBottom;
    }

    public void endOfTetrominoFall(GameObject tetrominoShape)
    {
        // Test si le jeu est terminé
        if(IsGameOver(tetrominoShape)) {
            Debug.Log("Fin du jeu");
            return;
        }

        // Sauvegarde des parties du tétromino dans la grille de jeu
        int childQuantity = tetrominoShape.transform.childCount;
        int i = 0;
        while (i < childQuantity) {
            board[(int)tetrominoShape.transform.GetChild(i).transform.position.x, (int)tetrominoShape.transform.GetChild(i).transform.position.y] = tetrominoShape.transform.GetChild(i).transform;
            i++;
        }

        // Gestion des lignes réalisées
        int lineDestroyedQuantity = checkBoardForLinesDestructionAndReturnLinesDestroyedQuantity();

        // Mise à jour du score
        UpdateScore(lineDestroyedQuantity);

        // Mise à jour du niveau
        _totalLinesDestroyed += lineDestroyedQuantity;
        UpdateLevel();

        // Apparition du prochain tétromino
        SpawnTetromino();
    }

    private bool IsGameOver(GameObject tetrominoShape)
    {
        int childQuantity = tetrominoShape.transform.childCount;
        int i = 0;
        
         // Test si toutes les parties du tétromino sont dans la zone de jeu en hauteur
        while(i < childQuantity && tetrominoShape.transform.GetChild(i).transform.position.y < _boardLimitTop) {
            i++;
        }

        return (i != childQuantity);
    }

    private int checkBoardForLinesDestructionAndReturnLinesDestroyedQuantity()
    {
        // Recherche des lignes réalisées / Destruction / Mise à jour du board
        int lineDestroyedQuantity = 0;
        int y = 0;
        while (y < board.GetLength(1)) {
            int x = 0;
            while (x < board.GetLength(0) && board[x, y] != null) {
                x++;
            }

            if (x == board.GetLength(0)) {
                lineDestroyedQuantity++;
                DestroyLine(y);
                UpdateBoardAfterDestroyingLine(y); // Descente d'une case les lignes du dessus
            } else {
                y++;
            }
        }

        return lineDestroyedQuantity;
    }

    private void DestroyLine(int indexLine)
    {   
        for (int x = 0; x < board.GetLength(0); x++) {
            if(board[x, indexLine] != null) {
                // Destruction des parties de tétromino positionnés sur la ligne à détruire
                Destroy(board[x, indexLine].gameObject);
                board[x, indexLine] = null;
            }  
        }
    }

    private void UpdateBoardAfterDestroyingLine(int indexLine)
    {
        if(indexLine + 1 == board.GetLength(1)) {
            return;
        }

        // Parcourt de toutes les lignes au-dessus de celle détruite pour les faire descendre d'une case
        for (int y = indexLine + 1; y < board.GetLength(1); y++) {
            for (int x = 0; x < board.GetLength(0); x++) {
                if (board[x, y] != null) {
                    board[x, y - 1] = board[x, y];
                    board[x, y - 1].transform.position += new Vector3(0, -1, 0);
                    board[x, y] = null;
                }
            }
        }
    }

    private void UpdateScore(int lineDestroyedQuantity)
    {
        switch (lineDestroyedQuantity) {
            case 1:
                _score += ScoreSimpleLine;
                break;
            case 2:
                _score += ScoreDoubleLine;
                break;
            case 3:
                _score += ScoreTripleLine;
                break;
            case 4:
                _score += ScoreTetris;
                break;
            default:
            break;
        }
    }

    private void UpdateLevel() 
    {
        if (_totalLinesDestroyed/((_level + 1) * _linesToDestroyPerLevel) == 1) {
            _level++;
            fallTimeInterval *= _coeffAccelerationFallTimePerLevel;
        }
    }
}
