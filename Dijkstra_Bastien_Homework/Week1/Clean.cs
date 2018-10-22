using UnityEngine;

public class Clean : MonoBehaviour {

    public float range = 100f;

    private RaycastHit selected;
    private RaycastHit lastSelected;
    private AudioSource cleanSoundSource;


    private void Start() {
        cleanSoundSource = GetComponent<AudioSource>();
        Physics.Raycast(transform.position,
                        transform.forward, 
                        out selected, 
                        range);
        lastSelected = selected;
    }


    private void Update () {
        lastSelected = selected;

        Physics.Raycast(transform.position, 
                        transform.forward, 
                        out selected, 
                        range);

        if (selected.transform != null) {
            if (selected.transform.tag == "Interactable") {
                SelectMesh(selected.transform.GetComponentsInChildren<Renderer>(), true);

                if (Input.GetMouseButtonDown(0)) {
                    cleanSoundSource.Play();
                    Destroy(selected.transform.gameObject);
                    Physics.Raycast(transform.position, 
                                    transform.forward, 
                                    out selected, 
                                    range);
                }
            }

            if (selected.transform.gameObject != lastSelected.transform.gameObject || selected.transform.tag != "Interactable")
                SelectMesh(lastSelected.transform.GetComponentsInChildren<Renderer>(), false);
        }
    }


    private void SelectMesh(Renderer [] toHighLight, bool select) {
        foreach(Renderer r in toHighLight) {
            if(select)
                r.material.color = new Color(0, 0, 1);
            else
                r.material.color = new Color(1, 1, 1);
        }
    }
}
