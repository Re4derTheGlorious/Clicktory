﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class OutputMachine : Machine
{
    private Gate gate_out;
    private Gate GetGate()
    {
        if (gate_out)
        {
            return gate_out;
        }
        else {
            foreach (GameObject part in parts)
            {
                if (part.GetComponent<Gate>())
                {
                    gate_out = part.GetComponent<Gate>();
                    return part.GetComponent<Gate>();
                }
            }
        }
        return null;
    }

    public List<string> deposit = new List<string>();
    private string FindDeposit()
    {
        if (deposit.Count==0)
        {
            deposit = new List<string>();
            foreach (GameObject part in parts)
            {
                if (part.GetComponent<MachinePart>().tile.deposit.Length > 0)
                {
                    deposit.Add(part.GetComponent<MachinePart>().tile.deposit);
                }
            }
        }

        if (deposit.Count > 0)
        {
            return deposit[Random.Range(0, deposit.Count)];
        }
        else
        {
            return "";
        }
    }

    public override bool CanActivate()
    {
        return base.CanActivate() && !GetGate().IsOccupied();
    }

    public override void EndActivation()
    {
        if (CanEndActivation())
        {
            base.EndActivation();
            string resType = FindDeposit();
            if (resType.Length == 0)
            {
                resType = "Dirt";
            }
            GetGate().res = Globals.GetSave().GetResources().CreateFlowing(resType, 1, GetGate().GetComponent<SpriteRenderer>().sortingOrder + SpriteOrderDirection((GetGate().DirectionRotated() + 2) % 4, GetGate().DirectionRotated()), GetGate().GetComponent<MachinePart>().transform.position);
            GetGate().res.Teleport(GetGate(), 0);
            GetGate().res.Appear();

            Globals.LogStat("Resources extracted", 1);

        }
        activationTimer--;
    }
}
