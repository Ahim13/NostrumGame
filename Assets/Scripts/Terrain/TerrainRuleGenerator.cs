﻿using System.Collections;
using System.Collections.Generic;
using Endless2DTerrain;
using UnityEngine;


namespace NostrumGames
{
    public class TerrainRuleGenerator
    {
        public TerrainRuleGenerator() { }

        public TerrainRule GenerateRule(TerrainRule.TerrainLength length, float minHeight, float maxHeight, float minSpace, float maxSpace, float calculatedSpace, float ruleLength, float meshLength, float angle)
        {
            TerrainRule newRule = new TerrainRule();
            SetTerrainRule(newRule, length, minHeight, maxHeight, minSpace, maxSpace, calculatedSpace, ruleLength, meshLength, angle);
            return newRule;
        }

        public TerrainRule GenerateRandomRule(TerrainRule.TerrainLength length, float minHeight, float maxHeight, float minSpace, float maxSpace, float calculatedSpace)
        {
            TerrainRule newRule = new TerrainRule();

            var ruleLength = Random.Range(50, 300);
            var meshLength = Random.Range(40, 60);
            int randomAngle = Random.Range(-35, 36);
            int nullAngle = 0;

            // what percentage chance to create randomAngled rule
            int angle = Random.Range(1, 101) <= 65 ? nullAngle : randomAngle;

            SetTerrainRule(newRule, length, minHeight, maxHeight, minSpace, maxSpace, calculatedSpace, ruleLength, meshLength, angle);
            return newRule;
        }

        public void AddToTerrainRules(TerrainRule ruleToAdd)
        {
            TerrainDisplayer.Instance.Rules.Add(ruleToAdd);
        }

        private void SetTerrainRule(TerrainRule rule, TerrainRule.TerrainLength length, float minHeight, float maxHeight, float minSpace, float maxSpace, float calculatedSpace, float ruleLength, float meshLength, float angle)
        {
            //Random always, we never want symmetric sin waves
            rule.SelectedTerrainStyle = TerrainRule.TerrainStyle.Random;

            rule.SelectedTerrainLength = length;
            rule.MinimumKeyVertexHeight = minHeight;
            rule.MaximumKeyVertexHeight = maxHeight;
            rule.MinimumKeyVertexSpacing = minSpace;
            rule.MaximumKeyVertexSpacing = maxSpace;
            rule.CalculatedVertexSpacing = calculatedSpace;
            if (length == TerrainRule.TerrainLength.Fixed) rule.RuleLength = ruleLength;
            rule.MeshLength = meshLength;
            rule.Angle = angle;
        }

    }
}