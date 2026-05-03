using SimpleCustomRoles.RoleYaml;
using System;
using System.Collections.Generic;
using System.Linq;
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
        List<Func<float, float>> _adjustmentFuncs = new();

        internal void AddAdjustmentFunc(Func<float, float> adjustmentFunc)
        {
            _adjustmentFuncs.Add(x =>
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

        internal void AddRole(CustomRoleBaseInfo role)
        {
            for (int i = 0; i < role.Spawn.SpawnAmount; i++)
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

            foreach (var AdjustmentFunc in _adjustmentFuncs)
            {
                randomNumber = AdjustmentFunc(randomNumber);
            }

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

            var role = roles[middle].Role;

            int middleChance = roles[middle].Upper - roles[middle].Lower;

            for (int i = middle + 1; i < roles.Count; i++)
            {
                int newLower = roles[i].Lower - middleChance;
                int newUpper = roles[i].Upper - middleChance;

                roles[i] = (newLower, newUpper, roles[i].Role);
            }
            roles.RemoveAt(middle);

            _ChanceToNotGetRole /= 1 - role!.Spawn.SpawnChance / 10000f;


            return role;
        }

        internal Pool() { }
    }
}
