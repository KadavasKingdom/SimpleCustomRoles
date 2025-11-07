namespace SimpleCustomRoles.RoleYaml;

public class TicketInfo
{
    public MathValueFloat TimeGrantDamage { get; set; } = new();
    public MathValueFloat InfluenceGrantDamage { get; set; } = new();

    public MathValueFloat TimeGrantKill { get; set; } = new();
    public MathValueFloat InfluenceGrantKill { get; set; } = new();
}
