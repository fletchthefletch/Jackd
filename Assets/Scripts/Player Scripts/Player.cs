using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerHealth;
    private int playerScore; // Player score is calculated as the total amount of money the player has earned
    private int playerMoney;

    [SerializeField]
    private PlayerCanvas playerUI;
    private PlayListCycler playlist;
    private AudioSource heartbeatSrc;

    void Start()
    {
        setPlayerHealth(playerHealth);
        setPlayerScore(0);
        playlist = FindObjectOfType<PlayListCycler>();
        heartbeatSrc = playlist.getSoundSource("heartbeat", "PLAYER");
        heartbeatSrc.loop = true;
    }
    public void setPlayerScore(int scoreIncrement)
    {
        playerMoney += scoreIncrement;
        playerScore += scoreIncrement;

        // Update main high score record
        if (LevelLoader.getHighScore() < playerScore)
        {
            LevelLoader.setHighScore(playerScore);
        }

        // Update money text
        playerUI.setMoney(playerMoney);
    }
    public int getPlayerScore() 
    {
        return playerScore;
    }
    public void setPlayerHealth(float val)
    {
        // Update health var
        playerHealth = val;
        // Update health UI 
        playerUI.sethealthbar(playerHealth);
        return;
    }
    public float getPlayerHealth()
    {
        return playerHealth;
    }
    public bool takeDamage(float damageAmount) // Returns true if player is still alive; false otherwise
    {

        playlist.playPlayerSound("HurtPlayer", true);
        float res = playerHealth - damageAmount;
        if (res <= 0.5f)
        {
            if (heartbeatSrc.isPlaying)
            {
                // Increase volume
                if (heartbeatSrc.volume <= 0.75f)
                {
                    heartbeatSrc.volume += 0.25f;
                }
            }
            else
            {
                // Start playing clip
                heartbeatSrc.PlayOneShot(heartbeatSrc.clip);
            }
        }
        if (res >= 0f)
        {
            setPlayerHealth(res);
            playlist.playPlayerSound("Pain1", true);
            return true;
        }
        else
        {
            // Player is dead!
            setPlayerHealth(0f);
            return false;
        }
    }
    public void heal(float healAmount)
    {
        float res = playerHealth + healAmount;

        if (heartbeatSrc.isPlaying)
        {
            // Increase volume
            if (heartbeatSrc.volume >= 0.25f)
            {
                heartbeatSrc.volume -= 0.25f;
            }
            if (heartbeatSrc.volume == 0f)
            {

                heartbeatSrc.Stop();
                heartbeatSrc.volume = 0.25f;
            }
        }
        if (res <= 1f)
        {
            setPlayerHealth(res);
            return;
        }
        else
        {
            setPlayerHealth(1f);
            return;
        }
    }
}
