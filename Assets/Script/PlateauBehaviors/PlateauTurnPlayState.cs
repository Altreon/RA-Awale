using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateauTurnPlayState : PlateauState
{
    Grenier chosen = null;

    public override void OnEnterState()
    {
        Pointer.Instance.PointerStateDelegate += PointerInteract;
        GameManager.Instance.Input.Player.Confirm.performed += ctx => ConfirmAction();

        owner.Greniers[(owner.SelectedGrenierId + 1) % 12].Selectable();

        base.OnEnterState();
    }
    public override void OnExitState()
    {
        Pointer.Instance.PointerStateDelegate -= PointerInteract;
        GameManager.Instance.Input.Player.Confirm.performed -= ctx => ConfirmAction();
        UnChoose();

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


        if ((owner.SelectedGrenierId + 1) % 12 != owner.ChosenGrenierId)
        {
            if (grenier.Id == (owner.SelectedGrenierId + 1) % 12) Choose(grenier);
        }
        else
        {
            if (grenier.Id == (owner.SelectedGrenierId + 2) % 12) Choose(grenier);
        }
    }

    void Choose (Grenier grenier) {
        UnChoose();

        chosen = grenier;
        chosen.Selected();
    }

    void UnSelect () {
        if(!chosen) return;

        chosen.Unselected();
        chosen = null;
    }

    void UnChoose () {
        if(!chosen) return;

        chosen.Selectable();
        chosen = null;
    }

    void ConfirmAction () {
        if(!chosen) return;

        StoredSeeds grenierStorage = chosen.Storage;
        StoredSeeds pointerStorage = Pointer.Instance.GetComponent<StoredSeeds>();
        if (!pointerStorage) return;

        Seed seed = pointerStorage.RemoveASeed();
        grenierStorage.AddSeed(seed);

        owner.SelectedGrenierId = chosen.Id;

        UnSelect();

        SoundBox.Instance.PlaySeedPut();


        if (pointerStorage.SeedNumber == 0)
        {
            if(owner.CheckGameOver()){
                owner.State = Plateau.pState.gameOver;
            } else if(owner.PlayerNumber != owner.GetGrenier(owner.SelectedGrenierId).player_number
            && (grenierStorage.SeedNumber == 2 || grenierStorage.SeedNumber == 3)){
                owner.State = Plateau.pState.turnEarn;
            }else{
                owner.State = Plateau.pState.turnChose;
            }
        }
        else
        {
            if ((owner.SelectedGrenierId + 1) % 12 != owner.ChosenGrenierId)
            {
                owner.Greniers[(owner.SelectedGrenierId + 1) % 12].Selectable();
            }
            else owner.Greniers[(owner.SelectedGrenierId + 2) % 12].Selectable();
        }
    }
}