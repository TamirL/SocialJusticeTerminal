using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialJusticeTerminal.ViewModels
{
    class BaseViewViewModel : BaseViewModel
    {
        #region Events

        public event EventHandler WindowCloseRequested;

        protected virtual void OnWindowCloseRequested()
        {
            EventHandler handler = WindowCloseRequested;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        #endregion
    }
}
