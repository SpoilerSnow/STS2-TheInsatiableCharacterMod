using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheInsatiable.Scripts;
public class TheInsatiableKeyword
{
    [CustomEnum("Swallow")]
    [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Swallow;

    [CustomEnum("SelfSwallow")]
    [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword SelfSwallow;

    [CustomEnum("Digest")]
    [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Digest;
}