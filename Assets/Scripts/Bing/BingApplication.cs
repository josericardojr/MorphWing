using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingApplication : MonoBehaviour
{

    public void UpdateXML()
    {
        GameObject.FindObjectOfType<Characters_Player>().SaveProvenance();
    }
    /* esta chamada está sendo feita sempre que o jogador é acertado. A ideia é que esta função faca a requisicao da 
    funcao de teste do processor, e printe aqui o valor de seu resultado*/
    public void FirstTest()
    {
        string pythonReturn = "";
        print(pythonReturn);
    }
}
