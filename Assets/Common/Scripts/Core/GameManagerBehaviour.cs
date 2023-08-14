using Cinemachine;
using TMPro;
using UnityEngine;

public class GameManagerBehaviour : MonoBehaviour
{
    [SerializeField] private new CinemachineVirtualCamera camera;
    [SerializeField] private PlayerCharacterController playerPrefab;
    [SerializeField] private WeaponAmmo weaponAmmo;
    [SerializeField] private ReloadsIndicator reloadsIndicator;
    [SerializeField] private HPBar hpBar;
    [SerializeField] private WavesController wavesController;
    [SerializeField] private EndGame endGame;
    [SerializeField] private TextMeshProUGUI globalTimer;
    [SerializeField] private PlayerInputHandler inputHandler;

    public static bool IS_GAME_OVER;

    private void Awake()
    {
        var player = Instantiate(playerPrefab, Vector3.one, Quaternion.identity);
        player.Init(inputHandler);
        var playerTransform = player.transform;
        camera.Follow = camera.LookAt = playerTransform;
        wavesController.SetPlayer(playerTransform);
        hpBar.Init(player.Health.MaxHealth);
        weaponAmmo.SetAmmo(player.MaxAmmoCount, player.MaxAmmoCount);

        player.Health.OnDamaged += damage => hpBar.UpdateHitPoints(player.Health.CurrentHealth);
        player.Health.OnDie += GamingLose;
        player.OnShot += currentAmmoCount => weaponAmmo.SetAmmo(currentAmmoCount, player.MaxAmmoCount);
        player.OnReload += (timeInReload, timeReload) => reloadsIndicator.Activate(timeInReload, timeReload);
        player.OnReloadEnded += reloadsIndicator.Deactivate;
        wavesController.TimerUpdatedEvent += ChangeGlobalTimer;
        wavesController.BattleEnd += GameWon;
        IS_GAME_OVER = false;
    }

    private void ChangeGlobalTimer(float time)
    {
        time = Mathf.Round(time);
        globalTimer.text = $"{time}";
    }

    private void GameWon()
    {
        IS_GAME_OVER = true;
        endGame.Won();
    }

    private void GamingLose()
    {
        IS_GAME_OVER = true;
        endGame.Lose();
    }
}