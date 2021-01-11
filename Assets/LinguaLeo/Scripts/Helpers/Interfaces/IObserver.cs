﻿using Managers;

namespace Helpers.Interfaces
{
    public interface IObserver
    {
        #region Events

        void OnNotify(object parametr, GAME_EVENTS notificationName);

        #endregion

        #region Public Methods

        int GetInstanceID();

        #endregion
    }
}
