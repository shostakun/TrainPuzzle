public class SaveBoardSettings : Savable
{
    public override void UpdateSaveData(SaveData saveData)
    {
        saveData.boardSize = Board.inst.size;
    }
}
