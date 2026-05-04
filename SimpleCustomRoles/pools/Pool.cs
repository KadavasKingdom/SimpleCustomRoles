using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleYaml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YamlDotNet.Serialization.NodeTypeResolvers;

namespace SimpleCustomRoles.Pools
{
    public class Pool
    {
        // Lower inclusive, upper exclusive
        internal List<(int Lower, int Upper, CustomRoleBaseInfo Role)> roles = [];
        internal List<Func<float, float>> AdjustmentFuncs = new();

        internal void AddAdjustmentFunc(Func<float, float> adjustmentFunc)
        {
            AdjustmentFuncs.Add(x =>
            {
                float num = adjustmentFunc(x); 
                if (num < 0 || num >= 1)
                    throw new ArithmeticException($"Adjustment function returned an invalid number, should be [0, 1) but is instead {num}");

                return num;
            });
        }

        float _ChanceToNotGetRole = 1f;

        internal void Print()
        {
            foreach ((int Lower, int Upper, var Role) in roles)
            {
                CL.Info($"{Lower}, {Upper} = {Role}");
                CL.Info($"{_ChanceToNotGetRole}");
            }
        }

        internal void AddRole(CustomRoleBaseInfo role, bool useSpawnAmount = true)
        {
            if (role.Spawn.SpawnChance == 0)
                return;
            
            for (int i = 0; i < (useSpawnAmount ? role.Spawn.SpawnAmount : 1); i++)
            {
                int LowerBound = 0;

                if (!roles.IsEmpty())
                    LowerBound = roles.Last().Upper;

                int UpperBound = LowerBound + role.Spawn.SpawnChance;

                _ChanceToNotGetRole *= 1 - role.Spawn.SpawnChance / 10000f;

                roles.Add((LowerBound, UpperBound, role));
            }
        }

        internal void SortByRarity()
        {
            int Cursor = 0;
            var ListByChance = roles.Select(x => (x.Upper - x.Lower, x.Role)).OrderBy(x => x.Item1).ToList();

            List<(int Lower, int Upper, CustomRoleBaseInfo Role)> NewRoles = new();

            foreach ((int Size, CustomRoleBaseInfo Role) in ListByChance)
            {
                NewRoles.Add((Cursor, Cursor + Size, Role));
                Cursor += Size;
            }
        }

        public CustomRoleBaseInfo? GetRandomRole()
        {
            float randomNumber = RandomGenerator.GetUInt32(true) / 4294967296f;

            return GetRandomRole(randomNumber);
        }

        internal CustomRoleBaseInfo? GetRandomRole(float randomNumber)
        {
            if (randomNumber < 0 || randomNumber >= 1)
                throw new ArgumentException("random number must be between 0 and 1");

            float unadjustedRandom = randomNumber;

            CL.Info($"Original roll: {randomNumber}");
            foreach (var AdjustmentFunc in AdjustmentFuncs)
            {
                randomNumber = AdjustmentFunc(randomNumber);
            }
            CL.Info($"Adjusted roll: {randomNumber}");

            if (randomNumber >= 1 - _ChanceToNotGetRole)
                return null;
            else
                randomNumber /= _ChanceToNotGetRole;

            if (roles.Count == 0)
                return null;

            int adjustedNumber = (int)(randomNumber * roles.Last().Upper);

            int middle = roles.Count / 2;

            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].Lower <= adjustedNumber)
                {
                    middle = i;
                    break;
                }
            }



            // unfortunately I suck w/ algorithms so this doesn't work rn
            // i don't think i can leverage stdlibs binary search either
            // it's okay though linear search is not slow enough for it to matter

            //int highest = roles.Count - 1;
            //int lowest = 0;

            //while (roles[middle].Lower > adjustedNumber || roles[middle].Upper <= adjustedNumber)
            //{
            //    if (roles[middle].Lower > adjustedNumber)
            //    {
            //        lowest = middle;
            //        middle += highest - middle / 2;
            //    }
            //    else
            //    {
            //        highest = middle;
            //        middle -= middle - lowest / 2;
            //    }
            //}

            if (!ValidateRole(middle))
            {
                RemoveRole(middle);
                return GetRandomRole(unadjustedRandom);
            }


            var role = RemoveRole(middle);

            CL.Info($"Selected role {role.Display}");

            return role;
        }

        internal bool ValidateRole(int RolePosition)
        {
            var role = roles[RolePosition].Role;

            if (!CustomRoleHelpers.IsShouldSpawn(role))
            {
                CL.Debug($"Role has been no longer spawn: {role.Rolename} (Reason: Player limited)", Main.Instance.Config.Debug);
                return false;
            }
            if (!GroupHelper.CanSpawn(role.Rolegroup, ref PoolManager.AlreadySpawnedRoles))
            {
                CL.Debug($"Role has been no longer spawn: {role.Rolename} (Reason: Group limited)", Main.Instance.Config.Debug);
                return false;
            }

            return true;
        }

        internal CustomRoleBaseInfo? RemoveRole(CustomRoleBaseInfo Role)
        {
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].Role == Role)
                {
                    return RemoveRole(i);
                }
            }

            return null;
        }

        internal CustomRoleBaseInfo RemoveRole(int RolePos)
        {
            var role = roles[RolePos];
            int middleChance = role.Upper - role.Lower;
            for (int i = RolePos + 1; i < roles.Count; i++)
            {
                int newLower = roles[i].Lower - middleChance;
                int newUpper = roles[i].Upper - middleChance;

                roles[i] = (newLower, newUpper, role.Role);
            }
            roles.RemoveAt(RolePos);

            PoolManager.AlreadySpawnedRoles.Add(role.Role);

            _ChanceToNotGetRole /= 1 - role.Role.Spawn.SpawnChance / 10000f;

            return role.Role;
        }

        internal Pool() { }
    }
}
