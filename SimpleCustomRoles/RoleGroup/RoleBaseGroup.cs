namespace SimpleCustomRoles.RoleGroup;

public class RoleBaseGroup
{
    public string Name { get; set; }
    public int MaxRole { get; set; } = -1;
    public float SpawnAmountMultiplier { get; set; } = 1f;
    public float SpawnChanceMultiplier { get; set; } = 1f;
    public List<string> GroupsToDeny { get; set; } = [];
    public List<string> RolesToDeny { get; set; } = [];
}
