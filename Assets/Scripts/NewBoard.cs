using UnityEngine;

public class NewBoard : MonoBehaviour
{
    public void NewLargeBoard()
    {
        Board.inst.NewBoard(BoardSize.Large);
    }

    public void NewMediumBoard()
    {
        Board.inst.NewBoard(BoardSize.Medium);
    }

    public void NewSmallBoard()
    {
        Board.inst.NewBoard(BoardSize.Small);
    }
}
