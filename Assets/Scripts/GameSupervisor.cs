using System;
using UnityEngine;

public class GameSupervisor: MonoBehaviour
{
    public GameObject cross;
    public GameObject circle;
    private bool _crossTurn;
    private Cell[][] _cells;

    private void Start()
    {
        _crossTurn = true;
        LoadBoard();
    }

    private void Update()
    {
        var winner = AnalyseBoard();
        if (winner != PieceType.None)
        {
            GameOver(winner);
        }
    }

    public GameObject GetCurrentPiecePrefab()
    {
        return _crossTurn ? cross : circle;
    }

    public PieceType GetCurrentPieceType()
    {
        return _crossTurn ? PieceType.Cross : PieceType.Circle;
    }

    public void SwitchTurn()
    {
        _crossTurn = !_crossTurn;
    }

    private PieceType AnalyseBoard() // -> winning piece type
    {
        // horizontal lines
        for (var i = 0; i < 3; i++)
        {
            var p1 = _cells[i][0].CurrentPiece();
            var p2 = _cells[i][1].CurrentPiece();
            var p3 = _cells[i][2].CurrentPiece();
            if (p1 == p2 && p2 == p3 && p1 == p3 && p1 != PieceType.None)
            {
                return p1;
            }
        }
        // vertical lines
        for (var i = 0; i < 3; i++)
        {
            var p1 = _cells[0][i].CurrentPiece();
            var p2 = _cells[1][i].CurrentPiece();
            var p3 = _cells[2][i].CurrentPiece();
            if (p1 == p2 && p2 == p3 && p1 == p3 && p1 != PieceType.None)
            {
                return p1;
            }
        }
        // diagonal lines
        return 0;
    }

    private void LoadBoard()
    {
        _cells = new Cell[3][];
        for (var i = 0; i < 3; i++)
        {
            _cells[i] = new Cell[3];
            for (var j = 0; j < 3; j++)
            {
                _cells[i][j] = transform.Find("Cell (" + i + ", " + j + ")").gameObject.GetComponent<Cell>();
            }
        }
    }

    private void GameOver(PieceType winner)
    {
        var winnerString = winner switch
        {
            PieceType.None => "No one",
            PieceType.Cross => "Cross",
            PieceType.Circle => "Circle",
            _ => "Strange Alien"
        };
        Debug.Log(winnerString + " wins!");
        Application.Quit();
    }
}