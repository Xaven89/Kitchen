using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface IHasProgress
    {
        public event EventHandler<OnProgreesChangedEventArgs> OnProgressChanged;
        
        public class OnProgreesChangedEventArgs : EventArgs
        {
            public float progressNormalized;
        }
    }
}
