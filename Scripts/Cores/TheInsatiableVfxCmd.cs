using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using static Godot.Resource;
using static Godot.Tween;

namespace TheInsatiable.Scripts;

public class TheInsatiableVfxCmd
{
    public static void CardFlashVfx(CardModel? card, Creature? target = null)
	{
		if (card == null)
		{
			return;
		}
		NCard node = NCard.Create(card.Owner.Creature.CombatState.CloneCard(card), (ModelVisibility)1);
		NCombatRoom instance = NCombatRoom.Instance;
		if (node == null || instance == null)
		{
			return;
		}
		NCreature creatureNode = instance.GetCreatureNode(card.Owner.Creature);
		if (target != null)
		{
			creatureNode = instance.GetCreatureNode(target);
		}
		if (creatureNode == null)
		{
			return;
		}
		TextureRect node2 = ((Node)node).GetNode<TextureRect>("%Frame");
		Material material = ((CanvasItem)node2).GetMaterial();
		ShaderMaterial mat = (ShaderMaterial)(object)((material is ShaderMaterial) ? material : null);
		if (mat != null)
		{
			Resource obj = ((Resource)mat).DuplicateDeep((DeepDuplicateMode)1);
			((CanvasItem)node2).SetMaterial((Material)(object)((obj is ShaderMaterial) ? obj : null));
		}
		else
		{
			Resource obj2 = ((Resource)card.FrameMaterial).DuplicateDeep((DeepDuplicateMode)1);
			((CanvasItem)node2).SetMaterial((Material)(object)((obj2 is ShaderMaterial) ? obj2 : null));
		}
		Material material2 = ((CanvasItem)node2).GetMaterial();
		mat = (ShaderMaterial)(object)((material2 is ShaderMaterial) ? material2 : null);
		GodotTreeExtensions.AddChildSafely((Node)(object)instance.CombatVfxContainer, (Node)(object)node);
		node.UpdateVisuals((PileType)0, (CardPreviewMode)1);
		node.SetPreviewTarget(card.Owner.Creature);
		((Control)node).GlobalPosition = creatureNode.VfxSpawnPosition;
		((Control)node).Scale = new Vector2(0.7f, 0.7f);
		float num = 0.8f;
		Tween val = ((Node)node).CreateTween();
		Tween val2 = ((Node)node).CreateTween();
		val.TweenProperty((GodotObject)(object)node, "modulate:a", 0f, (double)num).SetEase((EaseType)1).From(1f);
		if (mat != null)
		{
			val2.TweenMethod(Callable.From<float>((Action<float>)delegate(float value)
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				mat.SetShaderParameter("alpha", value);
			}), 1f, 0f, (double)num);
		}
		val.TweenProperty((GodotObject)(object)node, "scale", new Vector2(0.8f, 0.8f), (double)num).SetEase((EaseType)1);
		val.Finished += delegate
		{
			if (GodotObject.IsInstanceValid((GodotObject)(object)node))
			{
				((Node)node).QueueFree();
			}
		};
	}
}