using UnityEngine;

public class Cell : MonoBehaviour
{
    public float highlightSpeed;
    public Color highlightColor = ColorMaker.MakeGrey(60);
    
    private Color _idleColor;
    private GameSupervisor _supervisor;
    private Camera _cam;
    private Renderer _renderer;
    private PieceType _piece;

    public PieceType CurrentPiece()
    {
        return _piece;
    }

    private void Start()
    {
        _cam = Camera.main;
        _renderer = GetComponent<Renderer>();
        _idleColor = CurrentColor();
        _supervisor = transform.parent.GetComponent<GameSupervisor>();
        _piece = PieceType.None;
    }

    private void Update()
    {
        var hit = Raycast();

        if (hit.transform.name != transform.name)
        {
            FadeOut();
            return;
        }

        if (!IsBusy()) FadeIn();

        if (!Input.GetMouseButtonDown(0)) return;
        PutPiece(_supervisor.GetCurrentPiecePrefab());
        _piece = _supervisor.GetCurrentPieceType();
        _supervisor.SwitchTurn();
        FadeOut();
    }

    private void FadeIn()
    {
        _renderer.material.color = Color.Lerp(CurrentColor(), highlightColor, highlightSpeed * Time.deltaTime);
    }
    
    private void FadeOut()
    {
        _renderer.material.color = Color.Lerp(CurrentColor(), _idleColor, highlightSpeed * Time.deltaTime);    
    }

    private void PutPiece(GameObject piece)
    {
        if (IsBusy()) return;
        Instantiate(piece, transform);
    }

    private Color CurrentColor()
    {
        return _renderer.material.color;
    }

    private RaycastHit Raycast()
    {
        var ray = _cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out var hit);
        return hit;
    }

    private bool IsBusy()
    {
        return _piece != PieceType.None;
    }
}