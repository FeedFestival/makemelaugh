using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PremiseData : MonoBehaviour
{
    public int PremiseId;

    [TextArea(5, 20)]
    public string PremiseText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetPremise()
    {
        if (PremiseId > 0)
        {
            var premise = DomainLogic.DataService.GetPremise(PremiseId);

            ResetPremise();
            PremiseText = premise.Text;
        }
    }

    public void ResetPremise()
    {
        PremiseId = 0;
        PremiseText = "";
    }

    public void DeletePremise()
    {
        if (PremiseId > 0)
        {
            DomainLogic.DataService.GetPremise();

            PremiseId = 0;
        }
    }

    public void SavePremise()
    {
        var premise = new Premise();
        premise.Id = PremiseId > 0 ? PremiseId : 0;
        premise.Text = PremiseText;
        DomainLogic.DataService.SavePremise(premise);

        ResetPremise();
    }
}
