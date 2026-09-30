namespace Nursery.Identity.Models.Domain;

public sealed class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid FamilyId { get; private set; }      // one family per login session
    public DateTime FamilyCreatedAtUtc { get; private set; }
    public string TokenHash { get; private set; } = default!;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? UsedAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }
    private RefreshToken() { }

    public static RefreshToken Create(Guid userId, Guid familyId, DateTime familyCreatedAtUtc,   // ← new parameter
          string tokenHash, DateTime nowUtc, TimeSpan lifetime) => new()
          {
              Id = Guid.NewGuid(),
              UserId = userId,
              FamilyId = familyId,
              FamilyCreatedAtUtc = familyCreatedAtUtc,
              TokenHash = tokenHash,
              CreatedAtUtc = nowUtc,
              ExpiresAtUtc = nowUtc.Add(lifetime)
          };

    public bool IsActive => UsedAtUtc is null && RevokedAtUtc is null && ExpiresAtUtc > DateTime.UtcNow;
    public void MarkUsed() => UsedAtUtc = DateTime.UtcNow;
    public void Revoke() => RevokedAtUtc ??= DateTime.UtcNow;
}