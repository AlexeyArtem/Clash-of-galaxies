using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Views;
using System;

public class PlayerEnemyView : PlayerView
{
    public override void SetCardViews(ICollection<ICardView> cardViews)
    {
        base.SetCardViews(cardViews);

        foreach (var view in cardViews)
            view.SetActiveCardShirt(true);
    }
}
