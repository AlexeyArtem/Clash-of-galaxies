using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Views
{
    interface IPlayerView
    {
        public Action<ICardView> DropCardCallback { get; set; }
        public void SetCardViews(ICollection<ICardView> cardViews);
    }
}
