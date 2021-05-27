using UnityEngine.UI;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public Player player;
    public GameObject left, middle, right;
    public Sprite active, dead;
    private Image leftImage, middleImage, rightImage;

    private void Awake()
    {
        leftImage = left.GetComponent<Image>();
        rightImage = right.GetComponent<Image>();
        middleImage = middle.GetComponent<Image>();
    }
    void Update()
    {
        Debug.Log(player.Health);
        leftImage.sprite = player.Health > 0 ? active : dead;
        middleImage.sprite = player.Health > 1 ? active : dead;
        rightImage.GetComponent<Image>().sprite = player.Health > 2 ? active : dead;
    }
}
