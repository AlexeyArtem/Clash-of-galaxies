using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class PermissionMakeMoveEventArgs : EventArgs
    {
        public PermissionMakeMoveEventArgs(Player player, bool isPermissionMakeMove)
        {
            Player = player;
            IsPermissionMakeMove = isPermissionMakeMove;
        }

        public PermissionMakeMoveEventArgs(Player player, bool isPermissionMakeMove, IReadOnlyDictionary<Player, List<Card>> openCards) : this(player, isPermissionMakeMove)
        {
            OpenCards = openCards;
        }

        public Player Player { get; }
        public bool IsPermissionMakeMove { get; }
        public IReadOnlyDictionary<Player, List<Card>> OpenCards { get; }
    }

    public delegate void PermissionMakeMoveEventHandler(object sender, PermissionMakeMoveEventArgs args);
}
