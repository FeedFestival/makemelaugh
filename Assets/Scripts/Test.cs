using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    public RectTransform Panel1;
    public RectTransform Panel2;
    public RectTransform Content;
    public RectTransform Viwport;
    public RectTransform ScrollView;

    // Start is called before the first frame update
    void Start()
    {
        var text = gameObject.GetComponent<Text>();
        text.text = string.Empty;
        text.text += "Panel1" + Panel1.sizeDelta.ToString() + "\n";
        text.text += "Panel2" + Panel2.sizeDelta.ToString() + "\n";

        var contentSizeFitter = Content.GetComponent<ContentSizeFitter>();
        text.text += "Content" + Content.sizeDelta.ToString() + "\n";

        text.text += "Viwport" + Viwport.sizeDelta.ToString() + "\n";
        text.text += "ScrollView" + ScrollView.sizeDelta.ToString() + "\n";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
