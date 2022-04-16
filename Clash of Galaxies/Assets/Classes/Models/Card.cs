using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class Card : INotifyPropertyChanged
    {
        private int gamePoints;
        private Behaviour behaviour = null;

        public Card(string name, int gamePoints, string description)
        {
            Name = name;
            this.gamePoints = gamePoints;
            Description = description;
        }

        public Card(string name, int gamePoints, string description, string nameImg) : this (name, gamePoints, description)
        {
            NameImg = nameImg;
        }

        public Card(string name, int gamePoints, string description, Behaviour behaviour) : this(name, gamePoints, description)
        {
            this.behaviour = behaviour;
        }

        public Card(string name, int gamePoints, string description, string nameImg, Behaviour behaviour) : this(name, gamePoints, description, nameImg)
        {
            this.behaviour = behaviour;
        }

        public event EventHandler Destroy;
        public event PropertyChangedEventHandler PropertyChanged;

        public Behaviour Behaviour { get => behaviour; }
        public string Name { get; }
        public string Description { get; }
        public string NameImg { get; }
        public int InfluenceGamePoints
        {
            get
            {
                if (behaviour != null) return behaviour.InfluencingGamePoints;
                return 0;
            }
        }
        public bool IsActivateBehaviour
        {
            get
            {
                if (behaviour != null) return behaviour.IsActivate;
                return false;
            }
        }
        public int GamePoints
        {
            get
            {
                return gamePoints;
            }
            protected set
            {
                gamePoints = value;
                if (gamePoints <= 0) 
                {
                    OnDestroy();
                    return;
                }
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnDestroy()
        {
            Destroy?.Invoke(this, new EventArgs());
        }

        public Card Clone()
        {
            return new Card(Name, GamePoints, Description, NameImg, behaviour);
        }

        public void InfluenceOnCard(Card targetCard)
        {
            if (behaviour != null && behaviour.IsActivate)
            {
                targetCard.GamePoints += behaviour.InfluencingGamePoints;
            }
        }

        // Test Method
        public void GivePoints(int points)
        {
            GamePoints += points;
        }

        public void ActivateBehaviour()
        {
            behaviour?.Activate(this);
        }
    }
}
