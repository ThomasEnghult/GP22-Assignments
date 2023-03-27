public interface IHealth
{
    public float maxHealth { get; set; }
    public float health { get; set; }
    public void LoseHealth(float health);
    public void GainHealth(float health);

}
