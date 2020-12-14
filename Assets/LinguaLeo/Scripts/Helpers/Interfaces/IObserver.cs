using LinguaLeo.Scripts.Managers;

namespace LinguaLeo.Scripts.Helpers.Interfaces
{
    public interface IObserver
    {
        #region Events

        void OnNotify(object parametr, GameEvents notificationName);

        #endregion

        #region Public Methods

        int GetInstanceID();

        #endregion
    }
}
