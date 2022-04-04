using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Views
{
    interface IPlayerView
    {
        Action<ICardView> PlayCurrentCardCallback { get; set; }
        Action<ICardView> DropCardCallback { get; set; }
        void SetCardViews(ICollection<ICardView> cardViews);
    }
}
