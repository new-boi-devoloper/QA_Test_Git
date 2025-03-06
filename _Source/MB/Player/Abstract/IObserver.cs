namespace MB.Player.Abstract
{
    public interface IObserver
    {
        void OnNotify(PlayerAction playerAction);
    }
}