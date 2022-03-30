using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public abstract class Behaviour
    {
        private bool isActivate;
        protected Player owner;
        protected IReadOnlyDictionary<Player, List<Card>> openCards;

        protected Behaviour(Player owner, IReadOnlyDictionary<Player, List<Card>> openCards)
        {
            //if (!openCards.ContainsKey(owner)) 
            //{
            //    throw new ArgumentException("The owner of the behavior is not contained in the dictionary of open cards");
            //}

            this.owner = owner;
            this.openCards = openCards;
        }

        public abstract int InfluencingGamePoints { get; protected set; }
        public bool IsActivate
        {
            get
            {
                return isActivate;
            }
            protected set
            {
                isActivate = value;
                if (isActivate) OnBeginBehaviour();
                else OnEndBehaviour();
            }
        }

        public event EventHandler BeginBehaviour;
        public event EventHandler EndBehaviour;

        private void OnBeginBehaviour()
        {
            BeginBehaviour?.Invoke(this, new EventArgs());
        }

        private void OnEndBehaviour()
        {
            BeginBehaviour?.Invoke(this, new EventArgs());
        }

        public abstract void Activate(Card card);
    }
}
