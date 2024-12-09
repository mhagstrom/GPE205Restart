using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HudHelper : MonoBehaviour
{
    public static HudHelper Instance;
    
    public HudObjects hudObjectsP1;
    public HudObjects hudObjectsP2;

    public GameObject GameOver;
    public GameObject WinScreen;

    public GameObject spStart;
    public GameObject endGame;
    public GameObject coopStart;

    public PlayerController p1;
    public PlayerController p2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTank(PlayerController controller)
    {
        if (controller.PlayerNumber == 0)
        {
            p1 = controller;
        }
        else if (controller.PlayerNumber == 1)
        {
            p2 = controller;
        }
        else
        {
            return;
        }

        TankPawn pawn = controller.GetComponent<TankPawn>();

        pawn.PowerupManager.OnInventoryChanged += UpdateInventory;

        UpdateInventory(pawn.PowerupManager);
    }

    public void SetCoop(bool val)
    {
        hudObjectsP2.gameObject.SetActive(val);
    }

    public void UnsetTank(TankPawn pawn)
    {
        pawn.PowerupManager.OnInventoryChanged -= UpdateInventory;
    }

    private void Update()
    {
        UpdateHud(p1, hudObjectsP1);
        UpdateHud(p2, hudObjectsP2);
    }

    private void UpdateHud(PlayerController p, HudObjects ho)
    {
        if (p == null) return;

        ho.lives.text = $"Lives: {p.Lives}";
        ho.score.text = $"Score: {p.Score}";
        ho.hp.text = $"HP: {p.pawn.Health.CurrentHealth}";
    }

    private void UpdateInventory(PowerupManager target)
    {
        HudObjects objects = target.GetComponent<PlayerController>().PlayerNumber == 0 ? hudObjectsP1 : hudObjectsP2;

        objects.repairKits.text = target.GetPowerupCount(PowerupType.HealthBoost).ToString();
        objects.dmgBoosts.text = target.GetPowerupCount(PowerupType.DamageBoost).ToString();
        objects.speedBoosts.text = target.GetPowerupCount(PowerupType.SpeedBoost).ToString();
    }

    public void ShowGameOver()
    {
        GameOver.SetActive(true);
    }

    public void ShowWinScreen()
    {
        WinScreen.SetActive(true);
    }

    public void EndGame()
    {
        spStart.SetActive(true);
        coopStart.SetActive(true);

        endGame.SetActive(false);

        GameManager.Instance.EndGame();
    }

    public void StartGame(bool coop)
    {
        spStart.SetActive(false);
        coopStart.SetActive(false);
    
        endGame.SetActive(true);

        GameManager.Instance.StartGame(coop);
    }
}
