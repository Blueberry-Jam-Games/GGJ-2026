using BJ;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mom : MonoBehaviour
{
    [SerializeField]
    private Dialogue dialogue;

    bool loading = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BJ.Coroutines.DoNextFrame(() =>
        {
           GameObject ui = GameObject.FindWithTag("UI");
           DialogueManager dm = ui.GetComponent<DialogueManager>();

           dm.StartDialogue(dialogue); 
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (GameplayManager.Instance.EvaluateCondition("TutorialComplete") && !loading)
        {
            loading = true;
            LevelLoader.Instance.LoadLevel("SchoolGym", "Fade");
        }
    }
}
