using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class GastricAcid : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Gulp),
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Digest)
    ];
	public GastricAcid()
		: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        var swallowPile = CardPile.Get(Entry.SwallowPile, base.Owner);
		if (swallowPile != null)
        {
            var cards = swallowPile.Cards.ToList();
			foreach (var c in cards)
			{
				if (c is InsatiableCardModel icm)
				{
					await icm.OnDigest();
                    await icm.OnGulp();
				}
			}
        }
    }

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}