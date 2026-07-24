using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts.Relics;

[RegisterRelic(typeof(InsatiableRelicPool))]

public class GoldPanRelic : InsatiableRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
        HoverTipFactory.FromCard<GoldRush>()
    ];
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (participants.Contains(base.Owner.Creature) && base.Owner.PlayerCombatState.TurnNumber <= 1)
		{
            CardModel goldRush = base.Owner.Creature.CombatState.CreateCard<GoldRush>(base.Owner);
		    Flash();
            for (int i = 0; i < base.DynamicVars.Cards.BaseValue; i++)
            {
                CardCmd.Preview(goldRush);
                await Cmd.Wait(0.5f);
                await TheInsatiableCmd.SwallowCard(new ThrowingPlayerChoiceContext(), goldRush);
            }
		}
    }
}