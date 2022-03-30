using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Views
{
    public interface ICardView
    {
        void SetCardInfo(string name, string description, int gamePoints, int influenceGamePoints);
    }
}
