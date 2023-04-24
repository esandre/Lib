using JetBrains.Annotations;

namespace ConsoleApi.Serialization.Abstractions;

/// <inheritdoc />
/// <summary>
/// Tout contrat doit être un record, donc une classe concrète (le serializer supporte mal les interfaces)
/// et donc, incompatible avec <see cref="JsonSerializationContract"/> qui est aussi un record.
/// </summary>
[UsedImplicitly(ImplicitUseKindFlags.Assign | ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, 
    ImplicitUseTargetFlags.WithMembers | ImplicitUseTargetFlags.WithInheritors)]
public record JsonDeserializationContract;

//TODO STATIC ANALYZER : Les propriétés d'un contrat ne peuvent pas être autre chose que get + init;
//TODO STATIC ANALYZER : Les propriétés d'un contrat sont un type de base ou un autre contrat
