using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

public abstract class InsatiableCardModel : CustomCardModel, ITheInsatiableModel
{
	public virtual bool HasCustomPortrait => ResourceLoader.Exists($"res://TheInsatiable/images/cards/{GetType().Name}.png");
	public override string? PortraitPath => HasCustomPortrait ? ($"res://TheInsatiable/images/cards/{GetType().Name}.png") : ($"res://TheInsatiable/images/cards/1.png");

	public InsatiableCardModel(int energyCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true) 
		: base(energyCost, type, rarity, target, showInCardLibrary)
	{
	}
	public interface IChoosable
	{
		Task<CardModel?> OnChosen(PlayerChoiceContext choiceContext);
	}
	public static readonly IReadOnlyList<IChoosable> choesnpile1 = new InsatiableCardModel.IChoosable[]
    {
        ModelDb.Card<InsatiableDrawPile>(),
        ModelDb.Card<InsatiableHandPile>(),
        ModelDb.Card<InsatiableDiscardPile>()
    };
	public static readonly IReadOnlyList<IChoosable> choesnpile2 = new InsatiableCardModel.IChoosable[]
    {
        ModelDb.Card<InsatiableDrawPile>(),
        ModelDb.Card<InsatiableDiscardPile>()
    };
	public virtual Task BeforeCardSwallow(CardModel card, bool causedBySelfSwallow)
    {
        return Task.CompletedTask;
    }
    public virtual Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
    {
        return Task.CompletedTask;
    }
    public virtual Task BeforeCreatureSwallow(Creature creature, bool force)
    {
        return Task.CompletedTask;
    }
    public virtual Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force)
    {
        return Task.CompletedTask;
    }
    public virtual bool ShouldSelfSwallowTrigger(ICombatState combatState, CardModel card)
    {
        return true;
	}
    public virtual Task OnTurnEndInHandWrapperSelfSwallow(PlayerChoiceContext choiceContext)
	{
		return Task.CompletedTask;
	}
}
