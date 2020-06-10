using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlateauTurnChoseState : PlateauState
{
    Grenier chosen = null;

    public override void OnEnterState()
    {
        owner.PlayerNumber = (owner.PlayerNumber + 1) % 2;
        Pointer.Instance.PointerStateDelegate += PointerInteract;
        GameManager.Instance.Input.Player.Confirm.performed += ctx => ConfirmAction();

        foreach (Grenier grenier in owner.Greniers)
        {
            if (owner.PlayableChoice(grenier)) {
                grenier.Selectable();
            }
        }

        base.OnEnterState();
    }
    public override void OnExitState()
    {
        Pointer.Instance.PointerStateDelegate -= PointerInteract;
        GameManager.Instance.Input.Player.Confirm.performed -= ctx => ConfirmAction();
        UnChoose();
        foreach (Grenier grenier in owner.Greniers)
        {
            if (grenier.player_number == owner.PlayerNumber) grenier.Unselected();
        }
        base.OnExitState();
    }
    void PointerInteract(bool is_pointing, Collider pointed_collider)
    {
        Grenier grenier;

        if(!is_pointing){
            grenier = pointed_collider.gameObject.GetComponent<Grenier>();
            if (!grenier) return;

            if(chosen && chosen == grenier){
                UnChoose();
            }

            return;
        }

        grenier = pointed_collider.gameObject.GetComponent<Grenier>();
        if (!grenier) return;

        StoredSeeds grenierStorage = grenier.Storage;
        StoredSeeds pointerStorage = Pointer.Instance.GetComponent<StoredSeeds>();
        if (!pointerStorage) return;

        if (pointerStorage.SeedNumber == 0)
        {
            if (owner.PlayableChoice(grenier))
            {
                Choose(grenier);
            }
        }
    }

    void Choose (Grenier grenier) {
        UnChoose();

        chosen = grenier;
        chosen.Selected();
    }

    void UnChoose () {
        if(!chosen) return;

        if (chosen.player_number == owner.PlayerNumber && chosen.Storage.SeedNumber != 0)
        {
            chosen.Selectable();
        }
        else chosen.Unselected();
        chosen = null;
    }

    void ConfirmAction () {
        if(!chosen) return;

        StoredSeeds grenierStorage = chosen.Storage;
        StoredSeeds pointerStorage = Pointer.Instance.GetComponent<StoredSeeds>();
        if (!pointerStorage) return;

        Seed[] seeds = grenierStorage.RemoveAllSeed();
        pointerStorage.AddSeeds(seeds);

        owner.SelectedGrenierId = chosen.Id;
        owner.ChosenGrenierId = chosen.Id;

        SoundBox.Instance.PlaySeedTake();

        owner.State = Plateau.pState.turnPlay; 
    }
}