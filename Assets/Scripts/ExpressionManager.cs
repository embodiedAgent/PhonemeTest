using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionManager : MonoBehaviour
{
    public float blendSpeed = 1f;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;
    int currentBlendshape;
    float currentBlendshapeValue;
    // Start is called before the first frame update

    public void AcceptExpression(string expression, int expression_value)
    {
        currentBlendshape = skinnedMesh.GetBlendShapeIndex(expression);
        currentBlendshapeValue = skinnedMesh.GetBlendShapeFrameWeight(currentBlendshape,0);
    }
    void Start()
    {
        skinnedMesh = skinnedMeshRenderer.sharedMesh;
        currentBlendshapeValue=-1;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBlendshapeValue!=-1 && currentBlendshapeValue < 100f) 
        {
            skinnedMeshRenderer.SetBlendShapeWeight (currentBlendshape, currentBlendshapeValue);
            currentBlendshapeValue += blendSpeed;
        }
    }
}
