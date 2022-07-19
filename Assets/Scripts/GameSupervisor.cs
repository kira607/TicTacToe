using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSupervisor: MonoBehaviour
{
    public GameObject cross;
    public GameObject circle;
    public GameObject gameOverMenuCanvas;
    
    private bool _gameRunning;
    private bool _crossTurn;
    private Cell[][] _cells;
    private Cell[] _winningCells;

    private void Start()
    {
        gameOverMenuCanvas.SetActive(false);
        _gameRunning = true;
        _crossTurn = true;
        LoadBoard();
    }

    private void Update()
    {
        if (!_gameRunning)
            return;
        var winner = AnalyseBoard();
        if (winner != PieceType.None)
        {
            GameOver(winner);
            _gameRunning = false;
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
        Cell c1, c2, c3;
        // horizontal lines
        for (var i = 0; i < 3; i++)
        {
            c1 = _cells[i][0];
            c2 = _cells[i][1];
            c3 = _cells[i][2];
            if (isWinningLine(new List<Cell> { c1, c2, c3 }))
            {
                _winningCells = new[] { c1, c2, c3 };
                return c1.CurrentPiece();
            }
        }
        // vertical lines
        for (var i = 0; i < 3; i++)
        {
            c1 = _cells[0][i];
            c2 = _cells[1][i];
            c3 = _cells[2][i];
            if (isWinningLine(new List<Cell> { c1, c2, c3 }))
            {
                _winningCells = new[] { c1, c2, c3 };
                return c1.CurrentPiece();
            }
        }
        // diagonal lines
        c1 = _cells[0][0];
        c2 = _cells[1][1];
        c3 = _cells[2][2];
        if (isWinningLine(new List<Cell> { c1, c2, c3 }))
        {
            _winningCells = new[] { c1, c2, c3 };
            return c1.CurrentPiece();
        }
        
        c1 = _cells[2][0];
        c2 = _cells[1][1];
        c3 = _cells[0][2];
        if (isWinningLine(new List<Cell> { c1, c2, c3 }))
        {
            _winningCells = new[] { c1, c2, c3 };
            return c1.CurrentPiece();
        }
        
        // is board is full
        return _cells.SelectMany(cellsRow => cellsRow).Any(cell => !cell.IsBusy()) ? (PieceType)0 : (PieceType)0;
    }

    bool isWinningLine(List<Cell> cells)
    {
        if (cells.Count == 0)
            return false;
        
        var winningPiece = cells[0].CurrentPiece();
        
        foreach (var p in cells.Select(cell => cell.CurrentPiece()))
        {
            if (p == PieceType.None)
            {
                return false;
            }

            if (p != winningPiece)
            {
                return false;
            }
        }

        return true;
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
        LockWinningCells();
        LockCells();
        var winnerString = winner switch
        {
            PieceType.None => "No one",
            PieceType.Cross => "Cross",
            PieceType.Circle => "Circle",
            _ => "Strange Alien"
        };
        foreach (var winningCell in _winningCells)
        {
            winningCell.HighlightWinning();
        }
        Debug.Log(winnerString + " wins!");
        gameOverMenuCanvas.gameObject.SetActive(true);
    }

    private void LockWinningCells()
    {
        foreach (var cell in _winningCells)
        {
            cell.Lock(true);
        }
    }

    private void LockCells()
    {
        foreach (var cellsRow in _cells)
        {
            foreach (var cell in cellsRow)
            {
                cell.Lock(false);
            }
        }
    }
}