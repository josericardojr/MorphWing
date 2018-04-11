using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Efeitos : MonoBehaviour {


    public enum Effects
    {
        DAMAGE_UP,
        DAMAGE_DOWN,
        SPEED_UP,
        SPEED_DOWN,
        INVERT_CONTROL,
		HEALTH
    }

    [SerializeField]
    private Effects efeitoAtual;

    public Effects GetEfeitoAtual()
    {
        return this.efeitoAtual;
    }

    public void SetEfeitoAtual(Effects effect)
    {
        this.efeitoAtual = effect;
    }

}
