using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Assets.Models
{
    public class PlayerGameResult : INotifyPropertyChanged
    {
        private int roundsWins = 0;
        private int totalGamePoints = 0;

        public int RoundsWins 
        {
            get => roundsWins;
            private set 
            {
                roundsWins = value;
                NotifyPropertyChanged();
            }
        }

        public int TotalGamePoints 
        {
            get 
            {
                return totalGamePoints;
            }
            set 
            {
                totalGamePoints = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void AddRoundWin() => RoundsWins += 1;
    }
}
