using System.Collections.Generic;

namespace MB.Player.Abstract
{
    public abstract class ObservablePlayer
    {
        private List<IObserver> _observers;

        private void AddObserver(IObserver observerToSubscribe)
        {
            _observers.Add(observerToSubscribe);
        }

        private void RemoveObserver(IObserver observerToUnSubscribe)
        {
            _observers.Add(observerToUnSubscribe);
        }

        private void NotifyObservers(PlayerAction playerAction)
        {
            foreach (var observer in _observers) observer.OnNotify(playerAction);
        }
    }

    public enum PlayerAction
    {
        Hit,
        TakeHit,
        Jump,
        Heal,
        InDialog,
        InMenu,
        InCombat,
        Died
    }
}